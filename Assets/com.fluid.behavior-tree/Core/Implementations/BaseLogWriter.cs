using System;
using CleverCrow.Fluid.BTs.Trees.Core.Interfaces;

namespace CleverCrow.Fluid.BTs.Trees.Core.Implementations
{
    public abstract class BaseLogWriter : ILogWriter
    {
        private const string LogFormat = "[{0}] {1}";
        private const string InfoLevel = "INFO";
        private const string WarningLevel = "WARN";
        private const string ErrorLevel = "ERROR";
        private const string CriticalLevel = "CRITICAL";

        protected string GetLogMessage(string logLevel, string message) => string.Format(LogFormat, logLevel, message);

        public void Log(string message)
        {
            var finalMessage = GetLogMessage(InfoLevel, message);
            PrintLog(finalMessage);
        }

        public void LogWarning(string message)
        {
            var finalMessage = GetLogMessage(WarningLevel, message);
            PrintLog(finalMessage);
        }

        public void LogError(string message)
        {
            var finalMessage = GetLogMessage(ErrorLevel, message);
            PrintLog(finalMessage);
        }

        public void LogException(Exception ex)
        {
            var finalMessage = GetLogMessage(CriticalLevel, $"{ex.Message}\r\n{ex.StackTrace}");
            PrintLog(finalMessage);
        }

        protected abstract void PrintLog(string finalMessage);
    }
}
