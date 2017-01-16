using BainsTech.DocMailer.Components;

namespace BainsTech.DocMailer.ViewModels
{
    internal class PasswordConfigViewModel : IPasswordConfigViewModel
    {
        private readonly IConfigurationSettings configurationSettings;
        
        public void SetSenderEmailAccountPassword(string  encryptedPassword)
        {
             configurationSettings.SetSenderEmailAccountPassword(encryptedPassword);
        }

        public bool IsEmailPasswordNeeded => string.IsNullOrEmpty(configurationSettings.SenderEmailAccountPassword);
        
        public PasswordConfigViewModel(IConfigurationSettings configurationSettings)
        {
            this.configurationSettings = configurationSettings;
        }
    }
}