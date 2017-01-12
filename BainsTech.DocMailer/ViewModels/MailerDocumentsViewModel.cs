using System.Collections.ObjectModel;
using BainsTech.DocMailer.Components;
using BainsTech.DocMailer.DataObjects;

//http://stackoverflow.com/questions/29005908/c-sharp-observablecollection-wpf-gridview-binding
namespace BainsTech.DocMailer.ViewModels
{
    internal class MailerDocumentsViewModel : IMailerDocumentsViewModel
    {
        private readonly IDocumentHandler documentHandler;
        public ObservableCollection<Document> Documents { get; set; }

        public MailerDocumentsViewModel(IDocumentHandler documentHandler)
        {
            this.documentHandler = documentHandler;
            this.Documents = new ObservableCollection<Document>();

            
        }

        private void Initialise()
        {
            
        }

    }
}