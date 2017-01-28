using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Util;
using BainsTech.DocMailer.Adapters;
using BainsTech.DocMailer.DataObjects;

namespace BainsTech.DocMailer.Components
{
    internal class DocumentHandler : IDocumentHandler
    {
        private readonly IConfigurationSettings configurationSettings;
        private readonly ILogger logger;
        private readonly IFileSystemAdapter fileSystemAdapter;

        public DocumentHandler(
            ILogger logger,
            IConfigurationSettings configurationSettings,
            IFileSystemAdapter fileSystemAdapter)
        {
            this.configurationSettings = configurationSettings;
            this.logger = logger;
            this.fileSystemAdapter = fileSystemAdapter;
        }

        public IEnumerable<Document> GetDocumentsByExtension(string folderPath, string extension)
        {
            if (string.IsNullOrEmpty(folderPath)) throw new ArgumentNullException(nameof(folderPath));
            if (string.IsNullOrEmpty(extension)) throw new ArgumentNullException(nameof(extension));

            var docs = new List<Document>();

            try
            {
                var files = fileSystemAdapter.GetFiles(folderPath, extension);

                foreach (var file in files)
                {
                    var fileName = fileSystemAdapter.GetFileName(file);
                    var companyNameKey = fileName?.Split(' ').First();
                    var emailAddress = companyNameKey != null
                        ? configurationSettings.GetEmailForCompany(companyNameKey)
                        : "??";

                    var doc = new Document
                    {
                        FilePath = file,
                        FileName = fileName,
                        EmailAddress = emailAddress,
                    };

                    SetDocumentStatus(doc, emailAddress);
                    docs.Add(doc);
                }
            }
            catch (Exception ex)
            {
                // TODO: Show errors in status bar via ErrorViewModel...
                logger.Error(ex, "GetDocumentsByExtension() encountered exception");
            }

            return docs;
        }

        public bool IsValidFileName(string fileName)
        {
            string companyName;
            string type;
            string month;

            return string.IsNullOrEmpty(ExtractFileNameComponents(fileName, out companyName, out type, out month));
        }

        public bool MoveDocument(string documentFilePath)
        {
            logger.Trace("Moving file " + documentFilePath);
            var moved = false;

            var sentDir = EnsureSentItemsDirectory(documentFilePath);
            if (string.IsNullOrEmpty(sentDir))
            {
                return false;
            }

            var destFileName = sentDir + @"\" + Path.GetFileName(documentFilePath);
            var remainingRetries = 5;
            while (remainingRetries-- != 0)
            {
                try
                {
                    fileSystemAdapter.Move(documentFilePath, destFileName);
                    return true;
                }
                catch(Exception ex)
                {
                    var errMsg = $"Error moving document:{documentFilePath}, remainingRetries:{remainingRetries}";

                    logger.Error(ex, errMsg);
                    if (remainingRetries > 0)
                    {
                        Task.Delay(1000).Wait();
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return false;
        }

        private string EnsureSentItemsDirectory(string documentFilePath)
        {
            var sentDir = fileSystemAdapter.GetDirectoryName(documentFilePath) + @"\Sent";
            if (fileSystemAdapter.Exists(sentDir))
            {
                return sentDir;
            }

            logger.Trace("Creating directory " + documentFilePath);
            try
            {
                fileSystemAdapter.CreateDirectory(sentDir);
                return sentDir;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error creating sent directory:");
                return null;
            }
        }

        public string ExtractFileNameComponents(string fileName, out string companyName, out string type,
            out string month)
        {
            var errMsg = $"Invalid file name is not in suported format 'CompanyName Paye/Payroll dd-mm-yy'.";
            string[] months =
            {
                "January", "February", "March", "April", "May", "June", "July", "August", "September",
                "October", "November", "December"
            };

            companyName = type = month = string.Empty;
            // <CompanyName> Payslips dd-mm-16.pdf
            var elems = fileName.Split(' ');
            if (elems.Length != 3)
                return errMsg;
            companyName = elems[0];
            type = elems[1];
            if (string.Compare(type, "PAYE", StringComparison.OrdinalIgnoreCase) != 0 &&
                string.Compare(type, "PAYROLL", StringComparison.OrdinalIgnoreCase) != 0)
                return errMsg + $"Invalid type '{type}'";

            var dateString = fileSystemAdapter.GetFileNameWithoutExtension(elems[2]);

            DateTime date;
            if (
                !DateTime.TryParseExact(dateString, "dd-mm-yy", CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal,
                    out date))
                return errMsg + $"Invalid date '{dateString}'";

            month = months[date.Month - 1];
            return null; // no errors;
        }

        private void SetDocumentStatus(Document document, string emailAddress)
        {
            string companyName;
            string type;
            string month;

            var incompatibleFileNameError = ExtractFileNameComponents(document.FileName, out companyName, out type, out month);
            var docHasCompatibleFileName = string.IsNullOrEmpty(incompatibleFileNameError);
            var docHasMappedEmail = !string.IsNullOrEmpty(emailAddress);

            if (docHasCompatibleFileName && docHasMappedEmail)
            {
                document.Status = DocumentStatus.ReadyToSend;
                return;
            }
            
            if (!docHasCompatibleFileName)
            {
                document.Status = DocumentStatus.IncompatibleFileName;
                document.StatusDesc = incompatibleFileNameError;
            }

            if (!docHasMappedEmail)
            {
                document.Status = DocumentStatus.NoMappedEmail;
            }
        }
    }
}