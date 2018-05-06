using Adnc.FluidBT.Decorators;
using NSubstitute;
using NUnit.Framework;

namespace Adnc.FluidBT.Testing {
    public class InverterTest {
        public class EnabledProperty {
            [Test]
            public void Returns_true_if_child_is_set () {
                var inverter = new Inverter { child = A.TaskStub().Build() };

                Assert.IsTrue(inverter.Enabled);
            }

            [Test]
            public void Returns_false_if_child_is_not_set () {
                var inverter = new Inverter();

                Assert.IsFalse(inverter.Enabled);
            }

            [Test]
            public void Returns_false_if_child_is_disabled () {
                var inverter = new Inverter { child = A.TaskStub().Build() };
                inverter.child.Enabled.Returns(false);

                Assert.IsTrue(inverter.Enabled);
            }

            [Test]
            public void Returns_false_if_child_is_set_but_set_to_false () {
                var inverter = new Inverter { child = A.TaskStub().Build() };
                inverter.Enabled = false;

                Assert.IsFalse(inverter.Enabled);
            }
        }
    }
}