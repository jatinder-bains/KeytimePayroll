using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Threading.Tasks;
using BainsTech.DocMailer.DataObjects;
using BainsTech.DocMailer.Infrastructure;

// http://www.serversmtp.com/en/smtp-yahoo
// https://msdn.microsoft.com/en-us/library/dd633709.aspx

namespace BainsTech.DocMailer.Components
{
    internal class DocumentMailer : IDocumentMailer
    {
        private readonly IConfigurationSettings configurationSettings;
        private ILogger logger;

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

        private void EmailDocument(Document document)
        {
            var smtpAddress = configurationSettings.SmtpAddress;
            var portNumber = configurationSettings.PortNumber;
            var enableSsl = configurationSettings.EnableSsl;

            var emailFrom = configurationSettings.SenderEmailAddress;
            var login = configurationSettings.SenderEmailAccountLogin;
            var password = configurationSettings.SenderEmailAccountPassword.Decrypt();
            var emailTo = document.EmailAddress;
            var subject = "Test email for " + DateTime.Now.ToLongTimeString();
            var body = "Hello please see attachment " + document.FileName;

            using (var mailMessage = new MailMessage())
            {
                mailMessage.From = new MailAddress(emailFrom);
                mailMessage.To.Add(emailTo);
                mailMessage.Bcc.Add(emailFrom);
                mailMessage.Subject = subject;
                mailMessage.Body = body;
                mailMessage.IsBodyHtml = true; // Can set to false, if you are sending pure text.

                mailMessage.Attachments.Add(new Attachment(document.FilePath));
                //mail.Attachments.Add(new Attachment("C:\\SomeZip.zip"));

                using (var smtpClient = new SmtpClient(smtpAddress, portNumber))
                {
                    smtpClient.Credentials = new NetworkCredential(login, password);
                    smtpClient.EnableSsl = enableSsl;
                    try
                    {
                        logger.Info("Sending " + document.FilePath + "...");
                        smtpClient.Send(mailMessage);
                        logger.Info("Sent " + document.FilePath);
                        document.SendResult = "Sent";
                    }
                    catch (Exception ex)
                    {
                        document.SendResult = "Error:" + ex.Message;
                    }
                }

            }
        }

    }
}
