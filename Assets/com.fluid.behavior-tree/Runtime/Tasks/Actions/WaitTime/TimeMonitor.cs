#if UNITY_2021_3_OR_NEWER
using UnityEngine;
#endif

namespace CleverCrow.Fluid.BTs.Tasks.Actions
{
    public class TimeMonitor : ITimeMonitor
    {
        // TODO: Don't use Unity API
        public float DeltaTime => Time.deltaTime;
    }
}
