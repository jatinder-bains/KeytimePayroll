using System.Collections.Generic;
using BainsTech.DocMailer.DataObjects;

namespace BainsTech.DocMailer.Components
{
    internal interface IDocumentMailer
    {
        void EmailDocuments(IEnumerable<Document> documents);
    }
}