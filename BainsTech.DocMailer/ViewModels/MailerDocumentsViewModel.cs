using System.Collections.ObjectModel;
using System.Windows.Input;
using BainsTech.DocMailer.Components;
using BainsTech.DocMailer.DataObjects;
using BainsTech.DocMailer.Infrastructure;

//using BainsTech.DocMailer.Infrastructure;

//http://stackoverflow.com/questions/29005908/c-sharp-observablecollection-wpf-gridview-binding

namespace BainsTech.DocMailer.ViewModels
{
    internal class MailerDocumentsViewModel : IMailerDocumentsViewModel
    {
        private readonly IConfigurationSettings configurationSettings;
        private readonly IDocumentHandler documentHandler;

        public MailerDocumentsViewModel(IDocumentHandler documentHandler, IConfigurationSettings configurationSettings)
        {
            this.documentHandler = documentHandler;
            this.configurationSettings = configurationSettings;
            Documents = new ObservableCollection<Document>();

            CreateRefreshDocumentsListCommand();
        }

        public ICommand RefreshDocumentsListCommand { get; set; }
        public ObservableCollection<Document> Documents { get; set; }
        
        public void CreateRefreshDocumentsListCommand()
        {
            RefreshDocumentsListCommand = new RelayCommand(RefreshDocumentsList);
        }

        public void RefreshDocumentsList(object val)
        {
            Documents.Clear();
            var documents = documentHandler.GetDocumentsByExtension(
                configurationSettings.DocumentsLocation, configurationSettings.DocumentExtension);

            foreach (var document in documents)
                Documents.Add(document);
        }
    }
}