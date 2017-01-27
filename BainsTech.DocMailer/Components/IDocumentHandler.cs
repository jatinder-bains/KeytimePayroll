using System.Collections.Generic;
using BainsTech.DocMailer.DataObjects;

namespace BainsTech.DocMailer.Components
{
    internal interface IDocumentHandler
    {
        IEnumerable<Document> GetDocumentsByExtension(string folder, string extension);
        string ExtractFileNameComponents(string fileName, out string companyName, out string type, out string month);
        bool IsValidFileName(string fileName);
        bool MoveDocument(string documentFilePath);
    }
}