using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using BainsTech.DocMailer.DataObjects;
using BainsTech.DocMailer.Factories;

// http://www.serversmtp.com/en/smtp-yahoo
// https://msdn.microsoft.com/en-us/library/dd633709.aspx

namespace BainsTech.DocMailer.Components
{
    internal class DocumentMailer : IDocumentMailer
    {
        private readonly IConfigurationSettings configurationSettings;
        private readonly IDocumentHandler documentHandler;


        private readonly ILogger logger;
        private readonly IMailMessageAdapterFactory mailMessageAdapterFactory;

        public DocumentMailer(
            IConfigurationSettings configurationSettings,
            ILogger logger,
            IDocumentHandler documentHandler,
            IMailMessageAdapterFactory mailMessageAdapterFactory)
        {
            this.configurationSettings = configurationSettings;
            this.logger = logger;
            this.documentHandler = documentHandler;
            this.mailMessageAdapterFactory = mailMessageAdapterFactory;
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
                var senderEmailAccountPasword = configurationSettings.SenderEmailAccountPassword;
                var recipientEmailAddress = document.EmailAddress;
                var subject = SubjectFromFileName(document.FileName);
                var body = "Email body TBC - " + document.FileName;

                using (var mailMessage = mailMessageAdapterFactory.CreateMailMessageAdapter())
                {
                    mailMessage.SetFromAddress(senderEmailAddress);
                    mailMessage.AddToAdress(recipientEmailAddress);
                    mailMessage.AddBccAddress(senderEmailAddress);
                    mailMessage.AddAttachment(document.FilePath);
                    mailMessage.Subject = subject;
                    mailMessage.Body = body;
                    mailMessage.IsBodyHtml = true;

                    mailMessage.AddAttachment(document.FilePath);

                    using (var smtpClient = mailMessageAdapterFactory.CreateSmtpClientAdapter(smtpAddress, portNumber))
                    {
                        smtpClient.SetCredentials(senderEmailAddress, senderEmailAccountPasword);

                        smtpClient.EnableSssl = enableSsl;
                        
                        document.Status = DocumentStatus.Sending;
                        
                        logger.Info("Sending {0} to {1}...", document.FileName, recipientEmailAddress);
                        smtpClient.Send(mailMessage);
                        
                        document.Status = DocumentStatus.Sent;

                        logger.Info("Sent {0}", document.FileName);
                    }
                }
            }
            catch (Exception ex)
            {
                document.Status = DocumentStatus.SendFailed;
                document.StatusDesc = "Error:" + ex.Message;
            }

            finally
            {
                if (document.Status == DocumentStatus.Sent)
                   documentHandler.MoveDocument(document.FilePath);
            }
        }
    }
}