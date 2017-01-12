using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BainsTech.DocMailer.Components;

namespace BainsTech.DocMailer.ViewModels
{
    internal class MainWindowViewModel : IMainWindowViewModel
    {
        private readonly IDocumentHandler documentHandler;
        private readonly IConfigurationSettings configurationSettings;

        public string StartImportText { get; set; }
        public string DocumentsLocation { get; private set; }
        public string DocumentExtension { get; private set; }

        public MainWindowViewModel(IDocumentHandler documentHandler, IConfigurationSettings configurationSettings)
        {
            this.documentHandler = documentHandler;
            this.configurationSettings = configurationSettings;
            Initialise();
        }

        private void Initialise()
        {
            var c = documentHandler.GetDocumentsByExtension(@"C:\tmp\PAYE", "pdf");
            StartImportText = "Start Mailing " + c.Count();
            DocumentsLocation = "Documents Location: " + configurationSettings.DocumentsLocation;
            DocumentExtension = "Document Type: " + configurationSettings.DocumentExtension;
        }
    }
}
