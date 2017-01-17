using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
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
        private readonly ILogger logger;
        private IDocumentMailer documentMailer;
        
        public MailerDocumentsViewModel(
            IDocumentHandler documentHandler,
            IConfigurationSettings configurationSettings, ILogger logger, IDocumentMailer documentMailer)
        {
            this.documentHandler = documentHandler;
            this.configurationSettings = configurationSettings;
            this.logger = logger;
            this.documentMailer = documentMailer;
            Documents = new ObservableCollection<Document>();

            logger.Info("foo");

            CreateRefreshDocumentsListCommand();
        }

        public ICommand RefreshDocumentsListCommand { get; set; }
        public ICommand MailDocumentsCommand { get; set; }

        public ObservableCollection<Document> Documents { get; set; }

        public void CreateRefreshDocumentsListCommand()
        {
            RefreshDocumentsListCommand = new RelayCommand(RefreshDocumentsList);
            MailDocumentsCommand = new RelayCommand(MailDocuments);
        }

        public void RefreshDocumentsList(object val)
        {
            logger.Info("MailerDocumentsViewModel.RefreshDocumentsList() - ENTER");
            Documents.Clear();

            var documents = documentHandler.GetDocumentsByExtension(
                configurationSettings.DocumentsLocation, configurationSettings.DocumentExtension).ToArray();

            logger.Info("Adding {0} documents",  documents.Count());

            foreach (var document in documents)
                Documents.Add(document);

            logger.Info("MailerDocumentsViewModel.RefreshDocumentsList() - EXIT");
        }

        public void MailDocuments(object val)
        {
            documentMailer.EmailDocuments(this.Documents);
            /*
            logger.Info("MailerDocumentsViewModel.MailDocuments() - ENTER");
            Parallel.ForEach(this.Documents, async document =>
            {
                var r = new Random(document.FileName.Length);
                await Task.Delay(r.Next(2, 10) * 1000);
                
                document.SendResult = "Sending...";
                await Task.Delay(r.Next(2,10) * 1000);
                document.SendResult = "Sent";

            });
            logger.Info("MailerDocumentsViewModel.MailDocuments() - EXIT");
            */

        }


    }
}