using NSubstitute;
using System.Collections.Generic;
using CleverCrow.Fluid.BTs.TaskParents;
using CleverCrow.Fluid.BTs.TaskParents.Composites;
using CleverCrow.Fluid.BTs.Tasks;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;

namespace CleverCrow.Fluid.BTs.Testing {
    public class SelectorTest {
        public class UpdateMethod {
            private Selector _selector;

            [SetUp]
            public void SetSelector () {
                _selector = new Selector();
            }

            public void CheckUpdateCalls (ITaskParent selector, List<int> updateCalls) {
                for (var i = 0; i < updateCalls.Count; i++) {
                    var child = selector.Children[i];
                    if (updateCalls[i] >= 0) {
                        child.Received(updateCalls[i]).Update();
                    }
                }
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

            public class MultipleNodes : UpdateMethod {
                [Test]
                public void Stops_on_continue () {
                    var childFailure = A.TaskStub().WithUpdateStatus(TaskStatus.Failure).Build();
                    _selector.AddChild(childFailure);

                    var childContinue = A.TaskStub().WithUpdateStatus(TaskStatus.Continue).Build();
                    _selector.AddChild(childContinue);
                    
                    var childSuccess = A.TaskStub().Build();
                    _selector.AddChild(childSuccess);

                    _selector.Update();

                    var updateCalls = new List<int> {1, 1, 0};
                    CheckUpdateCalls(_selector, updateCalls);
                }
                
                [Test]
                public void Reruns_the_same_node_if_it_returns_continue () {
                    var childFailure = A.TaskStub().WithUpdateStatus(TaskStatus.Failure).Build();
                    _selector.AddChild(childFailure);

                    var childContinue = A.TaskStub().WithUpdateStatus(TaskStatus.Continue).Build();
                    _selector.AddChild(childContinue);

                    _selector.Update();
                    _selector.Update();

                    var updateCalls = new List<int> {1, 2};
                    CheckUpdateCalls(_selector, updateCalls);
                }

                [Test]
                public void Returns_failure_if_all_return_failure () {
                    var childA = A.TaskStub().WithUpdateStatus(TaskStatus.Failure).Build();
                    _selector.AddChild(childA);
                    
                    var childB = A.TaskStub().WithUpdateStatus(TaskStatus.Failure).Build();
                    _selector.AddChild(childB);

                    Assert.AreEqual(TaskStatus.Failure, _selector.Update());
                }

                [Test]
                public void Stops_on_first_success_node () {
                    var childFailure = A.TaskStub().WithUpdateStatus(TaskStatus.Failure).Build();
                    _selector.AddChild(childFailure);

                    var childSuccessA = A.TaskStub().Build();
                    _selector.AddChild(childSuccessA);
                    
                    var childSuccessB = A.TaskStub().Build();
                    _selector.AddChild(childSuccessB);

                    _selector.Update();

                    var updateCalls = new List<int> {1, 1, 0};
                    CheckUpdateCalls(_selector, updateCalls);
                }
            }
        }
    }
}