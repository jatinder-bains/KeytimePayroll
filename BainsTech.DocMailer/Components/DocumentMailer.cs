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
        private readonly IDocumentHandler documentHandler;
            
        private readonly ILogger logger;

        public DocumentMailer(IConfigurationSettings configurationSettings, ILogger logger, IDocumentHandler documentHandler)
        {
            this.configurationSettings = configurationSettings;
            this.logger = logger;
            this.documentHandler = documentHandler;
        }

        public string EmailDocuments(IEnumerable<Document> documents)
        {
            Parallel.ForEach(documents, EmailDocument);

            return "";
        }

        private string SubjectFromFileName(string fileName)
        {
            string companyName;
            string type;
            string month;

            var error = documentHandler.ExtractFileNameComponents(fileName, out companyName, out type, out month);

            return string.IsNullOrEmpty(error) ? $"{companyName} {type} {month}" : error;
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
                        logger.Info("IsReadyToSend " + document.FilePath);
                        document.Status = "IsReadyToSend";
                    }
                }
            }
            catch (Exception ex)
                {
                    document.Status = "Error:" + ex.Message;
                }
        }

    }
}
