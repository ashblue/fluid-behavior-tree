using UnityEngine.Events;

namespace CleverCrow.Fluid.BTs.Tasks {
    /// <summary>
    /// A bundle of editor utilities that are for data visualization only.
    /// Unsafe to call outside of #if UNITY_EDITOR methods and non-performant for normal runtime
    /// </summary>
    public class EditorRuntimeUtilities {
        public readonly UnityEvent EventActive = new UnityEvent();
    }
}