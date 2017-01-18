using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using BainsTech.DocMailer.DataObjects;
using BainsTech.DocMailer.Exceptions;
using BainsTech.DocMailer.Infrastructure;

// http://www.serversmtp.com/en/smtp-yahoo
// https://msdn.microsoft.com/en-us/library/dd633709.aspx

namespace BainsTech.DocMailer.Components
{
    internal class DocumentMailer : IDocumentMailer
    {
        private readonly IConfigurationSettings configurationSettings;
        private readonly ILogger logger;
        private string[] months = {"January","February","March","April", "May", "June", "July", "August", "September", "October", "November", "December"};

        public DocumentMailer(IConfigurationSettings configurationSettings, ILogger logger)
        {
            this.configurationSettings = configurationSettings;
            this.logger = logger;
        }

        public string EmailDocuments(IEnumerable<Document> documents)
        {
            Parallel.ForEach(documents, EmailDocument);

            return "";
        }

        private string SubjectFromFileName(string fileName)
        {
            // <CompanyName> Payslips dd-mm-16.pdf
            var elems = fileName.Split(' ');
            if (elems.Length != 3)
            {
                throw InvalidFileNameFormatException.Create(fileName);
            }
            var companyName = elems[0];
            var type = elems[1];
            if (string.Compare(type, "PAYE", StringComparison.OrdinalIgnoreCase) != 0 &&
                string.Compare(type, "PAYROLL", StringComparison.OrdinalIgnoreCase) != 0)
            {
                throw InvalidFileNameFormatException.Create(fileName);
            }

            var dateString = Path.GetFileNameWithoutExtension(elems[2]);
            DateTime documentDate;
            if (!DateTime.TryParseExact(dateString, "dd-mm-yy", CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out documentDate))
            {
                throw InvalidFileNameFormatException.Create(fileName);
            }

            var month = months[documentDate.Month - 1];

            return $"{companyName} {type} {month}";
        }
        
        private void EmailDocument(Document document)
        {
            try
            {
                var smtpAddress = configurationSettings.SmtpAddress;
                var portNumber = configurationSettings.PortNumber;
                var enableSsl = configurationSettings.EnableSsl;

                var senderEmailAddress = configurationSettings.SenderEmailAddress;
                var senderEmailAccountPasword = configurationSettings.SenderEmailAccountPassword.Decrypt();
                var recipientEmailAddress = document.EmailAddress;
                var subject = SubjectFromFileName(document.FileName);
                var body = "Email body TBC - " + document.FileName;

                using (var mailMessage = new MailMessage())
                {

                    mailMessage.From = new MailAddress(senderEmailAddress);
                    mailMessage.To.Add(recipientEmailAddress);
                    mailMessage.Bcc.Add(senderEmailAddress);
                    mailMessage.Subject = subject;
                    mailMessage.Body = body;
                    mailMessage.IsBodyHtml = true;

                    mailMessage.Attachments.Add(new Attachment(document.FilePath));

                    using (var smtpClient = new SmtpClient(smtpAddress, portNumber))
                    {
                        smtpClient.Credentials = new NetworkCredential(senderEmailAddress, senderEmailAccountPasword);
                        smtpClient.EnableSsl = enableSsl;
                        logger.Info("Sending " + document.FilePath + "...");
                        smtpClient.Send(mailMessage);
                        logger.Info("Sent " + document.FilePath);
                        document.SendResult = "Sent";
                    }
                }
            }
            catch (Exception ex)
            {
                document.SendResult = "Error:" + ex.Message;
            }
        }

    }
}
