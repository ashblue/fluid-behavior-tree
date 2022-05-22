using CleverCrow.Fluid.BTs.Trees.Core.Implementations;
using CleverCrow.Fluid.BTs.Trees.Core.Interfaces;

namespace CleverCrow.Fluid.BTs.com.fluid.behavior_tree.Runtime.Core
{
    public static class BehaviorTreeConfiguration
    {
        public static IDependencyResolver DependencyResolver { get; set; }

        static BehaviorTreeConfiguration()
        {
            var resolver = new DummyDependencyResolver();
            resolver.Register<ILogWriter, ConsoleLogWriter>(new ConsoleLogWriter());
            resolver.Register<IRandomHandler, RandomHandler>(new RandomHandler());

            DependencyResolver = resolver;
        }
    }
}
