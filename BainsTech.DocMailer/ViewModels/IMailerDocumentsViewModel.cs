using System.Collections.ObjectModel;
using BainsTech.DocMailer.DataObjects;

namespace BainsTech.DocMailer.ViewModels
{
    internal interface IMailerDocumentsViewModel
    {
        ObservableCollection<Document> Documents { get; set; }
    }
}