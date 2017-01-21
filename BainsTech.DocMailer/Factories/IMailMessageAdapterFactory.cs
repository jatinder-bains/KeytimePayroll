using BainsTech.DocMailer.Adapters;

namespace BainsTech.DocMailer.Factories
{
    internal interface IMailMessageAdapterFactory
    {
        IMailMessageAdapter CreateMailMessageAdapter();
        ISmtpClientAdapter CreateSmtpClientAdapter(string host, int port);
    }
}