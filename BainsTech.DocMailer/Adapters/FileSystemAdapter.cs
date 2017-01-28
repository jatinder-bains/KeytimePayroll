using System.IO;

namespace BainsTech.DocMailer.Adapters
{
    public class FileSystemAdapter : IFileSystemAdapter
    {
        public string[] GetFiles(string path, string searchPattern)
        {
            return Directory.GetFiles(path, "*." + searchPattern);
        }

        public string GetFileName(string path)
        {
            return Path.GetFileName(path);
        }

        public void Move(string sourceFileName, string destFileName)
        {
            File.Move(sourceFileName, destFileName);
        }

        public string GetDirectoryName(string path)
        {
            return Path.GetDirectoryName(path);
        }

        public bool Exists(string path)
        {
            return Directory.Exists(path);
        }

        public DirectoryInfo CreateDirectory(string path)
        {
            return Directory.CreateDirectory(path);
        }

        public string GetFileNameWithoutExtension(string path)
        {
            return Path.GetFileNameWithoutExtension(path);
        }
    }
}