using System.Net;
using System.Net.Mail;
using BainsTech.DocMailer.Infrastructure;

namespace BainsTech.DocMailer.Adapters
{
    internal class SmtpClientAdapter : ISmtpClientAdapter
    {
        private readonly SmtpClient smtpClient;

        public SmtpClientAdapter(string host, int port)
        {
            this.smtpClient = new SmtpClient(host, port);
        }

        public void SetCredentials(string senderEmailAddress, string senderEmailAccountPasswordEncrypted)
        {
            var pw = senderEmailAccountPasswordEncrypted.Decrypt();
            smtpClient.Credentials = new NetworkCredential(senderEmailAddress, pw);
        }

        public bool EnableSssl
        {
            get { return smtpClient.EnableSsl; }
            set { smtpClient.EnableSsl = value; }
        }

        public void Send(IMailMessageAdapter mailMessageAdapter)
        {
            smtpClient.Send(mailMessageAdapter.MailMessage);
        }

        public void Dispose()
        {
            smtpClient?.Dispose();
        }
    }
}