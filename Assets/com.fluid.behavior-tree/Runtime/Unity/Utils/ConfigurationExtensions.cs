using CleverCrow.Fluid.BTs.Trees.Core.Implementations;
using CleverCrow.Fluid.BTs.Trees.Core.Interfaces;
using CleverCrow.Fluid.BTs.Trees.Runtime.Unity.Implementations;

namespace Fluid.BehaviorTree.Runtime.Unity.com.fluid.behavior_tree.Runtime.Unity.Utils
{
    public static class ConfigurationExtensions
    {
        public static void RegisterDefaultUnityImplementations(this DummyDependencyResolver resolver)
        {
            resolver.UnRegister<ILogWriter>();
            resolver.Register<ILogWriter, UnityLogWriter>(new UnityLogWriter());
        }

        public static void RegisterDefaultUnityImplementations(this IDependencyResolver resolver)
        {
            resolver.Register<ILogWriter, UnityLogWriter>(new UnityLogWriter());
        }
    }
}
