using System.Collections.Generic;
using System.Security.Cryptography;
using BainsTech.DocMailer.DataObjects;

// http://www.serversmtp.com/en/smtp-yahoo
// https://msdn.microsoft.com/en-us/library/dd633709.aspx

namespace BainsTech.DocMailer.Components
{
    internal class DocumentMailer : IDocumentMailer
    {
        public string EmailDocuments(IEnumerable<Document> documents)
        {
            return "";
        }
    }
}