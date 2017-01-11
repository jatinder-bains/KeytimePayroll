using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BainsTech.DocMailer.DataObjects
{
    public class Document 
    {
        public string FileName { get; set; }
        public string EmailAddress { get; set; }
        public bool Sent { get; set; }
        public string SendError { get; set; }
    }
}
