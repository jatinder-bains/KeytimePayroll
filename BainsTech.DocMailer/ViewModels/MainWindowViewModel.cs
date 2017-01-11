using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BainsTech.DocMailer.Components;

namespace BainsTech.DocMailer.ViewModels
{
    internal class MainWindowViewModel : IMainWindowViewModel
    {
        private readonly IDocumentHandler documentHandler;
        public string StartImportText { get; set; }

        public MainWindowViewModel(IDocumentHandler documentHandler)
        {
            this.documentHandler = documentHandler;
            var c = documentHandler.GetDocumentsByExtension("pdf");
            StartImportText = "Start Mailing " + c.Count();
        }
    }
}
