using System.Collections.Generic;
using BainsTech.DocMailer.DataObjects;

namespace BainsTech.DocMailer.Components
{
    internal interface IDocumentMailer
    {
        string EmailDocuments(IEnumerable<Document> documents);
    }
}