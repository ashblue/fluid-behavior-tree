using Adnc.FluidBT.TaskParents;
using Adnc.FluidBT.Tasks;
using NSubstitute;
using NUnit.Framework;

namespace Adnc.FluidBT.Testing {
    public class TaskSequenceTest {
        private ITask _childA;
        private ITask _childB;
        private TaskSequence _sequence;

        [SetUp]
        public void SetupChild () {
            _sequence = new TaskSequence();

            _childA = Substitute.For<ITask>();
            _childA.Enabled.Returns(true);
            _childA.Update().Returns(TaskStatus.Success);
            _sequence.AddChild(_childA);

            _childB = Substitute.For<ITask>();
            _childB.Enabled.Returns(true);
            _childB.Update().Returns(TaskStatus.Success);
            _sequence.AddChild(_childB);
        }

        public class UpdateMethod : TaskSequenceTest {
            public class UpdateMethodMisc : UpdateMethod {
                [Test]
                public void Skips_nodes_marked_disabled () {
                    _childA.Enabled.Returns(false);

                    _sequence.Update();

                    _sequence.children.ForEach((child) => {
                        if (child.Enabled) {
                            child.Received(1).Update();
                        } else {
                            child.Received(0).Update();
                        }
                    });
                }

                [Test]
                public void It_should_run_update_on_all_success_child_tasks () {
                    _sequence.Update();

                    _sequence.children.ForEach((c) => c.Received(1).Update());
                }

                [Test]
                public void It_should_not_run_update_on_any_child_tasks_after_success () {
                    _sequence.Update();
                    _sequence.Update();

                    _sequence.children.ForEach((c) => c.Received(1).Update());
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
                    _sequence.children.Clear();

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

        public class ResetMethod : TaskSequenceTest {
            [Test]
            public void Resets_ticking_of_child_nodes () {
                _childB.Update().Returns(TaskStatus.Continue);

                _sequence.Update();
                _sequence.Reset();
                _sequence.Update();

                _sequence.children.ForEach((c) => c.Received(2).Update());
            }
        }
    }
}

