using System;
using CleverCrow.Fluid.BTs.Trees.Core.Interfaces;

namespace CleverCrow.Fluid.BTs.Trees.Core.Implementations
{
    public class ConsoleLogWriter : BaseLogWriter
    {
        protected override void PrintLog(string finalMessage) => Console.WriteLine(finalMessage);
    }
}
