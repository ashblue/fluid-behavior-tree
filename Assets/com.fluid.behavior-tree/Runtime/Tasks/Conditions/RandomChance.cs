using UnityEngine;

namespace CleverCrow.Fluid.BTs.Tasks
{
    public class RandomChance : ConditionBase
    {
        // TODO: Convert into properties
        public float Chance = 1;
        public float OutOf = 1;
        public int Seed;

        // TODO: use global seed
        protected override bool OnUpdate()
        {
            var oldState = Random.state;

            if (Seed != 0)
            {
                Random.InitState(Seed);
            }

            var percentage = Chance / OutOf;
            var rng = Random.value;

            if (Seed != 0)
            {
                Random.state = oldState;
            }

            return rng <= percentage;
        }
    }
}
