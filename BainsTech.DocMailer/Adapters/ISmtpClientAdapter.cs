using System;

namespace BainsTech.DocMailer.Adapters
{
    internal interface ISmtpClientAdapter : IDisposable
    {
        bool EnableSssl { get; set; }
        void SetCredentials(string senderEmailAddress, string senderEmailAccountPasswordEncrypted);
        void Send(IMailMessageAdapter mailMessage);
    }
}