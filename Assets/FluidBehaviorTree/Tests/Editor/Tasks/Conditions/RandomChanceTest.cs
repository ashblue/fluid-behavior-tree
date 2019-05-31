using CleverCrow.Fluid.BTs.Tasks;
using NUnit.Framework;

namespace CleverCrow.Fluid.BTs.Testing {
    public class RandomChanceTest {
        [Test]
        public void It_should_create_a_random_chance () {
            var randomChance = new RandomChance {
                chance = 1,
                outOf = 2
            };
            
            Assert.IsNotNull(randomChance.Update());
        }
    }
}