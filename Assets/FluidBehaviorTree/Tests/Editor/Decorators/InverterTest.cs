using CleverCrow.Fluid.BTs.Decorators;
using CleverCrow.Fluid.BTs.Tasks;
using NUnit.Framework;

namespace CleverCrow.Fluid.BTs.Testing {
    public class InverterTest {
        public class UpdateMethod {
            [Test]
            public void Returns_failure_when_child_returns_success () {
                var inverter = new Inverter();
                inverter.AddChild(A.TaskStub().Build());

                Assert.AreEqual(TaskStatus.Failure, inverter.Update());
            }

            [Test]
            public void Returns_true_when_child_returns_false () {
                var inverter = new Inverter();
                inverter.AddChild(A.TaskStub().WithUpdateStatus(TaskStatus.Failure).Build());

                Assert.AreEqual(TaskStatus.Success, inverter.Update());
            }

            [Test]
            public void Returns_continue_when_child_returns_continue () {
                var inverter = new Inverter();
                inverter.AddChild(A.TaskStub().WithUpdateStatus(TaskStatus.Continue).Build());

                Assert.AreEqual(TaskStatus.Continue, inverter.Update());
            }
        }
    }
}