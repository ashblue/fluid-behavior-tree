using Adnc.FluidBT.TaskParents;
using Adnc.FluidBT.Tasks;
using NSubstitute;
using NUnit.Framework;

namespace Adnc.FluidBT.Testing {
    public class SelectorTest {
        public class UpdateMethod {
            private Selector _selector;

            [SetUp]
            public void SetSelector () {
                _selector = new Selector();
            }

            public class SingleNode : UpdateMethod {
                [Test]
                public void Returns_success_if_a_child_task_returns_success () {
                    _selector.AddChild(A.TaskStub().Build());

                    Assert.AreEqual(TaskStatus.Success, _selector.Update());
                }

                [Test]
                public void Returns_failure_if_a_child_task_returns_failure () {
                    var child = A.TaskStub().WithUpdateStatus(TaskStatus.Failure).Build();
                    _selector.AddChild(child);

                    Assert.AreEqual(TaskStatus.Failure, _selector.Update());
                }

                [Test]
                public void Returns_continue_if_a_child_task_returns_continue () {
                    var child = A.TaskStub().WithUpdateStatus(TaskStatus.Continue).Build();
                    _selector.AddChild(child);

                    Assert.AreEqual(TaskStatus.Continue, _selector.Update());
                }
            }

            public class TwoNodes : UpdateMethod {
                // @TODO Break off into a stops on continue test
                [Test]
                public void Reruns_the_same_node_if_it_returns_continue () {
                    var selector = new Selector();

                    var childSuccess = A.TaskStub().WithUpdateStatus(TaskStatus.Failure).Build();
                    selector.AddChild(childSuccess);

                    var childContinue = A.TaskStub().WithUpdateStatus(TaskStatus.Continue).Build();
                    selector.AddChild(childContinue);

                    selector.Update();

                    // @TODO Check child index instead
                    childSuccess.Received(1).Update();
                    childContinue.Received(1).Update();

                    selector.Update();

                    childSuccess.Received(1).Update();
                    childContinue.Received(2).Update();
                }

                public void Stops_on_continue () {

                }

                public void Returns_failure_if_all_return_failure () {

                }

                public void Only_runs_first_node_if_success () {

                }

                public void Runs_all_nodes_on_success () {
                }
            }
        }

        public class EndMethod {
            public void Calls_end_on_current_child_node () {

            }
        }

        public class ResetMethod {
            public void Sets_child_index_to_zero () {

            }
        }
    }
}