using UnityEngine;

namespace Adnc.FluidBT.Examples {
    public class FlagManager : MonoBehaviour {
        public static FlagManager current;
        
        public GameObject flag;
        public GameObject goalRed;
        public GameObject goalBlue;

        [HideInInspector]
        public GameObject flagStart;

        private void Awake () {
            flagStart = flag;
            current = this;
        }
    }
}