using System.Configuration;


namespace BainsTech.DocMailer.Components
{
    internal class ConfigurationSettings : IConfigurationSettings
    {
        const string SenderEmailAccountPasswordKey = "SenderEmailAccountPassword";
        public string DocumentsLocation { get; }
        public string DocumentExtension { get; }
        public string SenderEmailAccountPassword { get;}
        public string SenderEmailAddress { get; }
        
        public string GetEmailForCompany(string companyName)
        {
            return ConfigurationManager.AppSettings[companyName];
        }

        public ConfigurationSettings()
        {
            var appSettings = ConfigurationManager.AppSettings;
            DocumentsLocation = appSettings["DocumentsLocation"];
            DocumentExtension = appSettings["DocumentExtension"];
            SenderEmailAccountPassword = appSettings[SenderEmailAccountPasswordKey];
            SenderEmailAddress = appSettings["SenderEmailAddress"];
        }

        public void SetSenderEmailAccountPassword(string encryptedPassword)
        {
            //var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var appSettings = ConfigurationManager.AppSettings;
            appSettings[SenderEmailAccountPasswordKey] = encryptedPassword;
            ConfigurationManager.RefreshSection("appSettings");
        }

    }
}