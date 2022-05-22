#if UNITY_2021_3_OR_NEWER
using UnityEngine;
#endif

namespace CleverCrow.Fluid.BTs.Tasks
{
    public class RandomChance : ConditionBase
    {
        public float Chance { get; set; } = 1;
        public float OutOf { get; set; } = 1;
        public int Seed { get; set; }

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
