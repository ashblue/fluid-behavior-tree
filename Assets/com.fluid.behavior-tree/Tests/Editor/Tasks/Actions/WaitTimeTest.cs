using NSubstitute;
using NUnit.Framework;

namespace CleverCrow.Fluid.BTs.Tasks.Actions.Editors.Tests {
    public class WaitTimeTest {
        public class UpdateMethod {
            [Test]
            public void It_should_return_continue_if_time_has_not_passed () {
                var timeMonitor = Substitute.For<ITimeMonitor>();
                timeMonitor.DeltaTime.Returns(0);
                var waitTime = new WaitTime(timeMonitor) {time = 1};

                Assert.AreEqual(TaskStatus.Continue, waitTime.Update());
            }

            [Test]
            public void It_should_return_success_if_time_has_passed () {
                var timeMonitor = Substitute.For<ITimeMonitor>();
                timeMonitor.DeltaTime.Returns(2);
                var waitTime = new WaitTime(timeMonitor) {time = 1};
            
                Assert.AreEqual(TaskStatus.Success, waitTime.Update());
            }
        }

        public class ResetMethod {
            [Test]
            public void It_should_reset_time () {
                var timeMonitor = Substitute.For<ITimeMonitor>();
                var waitTime = new WaitTime(timeMonitor) {time = 1};

                timeMonitor.DeltaTime.Returns(2);
                waitTime.Update();
                waitTime.ResetTask();
                timeMonitor.DeltaTime.Returns(0);
                
                Assert.AreEqual(TaskStatus.Continue, waitTime.Update());

            }
        }
    }
}
