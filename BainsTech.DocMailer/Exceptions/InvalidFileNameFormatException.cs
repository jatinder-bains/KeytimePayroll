using System;

namespace BainsTech.DocMailer.Exceptions
{
    internal class InvalidFileNameFormatException : Exception
    {
        public InvalidFileNameFormatException(string message) : base(message)
        {
            
        }

        public static InvalidFileNameFormatException Create(string fileName)
        {
            var message =
                $"Filename {fileName} doesn't conform expected file naming convention 'CompanyName [Payrol,Paye] dd-mm-yy'";
            return  new InvalidFileNameFormatException(message);
        }
    }
}
