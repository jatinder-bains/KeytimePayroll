using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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

        public void EmailDocuments(IReadOnlyCollection<Document> documents)
        {
            var documentsGroupedByEmail = documents.GroupBy(d => d.EmailAddress).ToList();

            Parallel.ForEach(documentsGroupedByEmail, (g) =>
            {
                EmailDocumentsForSameRecipient(g.ToList());

            });
        }

        private void EmailDocumentsForSameRecipient(IReadOnlyCollection<Document> documents)
        {
            var documentsToSend = documents as Document[] ?? documents.ToArray();
            var recipientEmailAddress = documentsToSend[0].EmailAddress;

            if (documents == null || !documentsToSend.Any()) throw new ArgumentException(@"Parameter is null or empty", nameof(documents));
            if(documentsToSend.Any(d => string.Compare(d.EmailAddress, recipientEmailAddress, StringComparison.CurrentCultureIgnoreCase) != 0))
                throw new ArgumentException(@"Not all documents are for email address {recipientEmailAddress}", nameof(documents));

            var allSent = false;

            try
            {
                
                var smtpAddress = configurationSettings.SmtpAddress;
                var portNumber = configurationSettings.PortNumber;
                var enableSsl = configurationSettings.EnableSsl;

                var senderEmailAddress = configurationSettings.SenderEmailAddress;
                var senderEmailAccountPasword = configurationSettings.SenderEmailAccountPassword;
                
                var subject = BuildSubjectFromDocuments(documents);
                var body = "Email body TBC" ;

                using (var mailMessage = mailMessageAdapterFactory.CreateMailMessageAdapter())
                {
                    mailMessage.SetFromAddress(senderEmailAddress);
                    mailMessage.AddToAdress(recipientEmailAddress);
                    mailMessage.AddBccAddress(senderEmailAddress);
                    mailMessage.Subject = subject;
                    mailMessage.Body = body;
                    mailMessage.IsBodyHtml = true;

                    mailMessage.AddAttachments(documents.Select(d => d.FilePath).ToArray());

                    using (var smtpClient = mailMessageAdapterFactory.CreateSmtpClientAdapter(smtpAddress, portNumber))
                    {
                        smtpClient.SetCredentials(senderEmailAddress, senderEmailAccountPasword);

                        smtpClient.EnableSssl = enableSsl;

                        foreach (var document in documents)
                        {
                            document.Status = DocumentStatus.Sending;
                        }
                        
                        logger.Info($"Sending {documents.Count} to {recipientEmailAddress}...");

                        smtpClient.Send(mailMessage);
                        //Task.Delay(2000).Wait();

                        foreach (var document in documents)
                        {
                            document.Status = DocumentStatus.Sent;
                        }
                        allSent = true;
                        logger.Info($"Sent {documents.Count} to {recipientEmailAddress}.");
                    }
                }
            }
            catch (Exception ex)
            {
                foreach (var document in documents)
                {
                    document.Status = DocumentStatus.SendFailed;
                    document.StatusDesc = "Error:" + ex.Message;
                }
            }

            finally
            {
                if (allSent)
                {
                    foreach (var document in documents)
                    {
                        if (!documentHandler.MoveDocument(document.FilePath))
                        {
                            document.Status = DocumentStatus.SentDocMoveFailed;
                        }
                    }
                }
            }
        }

        private string BuildSubjectFromFileName(string fileName)
        {
            string companyName;
            DocumentType type;
            string month;

            var error = documentHandler.ExtractFileNameComponents(fileName, out companyName, out type, out month);

            return string.IsNullOrEmpty(error) ? $"{companyName} {type} {month}" : error;
        }

        private string BuildSubjectFromDocuments(IReadOnlyCollection<Document> documents)
        {
            if (documents.Count == 0)
            {
                return BuildSubjectFromFileName(documents.First().FileName);
            }
            var countPayeDocs = documents.Count(d => d.DocumentType == DocumentType.Paye);
            var countPayrollDocs = documents.Count(d => d.DocumentType == DocumentType.Payroll);

            if (countPayeDocs == documents.Count)
            {
                return "PAYE documents";
            }

            if (countPayrollDocs == documents.Count)
            {
                return "Payroll documents";
            }

            return "Payroll and PAYE documents";
        }

    }
}