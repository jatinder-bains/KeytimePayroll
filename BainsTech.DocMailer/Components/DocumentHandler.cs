using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Util;
using BainsTech.DocMailer.Adapters;
using BainsTech.DocMailer.DataObjects;
using BainsTech.DocMailer.Repositories;

namespace BainsTech.DocMailer.Components
{
    internal class DocumentHandler : IDocumentHandler
    {
        private readonly IConfigurationSettings configurationSettings;
        private readonly ILogger logger;
        private readonly IFileSystemAdapter fileSystemAdapter;
        private readonly IFailedMovedDocumentsRepository failedMovedDocumentsRepository;

        public DocumentHandler(
            ILogger logger,
            IConfigurationSettings configurationSettings,
            IFileSystemAdapter fileSystemAdapter, 
            IFailedMovedDocumentsRepository failedMovedDocumentsRepository)
        {
            this.configurationSettings = configurationSettings;
            this.logger = logger;
            this.fileSystemAdapter = fileSystemAdapter;
            this.failedMovedDocumentsRepository = failedMovedDocumentsRepository;
        }

        public IEnumerable<Document> GetDocuments(string folderPath, string extension)
        {
            if (string.IsNullOrEmpty(folderPath)) throw new ArgumentNullException(nameof(folderPath));
            if (string.IsNullOrEmpty(extension)) throw new ArgumentNullException(nameof(extension));

            var docs = new List<Document>();

            try
            {
                var files = fileSystemAdapter.GetFiles(folderPath, extension);

                foreach (var file in files)
                {
                    var doc = GetDocument(file);
                    docs.Add(doc);
                }
            }
            catch (Exception ex)
            {
                // TODO: Show errors in status bar via ErrorViewModel...
                logger.Error(ex, "GetDocuments() encountered exception");
            }

            return docs;
        }

        public bool IsValidFileName(string fileName)
        {
            string companyName;
            DocumentType type;
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
            var sentDir = Path.GetDirectoryName(documentFilePath) + @"\Sent";
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

        public string ExtractFileNameComponents(
            string fileName, 
            out string companyName,
            out DocumentType documentType,
            out string month)
        {
            const int payrollFileElementsCount = 3; // "Payroll CompanyName dd-mm-yy.pdf"
            const int payeFileElementsCount = 4; // "Paye CompanyName jsbainsNiCode dd-mm-yy.pdf" or "Paye CompanyName All dd-mm-yy.pdf"  
            
            string[] months =
            {
                "January", "February", "March", "April", "May", "June",
                "July", "August", "September", "October", "November", "December"
            };

            companyName = month = string.Empty;
            documentType = DocumentType.Unknown;
            
            var fileNameElements = fileName.Split(' ');
            if (fileNameElements.Length != payrollFileElementsCount && fileNameElements.Length != payeFileElementsCount)
            {
                return "Unsupported file name format";
            }

            var documentTypeString= fileNameElements[0];
            if (!Enum.TryParse(documentTypeString, true, out documentType))
            {
                return $"Unsupported document type '{documentTypeString}'";
            }

            companyName = fileNameElements[1];

            var dateElementIndex = (documentType == DocumentType.Payroll) ? 3 : 2;
            var dateString = Path.GetFileNameWithoutExtension(fileNameElements[dateElementIndex]);
            
            DateTime date;
            if (!DateTime.TryParseExact(dateString, "dd-MM-yy", CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out date))
                return $"Invalid date '{dateString}'";

            month = months[date.Month - 1];
            return null; // no errors;
        }

        private Document GetDocument(string fullFilePath)
        {
            var document = new Document();

            string companyName;
            DocumentType type;
            string month;

            var fileName = Path.GetFileName(fullFilePath);

            var incompatibleFileNameError = ExtractFileNameComponents(fileName, out companyName, out type, out month);

            document.FilePath = fullFilePath;
            document.FileName = fileName;
            document.DocumentType = type;
            document.EmailAddress = "??";

            var docHasCompatibleFileName = string.IsNullOrEmpty(incompatibleFileNameError);

            if (docHasCompatibleFileName)
            {
                var emailAddress = configurationSettings.GetEmailForCompany(companyName);
                if (!string.IsNullOrEmpty(emailAddress))
                {
                    document.EmailAddress = emailAddress;

                    document.Status = UnBlacklistDocument(document)
                        ? DocumentStatus.ReadyToSend
                        : DocumentStatus.SentDocMoveFailed;
                }
                else
                {
                    document.EmailAddress = "??";
                    document.Status = DocumentStatus.NoMappedEmail;
                }
            }
            else
            {
                document.Status = DocumentStatus.IncompatibleFileName;
                document.StatusDesc = incompatibleFileNameError;
            }

            return document;
        }

        private bool UnBlacklistDocument(Document document)
        {
            if (!failedMovedDocumentsRepository.Contains(document.FileName))
            {
                return true;
            }

            logger.Trace($"Document {document.FileName} is blacklisted - attempt to move");
            if (MoveDocument(document.FilePath))
            {
                failedMovedDocumentsRepository.Remove(document.FileName);
                logger.Trace($"Document {document.FileName} is removed from blacklist");
                return true;
            }
            logger.Trace($"Faild to remove ddocument {document.FileName} blacklist");
            return false;
        }
        
    }
}