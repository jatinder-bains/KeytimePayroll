using System.Collections.ObjectModel;
using System.Security.Cryptography.X509Certificates;
using BainsTech.DocMailer.DataObjects;

namespace BainsTech.DocMailer.ViewModels
{
    internal interface IMailerDocumentsViewModel
    {
        ObservableCollection<Document> Documents { get; set; }
        
    }
}