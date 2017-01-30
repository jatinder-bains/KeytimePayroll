using System;
using System.Net.Mail;

namespace BainsTech.DocMailer.Adapters
{
    internal class MailMessageAdapter : IMailMessageAdapter
    {
        public MailMessage MailMessage { get; }

        public MailMessageAdapter()
        {
            this.MailMessage = new MailMessage();
        }

        public void SetFromAddress(string from)
        {
            MailMessage.From = new MailAddress(from);
        }

        public void AddToAdress(string to)
        {
            MailMessage.To.Add(to);
        }

        public void AddBccAddress(string bcc)
        {
            MailMessage.Bcc.Add(bcc);
        }

        public string Subject { get { return MailMessage.Subject; } set { MailMessage.Subject = value; } }
        public string Body { get { return MailMessage.Body; } set { MailMessage.Body = value; } }
        public bool IsBodyHtml { get { return MailMessage.IsBodyHtml; } set { MailMessage.IsBodyHtml = value; } }

        public void AddAttachment(string documentFilePath)
        {
            MailMessage.Attachments.Add(new Attachment(documentFilePath));
        }

        public void AddAttachments(string[] documentFilePaths)
        {
            foreach (var documentFilePath in documentFilePaths)
            {
                MailMessage.Attachments.Add(new Attachment(documentFilePath));
            }
        }

        public void Dispose()
        {
            MailMessage?.Dispose();
        }
    }
}