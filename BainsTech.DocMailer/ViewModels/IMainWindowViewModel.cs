using System.Security;
using BainsTech.DocMailer.Annotations;

namespace BainsTech.DocMailer.ViewModels
{
    internal interface IMainWindowViewModel
    {
        IPasswordConfigViewModel PasswordConfigViewModel { get; set; }
    }
}