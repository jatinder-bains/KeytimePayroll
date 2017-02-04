using BainsTech.DocMailer.Adapters;

namespace BainsTech.DocMailer.Factories
{
    internal class MailMessageAdapterFactory : IMailMessageAdapterFactory
    {
        public IMailMessageAdapter CreateMailMessageAdapter()
        {
            return new MailMessageAdapter();
        }

        public ISmtpClientAdapter CreateSmtpClientAdapter(string host, int port, bool testMode)
        {
            if(!testMode)
                return new SmtpClientAdapter(host, port);

            return new MockSmtpClientAdapter();
        }
    }
}