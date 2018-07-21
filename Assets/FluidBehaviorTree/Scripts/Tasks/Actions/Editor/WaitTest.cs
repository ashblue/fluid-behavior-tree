using NUnit.Framework;

namespace Adnc.FluidBT.Tasks.Actions.Testing {
    public class WaitTest {
        [Test]
        public void It_should_trigger_continue_on_first_tick () {
            var wait = new Wait();
            
            Assert.AreEqual(TaskStatus.Continue, wait.Update());
        }

        [Test]
        public void It_should_trigger_success_on_2nd_tick () {
            var wait = new Wait();

            Assert.AreEqual(TaskStatus.Continue, wait.Update());
            Assert.AreEqual(TaskStatus.Success, wait.Update());
        }
        
        [Test]
        public void It_should_trigger_success_after_2_ticks () {
            var wait = new Wait {
                turns = 2
            };
            
            Assert.AreEqual(TaskStatus.Continue, wait.Update());
            Assert.AreEqual(TaskStatus.Continue, wait.Update());
            Assert.AreEqual(TaskStatus.Success, wait.Update());
        }
    }
}
