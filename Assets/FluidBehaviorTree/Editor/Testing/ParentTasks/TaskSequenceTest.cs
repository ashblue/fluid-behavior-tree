using Adnc.FluidBT.TaskParents;
using Adnc.FluidBT.Tasks;
using NSubstitute;
using NUnit.Framework;

namespace Adnc.FluidBT.Testing {
    public class TaskSequenceTest {
        public class UpdateMethod {
            private ITask _childA;
            private TaskSequence _sequence;
            
            [SetUp]
            public void SetupChild () {
                _sequence = new TaskSequence();
                
                _childA = Substitute.For<ITask>();
                _childA.Update().Returns(TaskStatus.Success);
                _sequence.AddChild(_childA);
            }
            
            [Test]
            public void It_should_run_update_on_all_child_tasks () {
                var childB = Substitute.For<ITask>();
                _sequence.AddChild(childB);
                _sequence.Update();
                
                _sequence.children.ForEach((c) => c.Received().Update());
            }

            [Test]
            public void It_should_return_success_if_all_child_tasks_pass () {
                var childB = Substitute.For<ITask>();
                _sequence.AddChild(childB);

                _sequence.Update();
                
                Assert.AreEqual(TaskStatus.Success, _sequence.Update());
            }
            
            [Test]
            public void It_should_return_failure_if_a_child_task_fails () {
                _childA.Update().Returns(TaskStatus.Failure);

                Assert.AreEqual(TaskStatus.Failure, _sequence.Update());
            }

            [Test]
            public void It_should_return_continue_if_a_child_returns_continue () {
                _childA.Update().Returns(TaskStatus.Continue);

                Assert.AreEqual(TaskStatus.Continue, _sequence.Update());
            }
        }
    }
}

