using System;
using NLog;

namespace BainsTech.DocMailer.Components
{
    internal class Logger : ILogger
    {
        private static readonly NLog.Logger AppLogger = LogManager.GetLogger("DocMailer");

        public Logger()
        {
            AppLogger.Info("==== DocMailer starting ====");
        }
        public void Error(string message)
        {
            AppLogger.Error(message);
        }

        public void Error(Exception exception, string message )
        {
            AppLogger.Error(exception, message);
        }

        public void Error(string message, params object[] args)
        {
            AppLogger.Error(message, args);
        }

        public void Info(string message)
        {
            AppLogger.Info(message);
        }

        public void Info(string message, params object[] args)
        {
            AppLogger.Info(message, args);
        }

        public void Trace(string message, params object[] args)
        {
            AppLogger.Trace(message, args);
        }

        public void Trace(string message)
        {
            AppLogger.Trace(message);
        }
    }
}