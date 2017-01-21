using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using BainsTech.DocMailer.Components;
using BainsTech.DocMailer.DataObjects;
using BainsTech.DocMailer.Infrastructure;

//using BainsTech.DocMailer.Infrastructure;

//http://stackoverflow.com/questions/29005908/c-sharp-observablecollection-wpf-gridview-binding

namespace BainsTech.DocMailer.ViewModels
{
    internal class MailerDocumentsViewModel : IMailerDocumentsViewModel, INotifyPropertyChanged
    {
        private readonly IConfigurationSettings configurationSettings;
        private readonly IDocumentHandler documentHandler;
        private readonly ILogger logger;
        private readonly IDocumentMailer documentMailer;
        
        public MailerDocumentsViewModel(
            IDocumentHandler documentHandler,
            IConfigurationSettings configurationSettings, ILogger logger, IDocumentMailer documentMailer)
        {
            this.documentHandler = documentHandler;
            this.configurationSettings = configurationSettings;
            this.logger = logger;
            this.documentMailer = documentMailer;
            Documents = new ObservableCollection<Document>();
            TotalDocCount = Documents.Count;

            CreateRefreshDocumentsListCommand();
        }

        public ICommand RefreshDocumentsListCommand { get; set; }
        public ICommand MailDocumentsCommand { get; set; }

        public ObservableCollection<Document> Documents { get; set; }

        private int totalDocCount;
        public int TotalDocCount
        {
            get { return totalDocCount; }
            set
            {
                if (value == totalDocCount)
                {
                    return;
                }
                totalDocCount = value;
                OnPropertyChanged();
            }
        }

        private int readyToSendDocCount;
        public int ReadyToSendCount
        {
            get { return readyToSendDocCount; }
            set
            {
                if (value == readyToSendDocCount)
                {
                    return;
                }
                readyToSendDocCount = value;
                OnPropertyChanged();
            }
        }
        
        private int cantSendCount;
        public int CantSendCount
        {
            get { return cantSendCount; }
            set
            {
                if (value == cantSendCount)
                {
                    return;
                }
                cantSendCount = value;
                OnPropertyChanged();
            }
        }

        private int sentCount;
        public int SentCount
        {
            get { return sentCount; }
            set
            {
                if (value == sentCount)
                {
                    return;
                }
                sentCount = value;
                OnPropertyChanged();
            }
        }

        private int sendFailedCount;
        public int SendFailedCount
        {
            get { return sendFailedCount; }
            set
            {
                if (value == sendFailedCount)
                {
                    return;
                }
                sendFailedCount = value;
                OnPropertyChanged();
            }
        }



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

            RefreshCounts();

            logger.Info("MailerDocumentsViewModel.RefreshDocumentsList() - EXIT");
        }

        private void RefreshCounts()
        {
            TotalDocCount = Documents.Count;
            ReadyToSendCount = Documents.Count(d => d.Status == DocumentStatus.ReadyToSend || d.Status == DocumentStatus.SendFailed );
            CantSendCount = Documents.Count(d => d.Status == DocumentStatus.IncompatibleFileName ||
                                                 d.Status == DocumentStatus.NoMappedEmail);
            SentCount = Documents.Count(d => d.Status == DocumentStatus.Sent);
            SendFailedCount = Documents.Count(d => d.Status == DocumentStatus.SendFailed);
        }

        public void MailDocuments(object val)
        {
            documentMailer.EmailDocuments(this.Documents.Where(d => d.IsReadyToSend));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }
    }
}