namespace BainsTech.DocMailer.Repositories
{
    internal interface IFailedMovedDocumentsRepository
    {
        void Add(string document);
        bool Contains(string document);
        void Remove(string document);
        string[] Get();
    }
}