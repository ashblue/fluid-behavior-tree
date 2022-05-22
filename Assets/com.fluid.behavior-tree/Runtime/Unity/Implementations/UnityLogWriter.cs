using CleverCrow.Fluid.BTs.Trees.Core.Implementations;
using CleverCrow.Fluid.BTs.Trees.Core.Interfaces;
using UnityEngine;
using System;

namespace CleverCrow.Fluid.BTs.Trees.Runtime.Unity.Implementations
{
    public class UnityLogWriter : ILogWriter
    {
        public void Log(string message) => Debug.Log(message);

        public void LogWarning(string message) => Debug.LogWarning(message);

        public void LogError(string message) => Debug.LogError(message);

        public void LogException(Exception ex) => Debug.LogException(ex);
    }
}
