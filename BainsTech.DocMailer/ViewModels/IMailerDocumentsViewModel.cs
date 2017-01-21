using System.Collections.ObjectModel;
using BainsTech.DocMailer.DataObjects;

namespace BainsTech.DocMailer.ViewModels
{
    internal interface IMailerDocumentsViewModel
    {
        ObservableCollection<Document> Documents { get; set; }
        int TotalDocCount { get; set; }
        int ReadyToSendCount { get; set; }
        int CantSendCount { get; set; }
        int SentCount { get; set; }
        int SendFailedCount { get; set; }
    }
}