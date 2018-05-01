using Adnc.FluidBT.TaskParents;
using Adnc.FluidBT.Tasks;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;

namespace Adnc.FluidBT.Testing {
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

            // @TODO Break up these tests into better subsections
            public class WhenAbortLowerPriority : UpdateMethod {
                private Sequence _seqChild;
                private ITask _taskCondition;

                [SetUp]
                public void Setup_lower_priority_trigger () {
                    _seqChild = new Sequence { AbortType = AbortType.LowerPriority };

                    _taskCondition = A.TaskStub().WithAbortConditionSelf(true).Build();
                    _seqChild.AddChild(_taskCondition);
                    
                    _sequence.children.Clear();
                    _sequence.AddChild(_seqChild);
                    _sequence.AddChild(_childB);
                    _childB.Update().Returns(TaskStatus.Continue);
                }

                [Test]
                public void Does_not_trigger_abort_on_first_update () {
                    Assert.AreEqual(TaskStatus.Continue, _sequence.Update());
                }

                [Test]
                public void Triggers_when_a_lower_priority_sibling_composite_changes_from_success_to_failure () {
                    _sequence.Update();
                    _taskCondition.Update().Returns(TaskStatus.Failure);

                    Assert.AreEqual(TaskStatus.Failure, _sequence.Update());
                }

                [Test]
                public void Parent_task_triggers_reset_on_all_nodes () {
                    _sequence.Update();
                    _taskCondition.Update().Returns(TaskStatus.Failure);
                    _sequence.Update();

                    _taskCondition.Received(1).Reset();
                    _childB.Received(1).Reset();
                }

                [Test]
                public void Does_not_crash_on_a_conditional_abort_without_tasks () {
                    _seqChild.children.Clear();
                    
                    _sequence.Update();
                    _taskCondition.Update().Returns(TaskStatus.Failure);
                    _sequence.Update();
                }

                [Test]
                public void Does_not_recheck_conditional_aborts_after_reset_is_called () {
                    _sequence.Update();
                    _taskCondition.Update().Returns(TaskStatus.Failure);
                    _sequence.Update();
                    _sequence.Update();

                    _taskCondition.Received(3).Update();
                }
                
                [Test]
                public void Triggers_on_valid_conditional_task () {
                    _sequence.Update();
                    _taskCondition.Update().Returns(TaskStatus.Failure);
                    
                    Assert.AreEqual(TaskStatus.Failure, _sequence.Update());
                }

                [Test]
                public void Does_not_trigger_on_invalid_conditional_task () {
                    _taskCondition.GetAbortCondition().ReturnsNull();

                    _sequence.Update();
                    _taskCondition.Update().Returns(TaskStatus.Failure);
                    
                    Assert.AreEqual(TaskStatus.Continue, _sequence.Update());
                }
                
                [Test]
                public void Triggers_abort_on_nested_sequence_with_first_node_a_condition () {
                    _seqChild.children.Clear();
                    var nestedSeqChild = new Sequence();
                    nestedSeqChild.AddChild(_taskCondition);
                    _seqChild.AddChild(nestedSeqChild);
                    
                    _sequence.Update();
                    _taskCondition.Update().Returns(TaskStatus.Failure);

                    Assert.AreEqual(TaskStatus.Failure, _sequence.Update());
                }
                
                [Test]
                public void Does_not_trigger_on_nested_sequence_with_first_node_a_non_condition () {
                    _seqChild.children.Clear();
                    var nestedSeqChild = new Sequence();
                    nestedSeqChild.AddChild(_taskCondition);
                    _seqChild.AddChild(nestedSeqChild);
                    
                    _taskCondition.GetAbortCondition().ReturnsNull();

                    _sequence.Update();
                    _taskCondition.Update().Returns(TaskStatus.Failure);

                    Assert.AreEqual(TaskStatus.Continue, _sequence.Update());
                }

                [Test]
                public void Triggers_end_on_exited_pointer_task () {
                    _sequence.Update();
                    _taskCondition.Update().Returns(TaskStatus.Failure);
                    _sequence.Update();
                    
                    _childB.Received(1).End();
                }
                
                [Test]
                public void Triggers_end_on_nested_task () {
                    var sequence = new Sequence();
                    var child = CreateTaskStub();
                    sequence.AddChild(child);
                    child.Update().Returns(TaskStatus.Continue);

                    _childB.Update().Returns(TaskStatus.Success);
                    _sequence.AddChild(sequence);
                    
                    _sequence.Update();
                    _taskCondition.Update().Returns(TaskStatus.Failure);
                    _sequence.Update();
                    
                    child.Received(1).End();
                }

                [Test]
                public void Triggers_on_lower_priority_2nd_sibling () {
                    var task = CreateTaskStub();
                    _sequence.children.Clear();
                    _sequence.AddChild(task);
                    _sequence.AddChild(_seqChild);
                    _sequence.AddChild(_childB);
                    
                    _sequence.Update();
                    _taskCondition.Update().Returns(TaskStatus.Failure);

                    Assert.AreEqual(TaskStatus.Failure, _sequence.Update());
                }

