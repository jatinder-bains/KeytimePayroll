using System;

namespace BainsTech.DocMailer.Components
{
    internal interface ILogger
    {
        void Error(string message);
        void Error(Exception exception, string message);
        void Error(string message, params object[] args);
        void Info(string message);
        void Info( string message, params object[] args);
        void Trace(string message, params object[] args);
        void Trace(string message);
    }
}