using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BainsTech.DocMailer.DataObjects;

namespace BainsTech.DocMailer.Components
{
    internal class DocumentHandler : IDocumentHandler
    {
        private readonly IConfigurationSettings configurationSettings;

        public DocumentHandler(IConfigurationSettings configurationSettings)
        {
            this.configurationSettings = configurationSettings;
        }

        public IEnumerable<Document> GetDocumentsByExtension(string folderPath, string extension)
        {
            if (string.IsNullOrEmpty(folderPath)) throw new ArgumentNullException(nameof(folderPath));
            if (string.IsNullOrEmpty(extension)) throw new ArgumentNullException(nameof(extension));

            var files = Directory.GetFiles(folderPath, "*." + extension);
            
            var docs = new List<Document>();

            foreach (var file in files)
            {
                var fileName = Path.GetFileName(file);
                var companyNameKey = fileName?.Split(' ').First();
                
                var doc = new Document
                {
                    FilePath = file,
                    FileName = fileName,
                    EmailAddress = companyNameKey != null? configurationSettings.GetEmailForCompany(companyNameKey) : "??"
                };
                docs.Add(doc);

            }
            //var docs = files.Select(f => new Document {FileName = f}).ToList();
            return docs;
        }

        
    }
}