using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BainsTech.DocMailer.Components;
using BainsTech.DocMailer.Infrastructure;

namespace BainsTech.DocMailer.ViewModels
{
    internal class MainWindowViewModel : IMainWindowViewModel
    {
        private readonly IDocumentHandler documentHandler;
        private readonly IConfigurationSettings configurationSettings;
        
        public IMailerDocumentsViewModel MailerDocumentsViewModel { get; set; }
        public IPasswordConfigViewModel PasswordConfigViewModel { get; set; }

        public string StartImportText { get; set; }
        public string DocumentsLocation { get; private set; }
        public string DocumentExtension { get; private set; }

        public ICommand SetPasswordCommandMain { get; set; }

        public MainWindowViewModel(
            IDocumentHandler documentHandler,
            IConfigurationSettings configurationSettings,
            IMailerDocumentsViewModel mailerDocumentsViewModel, 
            IPasswordConfigViewModel passwordConfigViewModel)
        {
            this.documentHandler = documentHandler;
            this.configurationSettings = configurationSettings;
            MailerDocumentsViewModel = mailerDocumentsViewModel;
            PasswordConfigViewModel = passwordConfigViewModel;
            Initialise();
        }

        private void Initialise()
        {
            var c = documentHandler.GetDocumentsByExtension(@"C:\tmp\PAYE", "pdf");
            StartImportText = "Start Mailing " + c.Count();
            DocumentsLocation = "Documents Location: " + configurationSettings.DocumentsLocation;
            DocumentExtension = "Document Type: " + configurationSettings.DocumentExtension;
            SetPasswordCommandMain = new RelayCommand(SetPassword);
        }

        private void SetPassword(object obj)
        {
            
        }
    }
}
