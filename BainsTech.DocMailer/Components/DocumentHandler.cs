using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BainsTech.DocMailer.DataObjects;

namespace BainsTech.DocMailer.Components
{
    internal class DocumentHandler : IDocumentHandler
    {
        public IEnumerable<Document> GetDocumentsByExtension(string folderPath, string extension)
        {
            if (string.IsNullOrEmpty(folderPath)) throw new ArgumentNullException(nameof(folderPath));
            if (string.IsNullOrEmpty(extension)) throw new ArgumentNullException(nameof(extension));

            //var folder = new DirectoryInfo(folderPath);
            var files = Directory.GetFiles(folderPath, "*." + extension);

            var docs = files.Select(f => new Document {FileName = f}).ToList();
            return docs;
        }
    }
}