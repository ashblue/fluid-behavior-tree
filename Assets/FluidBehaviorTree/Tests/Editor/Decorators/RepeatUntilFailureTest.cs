using CleverCrow.Fluid.BTs.Decorators;
using CleverCrow.Fluid.BTs.Tasks;
using NUnit.Framework;

namespace CleverCrow.Fluid.BTs.Testing {
    public class RepeatUntilFailureTest {
        public class UpdateMethod {
            [Test]
            public void Returns_failure_on_child_failure () {
                var repeater = new RepeatUntilFailure();
                repeater.AddChild(A.TaskStub().WithUpdateStatus(TaskStatus.Failure).Build());

                Assert.AreEqual(TaskStatus.Failure, repeater.Update());
            }

            [Test]
            public void Returns_continue_on_child_success () {
                var repeater = new RepeatUntilFailure();
                repeater.AddChild(A.TaskStub().WithUpdateStatus(TaskStatus.Success).Build());

                Assert.AreEqual(TaskStatus.Continue, repeater.Update());
            }

            [Test]
            public void Returns_continue_on_child_continue () {
                var repeater = new RepeatUntilFailure();
                repeater.AddChild(A.TaskStub().WithUpdateStatus(TaskStatus.Continue).Build());

                Assert.AreEqual(TaskStatus.Continue, repeater.Update());
            }
        }
    }
}