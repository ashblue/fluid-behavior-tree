using Adnc.FluidBT.TaskParents;
using Adnc.FluidBT.Tasks;
using NSubstitute;
using System.Collections.Generic;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;

namespace Adnc.FluidBT.Testing {
    public class SelectorTest {
        public class UpdateMethod {
            private Selector _selector;

            [SetUp]
            public void SetSelector () {
                _selector = new Selector();
            }

            public void CheckUpdateCalls (ITaskParent selector, List<int> updateCalls) {
                for (var i = 0; i < updateCalls.Count; i++) {
                    var child = selector.children[i];
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

            public class ConditionalAbortSelf : UpdateMethod {
                public ITask childFailure;
                public ITask childContinue;
                
                [SetUp]
                public void CreateChildren () {
                    _selector.AbortType = AbortType.Self;

                    childFailure = A.TaskStub()
                        .WithAbortConditionSelf(true)
                        .WithUpdateStatus(TaskStatus.Failure)
                        .Build();
                    
                    childContinue = A.TaskStub()
                        .WithUpdateStatus(TaskStatus.Continue)
                        .Build();
                }
                
                [Test]
                public void Revaultes_first_condition_node_when_it_goes_from_failure_to_success () {
                    _selector.AddChild(childFailure);
                    _selector.AddChild(childContinue);

                    _selector.Update();
                    childFailure.Update().Returns(TaskStatus.Success);
                    _selector.Update();

                    var updateCalls = new List<int> {3, 1};
                    CheckUpdateCalls(_selector, updateCalls);
                }

                [Test]
                public void Does_not_revaluate_if_no_abort_type () {
                    _selector.AbortType = AbortType.None;
                    
                    _selector.AddChild(childFailure);
                    _selector.AddChild(childContinue);

                    _selector.Update();
                    childFailure.Update().Returns(TaskStatus.Success);
                    _selector.Update();

                    var updateCalls = new List<int> {1, 2};
                    CheckUpdateCalls(_selector, updateCalls);
                }

                [Test]
                public void Does_not_crash_if_self_abort_is_null () {
                    childFailure.GetAbortCondition().ReturnsNull();
                    
                    _selector.AddChild(childFailure);
                    _selector.AddChild(childContinue);

                    _selector.Update();
                    childFailure.Update().Returns(TaskStatus.Success);
                    _selector.Update();

                    var updateCalls = new List<int> {1, 2};
                    CheckUpdateCalls(_selector, updateCalls);
                }
                
                [Test]
                public void Returns_true_when_revaluating_abort_condition () {
                    _selector.AddChild(childFailure);
                    _selector.AddChild(childContinue);

                    _selector.Update();
                    childFailure.Update().Returns(TaskStatus.Success);

                    Assert.AreEqual(TaskStatus.Success, _selector.Update());
                }
                
                [Test]
                public void Triggers_reset_on_abort () {
                    _selector.AddChild(childFailure);
                    _selector.AddChild(childContinue);

                    _selector.Update();
                    childFailure.Update().Returns(TaskStatus.Success);
                    _selector.Update();
                    
                    childFailure.Received(1).Reset();
                    childContinue.Received(1).Reset();
                }

                [Test]
                public void Triggers_end_on_current_task_pointer () {
                    _selector.AddChild(childFailure);
                    _selector.AddChild(childContinue);

                    _selector.Update();
                    childFailure.Update().Returns(TaskStatus.Success);
                    _selector.Update();
                    
                    childContinue.Received(1).End();
                }

                [Test]
                public void Returns_continue_if_nested_sequence_condition_is_followed_by_failure_task () {
                    var nestedSequence = new Sequence()
                        .AddChild(childFailure)
                        .AddChild(A.TaskStub().WithUpdateStatus(TaskStatus.Failure).Build());
                    
                    _selector.AddChild(nestedSequence);
                    _selector.AddChild(childContinue);
                    
                    _selector.Update();
                    childFailure.Update().Returns(TaskStatus.Success);
                    
                    Assert.AreEqual(TaskStatus.Continue, _selector.Update());
                }
            }

            public class ConditionalAbortLowerPrioritySelector {
                public class AbortTypeSelector : UpdateMethod {
                    [Test]
                    public void Revaluates_from_where_a_conditional_abort_goes_from_failure_to_success () {
                        var sequenceSub = new Sequence();
                        sequenceSub.AbortType = AbortType.LowerPriority;
                        
                        var childFailure = A.TaskStub()
                            .WithAbortConditionSelf(true)
                            .WithUpdateStatus(TaskStatus.Failure)
                            .Build();
                        sequenceSub.AddChild(childFailure);
                        sequenceSub.AddChild(A.TaskStub().Build());

                        _selector
                            .AddChild(sequenceSub)
                            .AddChild(A.TaskStub().Build());

                        _selector.Update();
                        childFailure.Update().Returns(TaskStatus.Success);
                        _selector.Update();
                        
                        CheckUpdateCalls(sequenceSub, new List<int>{3, 1});
                        CheckUpdateCalls(_selector, new List<int>{-1, 1});
                    }
                    
                    [Test]
                    public void Returns_success_when_a_conditional_abort_goes_from_failure_to_success () {
                        var sequenceSub = new Sequence();
                        sequenceSub.AbortType = AbortType.LowerPriority;
                        
                        var childFailure = A.TaskStub()
                            .WithAbortConditionSelf(true)
                            .WithUpdateStatus(TaskStatus.Failure)
                            .Build();
                        sequenceSub.AddChild(childFailure);
                        sequenceSub.AddChild(A.TaskStub().Build());

                        _selector
                            .AddChild(sequenceSub)
                            .AddChild(A.TaskStub().Build());

                        _selector.Update();
                        childFailure.Update().Returns(TaskStatus.Success);
                        
                        Assert.AreEqual(TaskStatus.Success, _selector.Update());
                    }
                    
                    // @TODO Verify nested conditional abort runs from correct starting point (currently tests all children)
                }

                public class AbortTypeComposite {
                    // @TODO Add composite abort type tests
                }
            }
        }
    }
}