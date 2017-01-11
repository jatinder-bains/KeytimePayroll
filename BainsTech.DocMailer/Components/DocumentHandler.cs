using System.Collections.Generic;
using BainsTech.DocMailer.DataObjects;

namespace BainsTech.DocMailer.Components
{
    internal class DocumentHandler : IDocumentHandler
    {
        public IEnumerable<Document> GetDocumentsByExtension(string extension)
        {
            return new Document[]
            {
                new Document {FileName = "Doc1." + extension},
                new Document {FileName = "Doc2." + extension}
            };
        }
    }
}