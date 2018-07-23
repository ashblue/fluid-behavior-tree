using UnityEngine;

namespace Adnc.FluidBT.Tasks {
    public class RandomChance : ConditionBase {
        public float chance = 1;
        public float outOf = 1;
        
        protected override bool OnUpdate () {
            var percentage = chance / outOf;
            var rng = Random.value;
            
            return rng <= percentage;
        }
    }
}
