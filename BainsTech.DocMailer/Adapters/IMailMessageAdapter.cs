using System;
using System.Net.Mail;

namespace BainsTech.DocMailer.Adapters
{
    internal interface IMailMessageAdapter : IDisposable
    {
        void SetFromAddress(string from);
        void AddToAdress(string to);
        void AddBccAddress(string cc);
        string Subject { get; set; }
        string Body { get; set; }
        bool IsBodyHtml { get; set; }
        void AddAttachment(string documentFilePath);
        void AddAttachments(string[] documentFilePaths);
        MailMessage MailMessage { get; }
    }
}
