using System;

using static GiantScape.Common.Logging.Constants;

namespace GiantScape.Common.Logging
{
    public abstract class Logger
    {
        private readonly string logLabel;

        public Logger(string logLabel = "")
        {
            this.logLabel = logLabel;
        }

        public abstract void Log(string message);

        public void Error(string message) => Log(FormatMessage(LOG_ERROR, message));
        public void Warn(string message) => Log(FormatMessage(LOG_WARN, message));
        public void Info(string message) => Log(FormatMessage(LOG_INFO, message));
        public void Debug(string message) => Log(FormatMessage(LOG_DEBUG, message));

        public Logger SubLogger(string label)
        {
            return new SubLogger(this, label);
        }

        protected string FormatMessage(string logLevel, string message)
        {
            return string.Format(LOG_LINE_FORMAT, logLevel, logLabel, GetTimestamp(), message);
        }

        protected static string GetTimestamp()
        {
            return DateTime.Now.ToString("u");
        }
    }
}
