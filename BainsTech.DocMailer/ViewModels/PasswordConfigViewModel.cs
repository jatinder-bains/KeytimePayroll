using BainsTech.DocMailer.Components;

namespace BainsTech.DocMailer.ViewModels
{
    internal class PasswordConfigViewModel : IPasswordConfigViewModel
    {
        public PasswordConfigViewModel()
        {
            
        }
        private readonly IConfigurationSettings configurationSettings;

        public string SenderEmailAccountPassword { get; }

        public void SetSenderEmailAccountPassword(string  encryptedPassword)
        {
             configurationSettings.SetSenderEmailAccountPassword(encryptedPassword);
        }

        public bool IsPasswordSet => !string.IsNullOrEmpty(SenderEmailAccountPassword);
        
        public PasswordConfigViewModel(IConfigurationSettings configurationSettings)
        {
            this.configurationSettings = configurationSettings;
        }
    }
}