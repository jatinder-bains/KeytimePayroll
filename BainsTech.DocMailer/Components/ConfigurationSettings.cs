using System.Configuration;

namespace BainsTech.DocMailer.Components
{
    internal class ConfigurationSettings : IConfigurationSettings
    {
        public string DocumentsLocation { get; }
        public string DocumentExtension { get; }

        public ConfigurationSettings()
        {
            var appSettings = ConfigurationManager.AppSettings;
            DocumentsLocation = appSettings["DocumentsLocation"];
            DocumentExtension = appSettings["DocumentExtension"];
        }
    }
}