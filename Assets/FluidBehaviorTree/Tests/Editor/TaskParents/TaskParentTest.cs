using Adnc.FluidBT.TaskParents;
using Adnc.FluidBT.Tasks;
using Adnc.FluidBT.Tasks.Actions;
using NSubstitute;
using NUnit.Framework;

namespace Adnc.FluidBT.Testing {
    public class TaskParentTest {
        private TaskParentExample taskParent;

        [SetUp]
        public void SetTaskParent () {
            taskParent = new TaskParentExample();
        }

        public class TaskParentExample : TaskParentBase {
            public int childCount = -1;

            protected override int MaxChildren => childCount;
        }

        public class TaskExample : ActionBase {
            protected override TaskStatus OnUpdate () {
                return TaskStatus.Success;
            }
        }

        public class EnabledProperty : TaskParentTest {
            [Test]
            public void Returns_enabled_if_child () {
                taskParent.AddChild(A.TaskStub().Build());
                
                Assert.IsTrue(taskParent.Enabled);
            }
            
            [Test]
            public void Returns_disabled_if_child_and_set_to_disabled () {
                taskParent.AddChild(A.TaskStub().Build());
                taskParent.Enabled = false;
                
                Assert.IsFalse(taskParent.Enabled);
            }
        }

        public class AddChildMethod : TaskParentTest {
            [Test]
            public void Adds_a_child () {
                taskParent.AddChild(new TaskExample());
                
                Assert.AreEqual(1, taskParent.Children.Count);
            }
            
            [Test]
            public void Adds_two_children () {
                taskParent.AddChild(new TaskExample());
                taskParent.AddChild(new TaskExample());

                Assert.AreEqual(2, taskParent.Children.Count);
            }
            
            [Test]
            public void Ignores_overflowing_children () {
                taskParent.childCount = 1;
                
                taskParent.AddChild(new TaskExample());
                taskParent.AddChild(new TaskExample());

                Assert.AreEqual(1, taskParent.Children.Count);
            }
            
            [Test]
            public void Does_not_add_disabled_children () {
                var child = A.TaskStub()
                    .WithEnabled(false)
                    .Build();
                
                taskParent.AddChild(child);
                
                Assert.AreEqual(0, taskParent.Children.Count);
            }
        }
    }
}