using System.Collections.Generic;
using BainsTech.DocMailer.DataObjects;

namespace BainsTech.DocMailer.Components
{
    internal interface IDocumentHandler
    {
        IEnumerable<Document> GetDocuments(string folder, string extension);
        string ExtractFileNameComponents(string fileName, out string companyName, out DocumentType type, out string month);
        bool IsValidFileName(string fileName);
        bool MoveDocument(string documentFilePath);
    }
}