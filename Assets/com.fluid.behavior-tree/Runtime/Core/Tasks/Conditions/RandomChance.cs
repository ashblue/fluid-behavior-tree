using CleverCrow.Fluid.BTs.com.fluid.behavior_tree.Runtime.Core;
using CleverCrow.Fluid.BTs.Trees.Core.Interfaces;

namespace CleverCrow.Fluid.BTs.Tasks
{
    public class RandomChance : ConditionBase
    {
        public float Chance { get; set; } = 1;
        public float OutOf { get; set; } = 1;

        private readonly IRandomHandler _randomHandler = BehaviorTreeConfiguration.DependencyResolver.Resolve<IRandomHandler>();

        protected override bool OnUpdate()
        {
            var percentage = Chance / OutOf;
            var rng = _randomHandler.NextFloat(0, 1f);

            return rng <= percentage;
        }
    }
}
