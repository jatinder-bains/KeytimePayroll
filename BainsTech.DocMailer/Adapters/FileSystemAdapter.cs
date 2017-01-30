using System.IO;

namespace BainsTech.DocMailer.Adapters
{
    internal class FileSystemAdapter : IFileSystemAdapter
    {
        public string[] GetFiles(string path, string searchPattern)
        {
            return Directory.GetFiles(path, "*." + searchPattern);
        }
        
        public void Move(string sourceFileName, string destFileName)
        {
            File.Move(sourceFileName, destFileName);
        }
        
        public bool Exists(string path)
        {
            return Directory.Exists(path);
        }

        public DirectoryInfo CreateDirectory(string path)
        {
            return Directory.CreateDirectory(path);
        }
    }
}