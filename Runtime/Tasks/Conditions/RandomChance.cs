using UnityEngine;

namespace CleverCrow.Fluid.BTs.Tasks {
    public class RandomChance : ConditionBase {
        public float chance = 1;
        public float outOf = 1;
        public int seed;

        protected override bool OnUpdate () {
            var oldState = Random.state;

            if (seed != 0) {
                Random.InitState(seed);
            }
            
            var percentage = chance / outOf;
            var rng = Random.value;

            if (seed != 0) {
                Random.state = oldState;
            }
            
            return rng <= percentage;
        }
    }
}
