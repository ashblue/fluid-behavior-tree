using UnityEngine;

namespace Adnc.FluidBT.Tasks {
    public class RandomChance : ConditionBase {
        private Random.State _state;
        
        public float chance = 1;
        public float outOf = 1;
        public int seed;

        protected override void OnInit () {                
            var oldState = Random.state;

            if (seed != 0) {
                Random.InitState(seed);
            }

            _state = Random.state;
            Random.state = oldState;
        }

        protected override bool OnUpdate () {
            var oldState = Random.state;
            Random.state = _state;
            
            var percentage = chance / outOf;
            var rng = Random.value;
            
            Random.state = oldState;
            
            return rng <= percentage;
        }
    }
}
