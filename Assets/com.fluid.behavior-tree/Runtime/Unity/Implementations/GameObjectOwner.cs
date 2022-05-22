using CleverCrow.Fluid.BTs.Trees.Core.Interfaces;
using UnityEngine;

namespace CleverCrow.Fluid.BTs.Trees.Runtime.Unity.Implementations
{
    public class GameObjectOwner : IOwner
    {
        public string Name
        {
            get => _gameObject.name;
            set => _gameObject.name = value;
        }

        private GameObject _gameObject;

        public GameObjectOwner(GameObject gameObject)
        {
            _gameObject = gameObject;
        }

        public GameObjectOwner(string name)
        {
            _gameObject = new GameObject(name);
        }

        public GameObjectOwner()
        {
            _gameObject = new GameObject();
        }

        public void Destroy() => Object.Destroy(_gameObject);

        public void DestroyImmediate() => Object.DestroyImmediate(_gameObject);
    }
}
