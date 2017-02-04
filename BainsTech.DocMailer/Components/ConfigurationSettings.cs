using System;
using System.Configuration;
using BainsTech.DocMailer.Infrastructure;

namespace BainsTech.DocMailer.Components
{
    internal class ConfigurationSettings : IConfigurationSettings
    {
        private const string SenderEmailAccountPasswordKey = "SenderEmailAccountPassword";

        public string DocumentsLocation { get; }
        public string DocumentExtension { get; }

        public string SenderEmailAccountPassword => ConfigurationManager.AppSettings[SenderEmailAccountPasswordKey];

        public string SenderEmailAddress { get; }
        public string SmtpAddress { get; }
        public int PortNumber { get; }
        public bool EnableSsl { get; }
        public bool NoTrump { get; }
        public bool TestMode { get; }

        public ConfigurationSettings()
        {
            var appSettings = ConfigurationManager.AppSettings;
            DocumentsLocation = appSettings["DocumentsLocation"];
            DocumentExtension = appSettings["DocumentExtension"];
            SenderEmailAddress = appSettings["SenderEmailAddress"];
            SmtpAddress = appSettings["SmtpAddress"];
            PortNumber = int.Parse(appSettings["PortNumber"]);
            EnableSsl = bool.Parse(appSettings["EnableSsl"]);
            TestMode = bool.Parse(appSettings["TestMode"]);
            try
            {
                NoTrump = bool.Parse(appSettings["NoTrump"]);
            }
            catch
            {
                NoTrump = false;
            }
        }

        public string GetEmailForCompany(string companyName)
        {
            return ConfigurationManager.AppSettings[companyName];
        }

        public void SetSenderEmailAccountPassword(string encryptedPassword)
        {
            UpdateSetting(SenderEmailAccountPasswordKey, encryptedPassword);
            /*
            var appSettings = ConfigurationManager.AppSettings;
            appSettings[SenderEmailAccountPasswordKey] = encryptedPassword;
            ConfigurationManager.RefreshSection("appSettings");
            */
        }

        private static void UpdateSetting(string key, string value)
        {
            var configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            configuration.AppSettings.Settings[key].Value = value;
            configuration.Save();

            ConfigurationManager.RefreshSection("appSettings");
        }
    }
}