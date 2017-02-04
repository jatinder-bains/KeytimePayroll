using System.Windows.Input;

namespace BainsTech.DocMailer.Components
{
    internal interface IConfigurationSettings
    {
        string DocumentsLocation { get; }
        string DocumentExtension { get; }
        string SenderEmailAccountPassword { get; }
        string SenderEmailAddress { get; }
        string SmtpAddress { get; }
        int PortNumber { get; }
        bool EnableSsl { get; }
        bool NoTrump { get; }
        bool TestMode { get; }


        string GetEmailForCompany(string companyName);
        void SetSenderEmailAccountPassword(string encryptedPassword);
    }


}