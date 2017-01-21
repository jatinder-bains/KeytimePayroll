using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using BainsTech.DocMailer.Adapters;
using BainsTech.DocMailer.DataObjects;
using BainsTech.DocMailer.Exceptions;
using BainsTech.DocMailer.Factories;
using BainsTech.DocMailer.Infrastructure;

// http://www.serversmtp.com/en/smtp-yahoo
// https://msdn.microsoft.com/en-us/library/dd633709.aspx

namespace BainsTech.DocMailer.Components
{
    internal class DocumentMailer : IDocumentMailer
    {
        private readonly IConfigurationSettings configurationSettings;
        private readonly IDocumentHandler documentHandler;
        private readonly IMailMessageAdapterFactory mailMessageAdapterFactory;

            
        private readonly ILogger logger;

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
                        
                        document.StatusDesc = "Sending KKK...";
                        document.Status = DocumentStatus.Sending;
                        smtpClient.Send(mailMessage);
                        document.Status = DocumentStatus.Sent;
                    }
                }
            }
            catch (Exception ex)
            {
                document.Status = DocumentStatus.SendFailed;
                document.StatusDesc = "Error:" + ex.Message;
            }
        }

    }
}
