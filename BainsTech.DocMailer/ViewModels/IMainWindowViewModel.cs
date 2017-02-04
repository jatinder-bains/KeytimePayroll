using System.ComponentModel;
using System.Security;

namespace BainsTech.DocMailer.ViewModels
{
    internal interface IMainWindowViewModel: INotifyPropertyChanged
    {
        IPasswordConfigViewModel PasswordConfigViewModel { get; set; }
    }
}