using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using BainsTech.DocMailer.DataObjects;

namespace BainsTech.DocMailer.Components
{
    internal class DocumentHandler : IDocumentHandler
    {
        private readonly IConfigurationSettings configurationSettings;
        private readonly ILogger logger;

        public DocumentHandler(IConfigurationSettings configurationSettings, ILogger logger)
        {
            this.configurationSettings = configurationSettings;
            this.logger = logger;
        }

        public IEnumerable<Document> GetDocumentsByExtension(string folderPath, string extension)
        {
            if (string.IsNullOrEmpty(folderPath)) throw new ArgumentNullException(nameof(folderPath));
            if (string.IsNullOrEmpty(extension)) throw new ArgumentNullException(nameof(extension));

            var docs = new List<Document>();

            try
            {
                var files = Directory.GetFiles(folderPath, "*." + extension);

                foreach (var file in files)
                {
                    var fileName = Path.GetFileName(file);
                    var companyNameKey = fileName?.Split(' ').First();

                    var doc = new Document
                    {
                        FilePath = file,
                        FileName = fileName,
                        EmailAddress =
                            companyNameKey != null ? configurationSettings.GetEmailForCompany(companyNameKey) : "??"
                    };

                    SetDocumentStatus(doc);
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

            var dateString = Path.GetFileNameWithoutExtension(elems[2]);

            DateTime date;
            if (
                !DateTime.TryParseExact(dateString, "dd-mm-yy", CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal,
                    out date))
                return errMsg + $"Invalid date '{dateString}'";

            month = months[date.Month - 1];
            return string.Empty; // no errors;
        }

        private void SetDocumentStatus(Document document)
        {
            string companyName;
            string type;
            string month;

            var invaidFileNameError = ExtractFileNameComponents(document.FileName, out companyName, out type, out month);
            if (string.IsNullOrEmpty(invaidFileNameError))
            {
                document.IsReadyToSend = true;
                document.Status = "Ready to send";
                return;
            }

            document.IsReadyToSend = false;
            document.Status = invaidFileNameError;
        }
    }
}