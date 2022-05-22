using System;
using System.Collections.Generic;
using System.Data;
using CleverCrow.Fluid.BTs.Trees.Core.Interfaces;

namespace CleverCrow.Fluid.BTs.Trees.Core.Implementations
{
    /// <summary>
    /// Basic dependency resolver with bad performance. <br/>
    /// It's strongly advised to use your own implementation base on your DI framework.
    /// </summary>
    public class DummyDependencyResolver : IDependencyResolver
    {
        private readonly Dictionary<string, object> _dependencies = new();

        public void UnRegister<T>(string id = "")
        {
            var key = GetDependencyKey<T>(id);

            if (_dependencies.ContainsKey(key))
            {
                _dependencies.Remove(key);
            }
        }

        public void Register<TImplementation>(TImplementation implementation, string id = "")
        {
            var key = GetDependencyKey<TImplementation>(id);

            if (_dependencies.ContainsKey(key))
            {
                throw new DuplicateNameException($"Dependency with key '{key}' already exists");
            }

            _dependencies.Add(key, implementation);
        }

        public void Register<TInterface, TImplementation>(TImplementation implementation, string id = "")
            where TImplementation : TInterface
        {
            var key = GetDependencyKey<TInterface>(id);

            if (_dependencies.ContainsKey(key))
            {
                throw new DuplicateNameException($"Dependency with key '{key}' already exists");
            }

            _dependencies.Add(key, implementation);
        }

        public T Resolve<T>() where T : class => Resolve<T>("");

        public T Resolve<T>(string id) where T : class
        {
            var key = GetDependencyKey<T>(id);

            if (_dependencies.TryGetValue(key, out var value))
            {
                return value as T;
            }

            return null;
        }

        private string GetDependencyKey<TInterface>(string id = "") => $"{typeof(TInterface).FullName}__{id}";
    }
}
