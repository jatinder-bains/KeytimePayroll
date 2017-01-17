using System.Security;

namespace BainsTech.DocMailer.ViewModels
{
    internal interface IMainWindowViewModel
    {
        IPasswordConfigViewModel PasswordConfigViewModel { get; set; }
    }
}