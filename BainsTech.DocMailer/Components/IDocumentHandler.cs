using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using BainsTech.DocMailer.DataObjects;

namespace BainsTech.DocMailer.Components
{
    internal interface IDocumentHandler
    {
        IEnumerable<Document> GetDocumentsByExtension(string folder, string extension);
    }
}

