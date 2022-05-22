using System;

namespace CleverCrow.Fluid.BTs.Trees.Core.Interfaces
{
    public interface IDebugHandler
    {
        void OnEventActive();
        void SubscribeToEventActive(Action callback);
        void UnsubscribeFromEventActive(Action callback);
    }
}
