using System;
using System.IO;
using System.Linq;

namespace BainsTech.DocMailer.Repositories
{
    internal class FailedMovedDocumentsRepository : IFailedMovedDocumentsRepository
    {
        private string dataFile;
        public FailedMovedDocumentsRepository()
        {
            Initialise();
        }

        private void Initialise()
        {
            var appdateFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString() + @"\DocMailer";
            dataFile = appdateFolder + @"\MoveFailedDocs.dat";
            Console.WriteLine(appdateFolder);
            if (!Directory.Exists(appdateFolder))
            {
                Directory.CreateDirectory(appdateFolder);
                CreateFile(dataFile);
                return;
            }

            if (!File.Exists(dataFile))
            {
                CreateFile(dataFile);
            }
        }

        private void CreateFile(string dataFile)
        {
            using (File.CreateText(dataFile))
            {

            }
        }

        public void Add(string document)
        {
            var docs = File.ReadAllLines(dataFile).ToList();
            if (docs.Contains(document.ToLower()))
            {
                return;
            }

            docs.Add(document.ToLower());
            File.WriteAllLines(dataFile, docs.ToArray());
        }

        public bool Contains(string document)
        {
            var docs = File.ReadAllLines(dataFile).ToList();
            return docs.Contains(document.ToLower());
        }

        public void Remove(string document)
        {
            var docs = File.ReadAllLines(dataFile).ToList();
            if (docs.Contains(document.ToLower()))
            {
                docs.Remove(document.ToLower());
                File.WriteAllLines(dataFile, docs.ToArray());
            }
        }

        public string[] Get()
        {
            return File.ReadAllLines(dataFile);
        }
    }
}