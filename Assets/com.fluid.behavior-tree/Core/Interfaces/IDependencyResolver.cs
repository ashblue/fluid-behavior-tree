namespace CleverCrow.Fluid.BTs.Trees.Core.Interfaces
{
    public interface IDependencyResolver
    {
        T Resolve<T>() where T : class;
        T Resolve<T>(string id) where T : class;
        void Register<TImplementation>(TImplementation implementation, string id = "");
        void Register<TInterface, TImplementation>(TImplementation implementation, string id = "")
            where TImplementation : TInterface;
    }
}
