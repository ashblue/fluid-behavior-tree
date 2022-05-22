using System;
using CleverCrow.Fluid.BTs.Trees.Core.Interfaces;

namespace CleverCrow.Fluid.BTs.Trees.Core.Implementations
{
    public class DebugHandler : IDebugHandler
    {
        private event Action EventActive;

        public void OnEventActive()
        {
            EventActive?.Invoke();
        }

        public void SubscribeToEventActive(Action callback)
        {
            EventActive += callback;
        }

        public void UnsubscribeFromEventActive(Action callback)
        {
            EventActive -= callback;
        }
    }
}
