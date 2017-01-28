using System.IO;

namespace BainsTech.DocMailer.Adapters
{
    public interface IFileSystemAdapter
    {
        string[] GetFiles(string path, string searchPattern);
        string GetFileName(string path);
        void Move(string sourceFileName, string destFileName);
        string GetDirectoryName(string path);
        bool Exists(string path);
        DirectoryInfo CreateDirectory(string path);
        string GetFileNameWithoutExtension(string path);
    }
}