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
        private readonly ILogger logger;

        public DocumentHandler(IConfigurationSettings configurationSettings, ILogger logger)
        {
            this.configurationSettings = configurationSettings;
            this.logger = logger;
        }

        public IEnumerable<Document> GetDocumentsByExtension(string folderPath, string extension)
        {
            if (string.IsNullOrEmpty(folderPath)) throw new ArgumentNullException(nameof(folderPath));
            if (string.IsNullOrEmpty(extension)) throw new ArgumentNullException(nameof(extension));

            var docs = new List<Document>();

            try
            {
                var files = Directory.GetFiles(folderPath, "*." + extension);

                foreach (var file in files)
                {
                    var fileName = Path.GetFileName(file);
                    var companyNameKey = fileName?.Split(' ').First();

                    var doc = new Document
                    {
                        FilePath = file,
                        FileName = fileName,
                        EmailAddress =
                            companyNameKey != null ? configurationSettings.GetEmailForCompany(companyNameKey) : "??"
                    };
                    docs.Add(doc);

                }
            }
            catch (Exception ex)
            {
                // TODO: Show errors in status bar via ErrorViewModel...
                logger.Error(ex, "GetDocumentsByExtension() encountered exception");
            }
            
            return docs;
        }

        
    }
}