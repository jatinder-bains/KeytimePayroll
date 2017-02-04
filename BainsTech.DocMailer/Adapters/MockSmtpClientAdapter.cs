namespace BainsTech.DocMailer.Adapters
{
    internal class MockSmtpClientAdapter : ISmtpClientAdapter
    {
        public void Dispose()
        {
            
        }

        public bool EnableSssl { get; set; }
        public void SetCredentials(string senderEmailAddress, string senderEmailAccountPasswordEncrypted)
        {
            
        }

        public void Send(IMailMessageAdapter mailMessage)
        {
            
        }
    }
}