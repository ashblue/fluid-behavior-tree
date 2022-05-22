using System;

namespace CleverCrow.Fluid.BTs.Trees.Core.Interfaces
{
    public interface ILogWriter
    {
        void Log(string message);
        void LogWarning(string message);
        void LogError(string message);
        void LogException(Exception ex);
    }
}
