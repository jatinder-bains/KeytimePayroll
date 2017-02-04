using System.ComponentModel;
using System.Runtime.CompilerServices;
using BainsTech.DocMailer.Components;
using BainsTech.DocMailer.Properties;

namespace BainsTech.DocMailer.ViewModels
{
    internal class MainWindowViewModel : IMainWindowViewModel
    {
        private readonly IConfigurationSettings configurationSettings;

        private bool testMode;

        private string title;

        public MainWindowViewModel(
            IConfigurationSettings configurationSettings,
            IMailerDocumentsViewModel mailerDocumentsViewModel,
            IPasswordConfigViewModel passwordConfigViewModel)
        {
            this.configurationSettings = configurationSettings;
            MailerDocumentsViewModel = mailerDocumentsViewModel;
            PasswordConfigViewModel = passwordConfigViewModel;
            Initialise();
        }

        public IMailerDocumentsViewModel MailerDocumentsViewModel { get; set; }

        public string StartImportText { get; set; }
        public string DocumentsLocation { get; private set; }
        public string DocumentExtension { get; private set; }
        public string EmailAddress { get; private set; }

        public bool TestMode
        {
            get { return testMode; }
            set
            {
                if (testMode == value) return;
                testMode = value;
                OnPropertyChanged();
                Title = testMode ? "Document Mailer ** TEST MODE ** " : "Document Mailer";
            }
        }

        public string Title
        {
            get { return title; }
            set
            {
                if (title == value) return;
                title = value;
                OnPropertyChanged();
            }
        }

        public IPasswordConfigViewModel PasswordConfigViewModel { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Initialise()
        {
            DocumentsLocation = configurationSettings.DocumentsLocation;
            DocumentExtension = configurationSettings.DocumentExtension;
            EmailAddress = !string.IsNullOrEmpty(configurationSettings.SenderEmailAddress)
                ? configurationSettings.SenderEmailAddress
                : "!! Not set !!";
            TestMode = configurationSettings.TestMode;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}