using CleverCrow.Fluid.BTs.TaskParents.Composites;
using CleverCrow.Fluid.BTs.Tasks;
using NSubstitute;
using NUnit.Framework;

namespace CleverCrow.Fluid.BTs.Testing {
    public class TaskSequenceTest {
        private ITask _childA;
        private ITask _childB;
        private Sequence _sequence;

        [SetUp]
        public void SetupChild () {
            _sequence = new Sequence();

            _childA = CreateTaskStub();
            _sequence.AddChild(_childA);

            _childB = CreateTaskStub();
            _sequence.AddChild(_childB);
        }

        private ITask CreateTaskStub () {
            var task = A.TaskStub().Build();

            return task;
        }

        public class UpdateMethod : TaskSequenceTest {
            public class UpdateMethodMisc : UpdateMethod {
                [Test]
                public void It_should_run_update_on_all_success_child_tasks () {
                    _sequence.Update();

                    _sequence.Children.ForEach((c) => c.Received(1).Update());
                }

                [Test]
                public void It_should_retick_a_continue_node_when_update_is_rerun () {
                    _childB.Update().Returns(TaskStatus.Continue);

                    _sequence.Update();
                    _sequence.Update();

                    _childB.Received(2).Update();
                }

                [Test]
                public void It_should_not_update_previous_nodes_when_a_continue_node_is_rerun () {
                    _childB.Update().Returns(TaskStatus.Continue);

                    _sequence.Update();
                    _sequence.Update();

                    _childA.Received(1).Update();
                }
            }

            public class ReturnedStatusType : UpdateMethod {
                [Test]
                public void Returns_success_if_no_children () {
                    _sequence.Children.Clear();

                    Assert.AreEqual(TaskStatus.Success, _sequence.Update());
                }

                [Test]
                public void Should_be_success_if_all_child_tasks_pass () {
                    _sequence.Update();

                    Assert.AreEqual(TaskStatus.Success, _sequence.Update());
                }

                [Test]
                public void Should_be_failure_if_a_child_task_fails () {
                    _childA.Update().Returns(TaskStatus.Failure);

                    Assert.AreEqual(TaskStatus.Failure, _sequence.Update());
                }

                [Test]
                public void Should_be_continue_if_a_child_returns_continue () {
                    _childA.Update().Returns(TaskStatus.Continue);

                    Assert.AreEqual(TaskStatus.Continue, _sequence.Update());
                }
            }
        }
    }
}