                [Test]
                public void Triggers_on_lower_priority_3rd_sibling () {
                    var task = CreateTaskStub();
                    var taskNext = CreateTaskStub();
                    _sequence.children.Clear();
                    _sequence.AddChild(task);
                    _sequence.AddChild(taskNext);
                    _sequence.AddChild(_seqChild);
                    _sequence.AddChild(_childB);
                    
                    _sequence.Update();
                    _taskCondition.Update().Returns(TaskStatus.Failure);

                    Assert.AreEqual(TaskStatus.Failure, _sequence.Update());
                }
            }

            public class WhenAbortSelf : UpdateMethod {
                [SetUp]
                public void SetAbortTypeSelf () {
                    _sequence.AbortType = AbortType.Self;
                }

                public class DefaultCalls : WhenAbortSelf {
                    [Test]
                    public void Does_not_trigger_on_invalid_conditional_task () {
                        _childB.Update().Returns(TaskStatus.Continue);
                        
                        _sequence.Update();
                        
                        _childA.Update().Returns(TaskStatus.Failure);
                        
                        Assert.AreEqual(TaskStatus.Continue, _sequence.Update());
                    }

                    [Test]
                    public void It_should_not_abort_on_the_1st_update_call () {
                        _sequence.Update();

                        _childA.Received(1).Update();
                    }

                    [Test]
                    public void It_should_not_fail_if_no_children_are_present () {
                        _sequence.children.Clear();

                        _sequence.Update();
                    }

                    [Test]
                    public void Nested_sequence_will_return_failure_while_being_ticked_through_multiple_frames () {
                        var parentSequence = new Sequence {AbortType = AbortType.Self};
                        var childSequence = new Sequence();
                        var condition = A.TaskStub().WithAbortConditionSelf(true).Build();
                        var action = A.TaskStub().WithUpdateStatus(TaskStatus.Continue).Build();

                        parentSequence
                            .AddChild(childSequence.AddChild(condition))
                            .AddChild(action);
                        
                        Assert.AreEqual(TaskStatus.Continue, parentSequence.Update());
                        condition.Update().Returns(TaskStatus.Failure);

                        Assert.AreEqual(TaskStatus.Failure, parentSequence.Update());
                    }
                }

                public class WhenAbortIsReady : WhenAbortSelf {
                    private Sequence _parentSequence;
                    private ITask _condition;
                    private ITask _action;
                    
                    [SetUp]
                    public void SetupAbort () {
                        _parentSequence = new Sequence {AbortType = AbortType.Self};
                        _condition = A.TaskStub().WithAbortConditionSelf(true).Build();
                        _action = A.TaskStub().WithUpdateStatus(TaskStatus.Continue).Build();
                        
                        _parentSequence
                            .AddChild(_condition)
                            .AddChild(_action);
                        
                        _parentSequence.Update();
                        
                        _condition.Update().Returns(TaskStatus.Failure);
                    }

                    [Test]
                    public void Return_failure_on_tick_if_the_first_node_changes_from_success_to_failure () {
                        Assert.AreEqual(TaskStatus.Failure, _parentSequence.Update());
                    }

                    [Test]
                    public void Does_not_abort_if_not_marked_abort_self () {
                        _parentSequence.AbortType = AbortType.None;

                        Assert.AreEqual(TaskStatus.Continue, _parentSequence.Update());
                    }

                    [Test]
                    public void It_should_run_end_when_aborting_on_the_active_node () {
                        _parentSequence.Update();

                        _action.Received(1).End();
                    }

                    [Test]
                    public void Triggers_reset_after_firing_abort () {
                        _parentSequence.Update();

                        _parentSequence.children.ForEach((child) => {
                            child.Received().Reset();
                        });
                    }
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

            public class WhenAbortSelfAndLowerPriority {
                [Test]
                public void Triggers_self_abort_when_valid () {
                    var sequence = new Sequence();
                    sequence.AbortType |= AbortType.Self;
                    sequence.AbortType |= AbortType.LowerPriority;

                    var condition = A.TaskStub()
                        .WithAbortConditionSelf(true)
                        .Build();
                    sequence.AddChild(condition);

                    var action = A.TaskStub()
                        .WithUpdateStatus(TaskStatus.Continue)
                        .Build();
                    sequence.AddChild(action);

                    Assert.AreEqual(TaskStatus.Continue, sequence.Update());

                    condition.Update().Returns(TaskStatus.Failure);
                    
                    Assert.AreEqual(TaskStatus.Failure, sequence.Update());
                }
                
                [Test]
                public void Triggers_lower_priority_when_valid () {
                    var sequence = new Sequence();
                    sequence.AbortType |= AbortType.Self;
                    sequence.AbortType |= AbortType.LowerPriority;
                    
                    var sequenceSub = new Sequence();
                    sequenceSub.AbortType = AbortType.LowerPriority;
                    
                    var condition = A.TaskStub()
                        .WithAbortConditionSelf(true)
                        .Build();
                    sequenceSub.AddChild(condition);
                    sequence.AddChild(sequenceSub);

                    var action = A.TaskStub()
                        .WithUpdateStatus(TaskStatus.Continue)
                        .Build();
                    sequence.AddChild(action);

                    Assert.AreEqual(TaskStatus.Continue, sequence.Update());

                    condition.Update().Returns(TaskStatus.Failure);
                    
                    Assert.AreEqual(TaskStatus.Failure, sequence.Update());
                }
            }
        }
    }
}

