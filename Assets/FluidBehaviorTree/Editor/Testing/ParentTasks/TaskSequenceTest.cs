using Adnc.FluidBT.TaskParents;
using Adnc.FluidBT.Tasks;
using NUnit.Framework;

namespace Adnc.FluidBT.Testing {
    public class TaskSequenceTest {
        public class TaskExample : TaskBase {
        }
        
        // @TODO Move adding child classes to a base class
        [Test]
        public void Allow_adding_tasks_to_the_children_array () {
            var seq = new TaskSequence();

            seq.children.Add(new TaskExample());
        }
        
        [Test]
        public void Allow_adding_parent_tasks_to_the_children_array () {
            var seq = new TaskSequence();

            seq.children.Add(new TaskSequence());
        }
    }
}

