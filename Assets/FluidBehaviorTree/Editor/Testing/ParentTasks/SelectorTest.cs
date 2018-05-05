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
                public class AbortTypeSequence : UpdateMethod {
                    private Sequence sequenceSub;
                    private ITask childFailure;
                    private ITask endTask;

                    [SetUp]
                    public void SetDefaults () {
                        sequenceSub = new Sequence();
                        sequenceSub.AbortType = AbortType.LowerPriority;

                        childFailure = A.TaskStub()
                            .WithAbortConditionSelf(true)
                            .WithUpdateStatus(TaskStatus.Failure)
                            .Build();
                        sequenceSub.AddChild(childFailure);
                        sequenceSub.AddChild(A.TaskStub().Build());

                        endTask = A.TaskStub().WithUpdateStatus(TaskStatus.Continue).Build();
                    }

                    [Test]
                    public void Revaluates_from_where_a_conditional_abort_goes_from_failure_to_success () {
                        _selector
                            .AddChild(sequenceSub)
                            .AddChild(endTask);

                        _selector.Update();
                        childFailure.Update().Returns(TaskStatus.Success);
                        _selector.Update();

                        CheckUpdateCalls(sequenceSub, new List<int>{3, 1});
                        CheckUpdateCalls(_selector, new List<int>{-1, 1});
                    }
                    
                    [Test]
                    public void Returns_success_when_a_conditional_abort_goes_from_failure_to_success () {
                        _selector
                            .AddChild(sequenceSub)
                            .AddChild(endTask);

                        Assert.AreEqual(TaskStatus.Continue, _selector.Update());
                        childFailure.Update().Returns(TaskStatus.Success);
                        Assert.AreEqual(TaskStatus.Success, _selector.Update());
                    }

                    [Test]
                    public void Begins_revaluation_from_origin_of_the_conditional_abort () {
                        _selector
                            .AddChild(A.TaskStub().WithUpdateStatus(TaskStatus.Failure).Build())
                            .AddChild(sequenceSub)
                            .AddChild(endTask);

                        _selector.Update();
                        childFailure.Update().Returns(TaskStatus.Success);
                        _selector.Update();

                        CheckUpdateCalls(sequenceSub, new List<int>{3, 1});
                        CheckUpdateCalls(_selector, new List<int>{1, -1, 1});
                    }

                    [Test]
                    public void Keeps_conditional_aborts_before_the_revaluated_abort () {
                        var failSequence = new Sequence();
                        failSequence.AbortType = AbortType.LowerPriority;
                        failSequence.AddChild(A.TaskStub()
                            .WithAbortConditionSelf(true)
                            .WithUpdateStatus(TaskStatus.Failure)
                            .Build());

                        _selector
                            .AddChild(failSequence)
                            .AddChild(sequenceSub)
                            .AddChild(endTask);

                        _selector.Update();
                        childFailure.Update().Returns(TaskStatus.Success);
                        _selector.Update();

                        Assert.AreEqual(1, _selector.AbortLowerPriorities.Count);
                        Assert.IsTrue(_selector.AbortLowerPriorities.Contains(failSequence.children[0]));
                    }
                }

                public class AbortTypeSelector : UpdateMethod {
                    public class WhenAbortingFromFailureToSuccess : UpdateMethod {
                        private Selector selectorSub;
                        private ITask conditionAbort;

                        [SetUp]
                        public void Build_tree () {
                            selectorSub = new Selector();
                            selectorSub.AbortType = AbortType.LowerPriority;

                            conditionAbort = A.TaskStub()
                                .WithAbortConditionSelf(true)
                                .WithUpdateStatus(TaskStatus.Failure)
                                .Build();

                            var actionContinue = A.TaskStub()
                                .WithUpdateStatus(TaskStatus.Continue)
                                .Build();

                            _selector
                                .AddChild(selectorSub
                                    .AddChild(conditionAbort))
                                .AddChild(actionContinue);
                        }

                        [Test]
                        public void First_call_should_return_continue () {
                            Assert.AreEqual(TaskStatus.Continue, _selector.Update());
                        }

                        [Test]
                        public void Revaluates_when_going_from_failure_to_success () {
                            _selector.Update();
                            conditionAbort.Update().Returns(TaskStatus.Success);
                            _selector.Update();

                            CheckUpdateCalls(_selector, new List<int> { -1, 1 });
                            CheckUpdateCalls(selectorSub, new List<int> { 3 });
                        }

                        [Test]
                        public void Returns_success_when_going_from_failure_to_success () {
                            _selector.Update();
                            conditionAbort.Update().Returns(TaskStatus.Success);
                            Assert.AreEqual(TaskStatus.Success, _selector.Update());
                        }
                    }

                    public class WhenAbortingWithMultipleSelectorConditions : UpdateMethod {
                        private Selector selectorSub;
                        private ITask conditionAbortA;
                        private ITask conditionAbortB;

                        [SetUp]
                        public void Build_tree () {
                            selectorSub = new Selector();
                            selectorSub.AbortType = AbortType.LowerPriority;

                            conditionAbortA = A.TaskStub()
                                .WithAbortConditionSelf(true)
                                .WithUpdateStatus(TaskStatus.Failure)
                                .Build();

                            conditionAbortB = A.TaskStub()
                                .WithAbortConditionSelf(true)
                                .WithUpdateStatus(TaskStatus.Failure)
                                .Build();

                            var actionContinue = A.TaskStub()
                                .WithUpdateStatus(TaskStatus.Continue)
                                .Build();

                            _selector
                                .AddChild(selectorSub
                                    .AddChild(conditionAbortA)
                                    .AddChild(conditionAbortB))
                                .AddChild(actionContinue);
                        }

                        [Test]
                        public void First_call_should_return_continue () {
                            Assert.AreEqual(TaskStatus.Continue, _selector.Update());
                        }

                        [Test]
                        public void Revaluates_when_going_from_failure_to_success () {
                            _selector.Update();
                            conditionAbortA.Update().Returns(TaskStatus.Success);
                            _selector.Update();

                            CheckUpdateCalls(_selector, new List<int> { -1, 1 });
                            CheckUpdateCalls(selectorSub, new List<int> { 3, 1 });
                        }

                        [Test]
                        public void Revaluates_both_conditions_when_going_from_failure_to_success () {
                            _selector.Update();
                            conditionAbortB.Update().Returns(TaskStatus.Success);
                            _selector.Update();

                            // @NOTE Must re-run the entire selector since it only detects that something changed in it (could be more optimized)
                            CheckUpdateCalls(_selector, new List<int> { -1, 1 });
                            CheckUpdateCalls(selectorSub, new List<int> { 3, 3 });
                        }

                        [Test]
                        public void Returns_success_when_going_from_failure_to_success () {
                            _selector.Update();
                            conditionAbortA.Update().Returns(TaskStatus.Success);

                            Assert.AreEqual(TaskStatus.Success, _selector.Update());
                        }

                        [Test]
                        public void Returns_success_when_last_abort_goes_from_failure_to_success () {
                            _selector.Update();
                            conditionAbortB.Update().Returns(TaskStatus.Success);

                            Assert.AreEqual(TaskStatus.Success, _selector.Update());
                        }
                    }

                    public class WhenAbortsHaveSiblings : UpdateMethod {
                        private Selector selectorSub;
                        private Sequence selectorSequenceSub;
                        private ITask conditionAbortA;
                        private ITask conditionAbortASibling;
                        private ITask conditionAbortB;

                        [SetUp]
                        public void Build_tree () {
                            selectorSub = new Selector();
                            selectorSub.AbortType = AbortType.LowerPriority;

                            selectorSequenceSub = new Sequence();

                            conditionAbortA = A.TaskStub()
                                .WithAbortConditionSelf(true)
                                .WithUpdateStatus(TaskStatus.Failure)
                                .Build();

                            conditionAbortB = A.TaskStub()
                                .WithAbortConditionSelf(true)
                                .WithUpdateStatus(TaskStatus.Failure)
                                .Build();

                            conditionAbortASibling = A.TaskStub().WithUpdateStatus(TaskStatus.Failure).Build();

                            var actionContinue = A.TaskStub()
                                .WithUpdateStatus(TaskStatus.Continue)
                                .Build();

                            _selector
                                .AddChild(selectorSub
                                    .AddChild(selectorSequenceSub
                                        .AddChild(conditionAbortA)
                                        .AddChild(conditionAbortASibling))
                                    .AddChild(conditionAbortB))
                                .AddChild(actionContinue);
                        }

                        [Test]
                        public void First_call_should_return_continue () {
                            Assert.AreEqual(TaskStatus.Continue, _selector.Update());
                        }

                        [Test]
                        public void Returns_continue_when_conditional_is_activated_but_sibling_action_returns_failure () {
                            _selector.Update();
                            conditionAbortA.Update().Returns(TaskStatus.Success);
                            Assert.AreEqual(TaskStatus.Continue, _selector.Update());
                        }

                        [Test]
                        public void Revaluates_when_conditional_is_activated_but_sibling_action_returns_failure () {
                            _selector.Update();
                            conditionAbortA.Update().Returns(TaskStatus.Success);
                            _selector.Update();

                            CheckUpdateCalls(_selector, new List<int> { -1, 2 });
                            CheckUpdateCalls(selectorSub, new List<int> { -1, 2 });
                            CheckUpdateCalls(selectorSequenceSub, new List<int> { 3, 1 });
                        }

                        [Test]
                        public void Returns_success_when_conditional_is_activated_but_sibling_action_returns_success () {
                            conditionAbortASibling.Update().Returns(TaskStatus.Success);

                            _selector.Update();
                            conditionAbortA.Update().Returns(TaskStatus.Success);
                            Assert.AreEqual(TaskStatus.Success, _selector.Update());
                        }

                        [Test]
                        public void Revaluates_when_conditional_is_activated_but_sibling_action_returns_success () {
                            conditionAbortASibling.Update().Returns(TaskStatus.Success);

                            _selector.Update();
                            conditionAbortA.Update().Returns(TaskStatus.Success);
                            _selector.Update();

                            CheckUpdateCalls(_selector, new List<int> { -1, 1 });
                            CheckUpdateCalls(selectorSub, new List<int> { -1, 1 });
                            CheckUpdateCalls(selectorSequenceSub, new List<int> { 3, 1 });
                        }
                    }

                    public class WhenAbortIsNotThe1stNode : UpdateMethod {
                        private Selector selectorSub;
                        private ITask conditionAbort;

                        [SetUp]
                        public void Build_tree () {
                            selectorSub = new Selector();
                            selectorSub.AbortType = AbortType.LowerPriority;

                            conditionAbort = A.TaskStub()
                                .WithAbortConditionSelf(true)
                                .WithUpdateStatus(TaskStatus.Failure)
                                .Build();

                            var actionContinue = A.TaskStub()
                                .WithUpdateStatus(TaskStatus.Continue)
                                .Build();

                            _selector
                                .AddChild(A.TaskStub().WithUpdateStatus(TaskStatus.Failure).Build())
                                .AddChild(selectorSub
                                    .AddChild(conditionAbort))
                                .AddChild(actionContinue);
                        }

                        [Test]
                        public void First_call_should_return_continue () {
                            Assert.AreEqual(TaskStatus.Continue, _selector.Update());
                        }

                        [Test]
                        public void Revaluates_when_going_from_failure_to_success () {
                            _selector.Update();
                            conditionAbort.Update().Returns(TaskStatus.Success);
                            _selector.Update();

                            CheckUpdateCalls(_selector, new List<int> { 1, -1, 1 });
                            CheckUpdateCalls(selectorSub, new List<int> { 3 });
                        }

                        [Test]
                        public void Returns_success_when_going_from_failure_to_success () {
                            _selector.Update();
                            conditionAbort.Update().Returns(TaskStatus.Success);
                            Assert.AreEqual(TaskStatus.Success, _selector.Update());
                        }
                    }
                }
            }
        }
    }
}