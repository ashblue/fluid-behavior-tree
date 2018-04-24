using Adnc.FluidBT.TaskParents;
using Adnc.FluidBT.Tasks;
using NUnit.Framework;

namespace Adnc.FluidBT.Testing {
    public class TaskParentTest {
        public class TaskParentExample : TaskParentBase {
            public int childCount = -1;

            protected override int MaxChildren {
                get { return childCount; }
            }
        }

        public class TaskExample : TaskBase {
        }
        
        public class AddChildMethod {
            [Test]
            public void Adds_a_child () {
                var taskParent = new TaskParentExample();

                taskParent.AddChild(new TaskExample());
                
                Assert.AreEqual(1, taskParent.children.Count);
            }
            
            [Test]
            public void Adds_two_children () {
                var taskParent = new TaskParentExample();
           
                taskParent.AddChild(new TaskExample());
                taskParent.AddChild(new TaskExample());

                Assert.AreEqual(2, taskParent.children.Count);
            }
            
            [Test]
            public void Ignores_overflowing_children () {
                var taskParent = new TaskParentExample();
                taskParent.childCount = 1;
                
                taskParent.AddChild(new TaskExample());
                taskParent.AddChild(new TaskExample());

                Assert.AreEqual(1, taskParent.children.Count);
            }
        }
    }
}