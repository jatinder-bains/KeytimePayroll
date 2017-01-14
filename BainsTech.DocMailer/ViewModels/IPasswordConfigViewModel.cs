using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BainsTech.DocMailer.ViewModels
{
    internal interface IPasswordConfigViewModel
    {
        string SenderEmailAccountPassword { get;  }
        bool IsPasswordSet { get; }
        void SetSenderEmailAccountPassword(string encryptedPassword);
    }
}
