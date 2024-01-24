using UnityEngine;

namespace CleverCrow.Fluid.BTs.Tasks.Actions {
    public class TimeMonitor : ITimeMonitor {
        public float DeltaTime => Time.deltaTime;
    }
}
