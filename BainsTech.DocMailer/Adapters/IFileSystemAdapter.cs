using System.IO;

namespace BainsTech.DocMailer.Adapters
{
    internal interface IFileSystemAdapter
    {
        string[] GetFiles(string path, string searchPattern);
        void Move(string sourceFileName, string destFileName);
        bool Exists(string path);
        DirectoryInfo CreateDirectory(string path);
    }
}