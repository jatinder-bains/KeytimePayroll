using System.ComponentModel;
using System.Runtime.CompilerServices;
using BainsTech.DocMailer.Components;
using BainsTech.DocMailer.Properties;

namespace BainsTech.DocMailer.ViewModels
{
    internal class PasswordConfigViewModel : IPasswordConfigViewModel, INotifyPropertyChanged
    {
        private readonly IConfigurationSettings configurationSettings;
        
        public void SetSenderEmailAccountPassword(string  encryptedPassword)
        {
             configurationSettings.SetSenderEmailAccountPassword(encryptedPassword);
        }

        private bool isEmailPasswordNeeded;
        public bool IsEmailPasswordNeeded
        {
            get
            {
                isEmailPasswordNeeded = string.IsNullOrEmpty(configurationSettings.SenderEmailAccountPassword);
                return isEmailPasswordNeeded;
            }
            set
            {
                if (isEmailPasswordNeeded == value)
                {
                    return;
                }
                isEmailPasswordNeeded = value;
                OnPropertyChanged();
            }
        }
        
        public PasswordConfigViewModel(IConfigurationSettings configurationSettings)
        {
            this.configurationSettings = configurationSettings;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}