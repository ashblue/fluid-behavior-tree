using Adnc.FluidBT.Decorators;
using Adnc.FluidBT.Tasks;
using NUnit.Framework;

namespace Adnc.FluidBT.Testing {
    public class InverterTest {
        public class UpdateMethod {
            [Test]
            public void Returns_failure_when_child_returns_success () {
                var inverter = new Inverter {
                    child = A.TaskStub().Build()
                };

                Assert.AreEqual(TaskStatus.Failure, inverter.Update());
            }

            [Test]
            public void Returns_true_when_child_returns_false () {
                var inverter = new Inverter {
                    child = A.TaskStub().WithUpdateStatus(TaskStatus.Failure).Build()
                };

                Assert.AreEqual(TaskStatus.Success, inverter.Update());
            }

            [Test]
            public void Returns_continue_when_child_returns_continue () {
                var inverter = new Inverter {
                    child = A.TaskStub().WithUpdateStatus(TaskStatus.Continue).Build()
                };

                Assert.AreEqual(TaskStatus.Continue, inverter.Update());
            }

            [Test]
            public void Does_not_crash_if_no_child () {
                var inverter = new Inverter();

                inverter.Update();
            }
        }
    }
}