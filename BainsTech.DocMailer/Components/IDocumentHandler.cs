using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BainsTech.DocMailer.DataObjects;

namespace BainsTech.DocMailer.Components
{
    interface IDocumentHandler
    {
        IEnumerable<Document> GetDocumentsByExtension(string extension);
    }
}

