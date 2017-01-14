using System.Windows.Input;

namespace BainsTech.DocMailer.Components
{
    internal interface IConfigurationSettings
    {
        string DocumentsLocation { get; }
        string DocumentExtension { get; }
        string SenderEmailAccountPassword { get; }
        string SenderEmailAddress { get; }
        
        string GetEmailForCompany(string companyName);
        void SetSenderEmailAccountPassword(string encryptedPassword);
    }


}