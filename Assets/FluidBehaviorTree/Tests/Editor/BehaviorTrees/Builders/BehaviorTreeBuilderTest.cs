using CleverCrow.Fluid.BTs.Decorators;
using CleverCrow.Fluid.BTs.TaskParents.Composites;
using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Tasks.Actions;
using NUnit.Framework;

namespace CleverCrow.Fluid.BTs.Trees.Testing {
    public class BehaviorTreeBuilderTest {
        private int _invokeCount;
        private BehaviorTreeBuilder _builder;

        [SetUp]
        public void BeforeEach () {
            _invokeCount = 0;
            _builder = new BehaviorTreeBuilder(null);
        }

        public class SelectorRandomMethod : BehaviorTreeBuilderTest {
            [Test]
            public void It_should_add_a_random_selector () {
                var tree = _builder
                    .SelectorRandom("random selector")
                    .Build();

                var selectorRandom = tree.Root.Children[0] as SelectorRandom;

                Assert.IsNotNull(selectorRandom);
            }
        }

        public class SequenceMethod : BehaviorTreeBuilderTest {
            [Test]
            public void Create_a_sequence () {
                var tree = _builder
                    .Sequence()
                        .Do("action", () => {
                            _invokeCount++;
                            return TaskStatus.Success;
                        })
                    .Build();

                var sequence = tree.Root.Children[0] as Sequence;

                Assert.AreEqual(tree.Root.Children.Count, 1);
                Assert.AreEqual(sequence.Children.Count, 1);
                Assert.AreEqual(TaskStatus.Success, tree.Tick());
                Assert.AreEqual(1, _invokeCount);
            }

            [Test]
            public void Create_a_nested_sequence () {
                var tree = _builder
                    .Sequence("sequence")
                        .Do("action", () => {
                            _invokeCount++;
                            return TaskStatus.Success;
                        })
                        .Sequence("nested")
                            .Do("actionNested", () => {
                                _invokeCount++;
                                return TaskStatus.Success;
                            })
                    .Build();

                var sequence = tree.Root.Children[0] as Sequence;
                Assert.AreEqual(2, sequence.Children.Count);

                var nested = sequence.Children[1] as Sequence;
                Assert.AreEqual(nested.Children.Count, 1);
                Assert.AreEqual(TaskStatus.Success, tree.Tick());
                Assert.AreEqual(2, _invokeCount);
            }

            [Test]
            public void Create_a_nested_sequence_then_add_an_action_to_the_parent () {
                var tree = _builder
                    .Sequence("sequence")
                        .Do("action", () => {
                            _invokeCount++;
                            return TaskStatus.Success;
                        })
                        .Sequence("nested")
                            .Do("actionNested", () => {
                                _invokeCount++;
                                return TaskStatus.Success;
                            })
                        .End()
                        .Do("lastAction", () => {
                        _invokeCount++;
                            return TaskStatus.Success;
                        })
                    .Build();

                var sequence = tree.Root.Children[0] as Sequence;
                Assert.AreEqual(3, sequence.Children.Count);

                var nested = sequence.Children[1] as Sequence;
                Assert.AreEqual(nested.Children.Count, 1);
                Assert.AreEqual(TaskStatus.Success, tree.Tick());
                Assert.AreEqual(3, _invokeCount);
            }

            [Test]
            public void Create_two_nested_sequences_with_actions () {
                var tree = _builder
                    .Sequence("sequence")
                        .Sequence("nested")
                            .Do("actionNested", () => {
                                _invokeCount++;
                                return TaskStatus.Success;
                            })
                        .End()
                        .Sequence("nested")
                            .Do("actionNested", () => {
                                _invokeCount++;
                                return TaskStatus.Success;
                            })
                    .Build();

                var sequence = tree.Root.Children[0] as Sequence;
                Assert.AreEqual(2, sequence.Children.Count);

                var nested = sequence.Children[0] as Sequence;
                Assert.AreEqual(nested.Children.Count, 1);

                var nestedAlt = sequence.Children[1] as Sequence;
                Assert.AreEqual(nestedAlt.Children.Count, 1);

                Assert.AreEqual(TaskStatus.Success, tree.Tick());
                Assert.AreEqual(2, _invokeCount);
            }
        }

        public class SelectorMethod : BehaviorTreeBuilderTest {
            [Test]
            public void Create_a_selector () {
                var tree = _builder
                    .Selector("selector")
                        .Do("action", () => {
                            _invokeCount++;
                            return TaskStatus.Failure;
                        })
                        .Do("action", () => {
                            _invokeCount++;
                            return TaskStatus.Success;
                        })
                    .Build();

                var selector = tree.Root.Children[0] as Selector;

                Assert.AreEqual(tree.Root.Children.Count, 1);
                Assert.AreEqual(selector.Children.Count, 2);
                Assert.AreEqual(TaskStatus.Success, tree.Tick());
                Assert.AreEqual(2, _invokeCount);
            }
        }

        public class ParallelMethod : BehaviorTreeBuilderTest {
            [Test]
            public void Create_a_selector () {
                var tree = _builder
                    .Parallel()
                        .Do("action", () => {
                            _invokeCount++;
                            return TaskStatus.Success;
                        })
                        .Do("action", () => {
                            _invokeCount++;
                            return TaskStatus.Success;
                        })
                    .Build();

                var parallel = tree.Root.Children[0] as Parallel;

                Assert.AreEqual(tree.Root.Children.Count, 1);
                Assert.AreEqual(parallel.Children.Count, 2);
                Assert.AreEqual(TaskStatus.Success, tree.Tick());
                Assert.AreEqual(2, _invokeCount);
            }
        }

        public class ConditionMethod : BehaviorTreeBuilderTest {
            [Test]
            public void It_should_add_a_condition () {
                var tree = _builder
                    .Sequence()
                        .Condition("condition", () => {
                            _invokeCount++;
                            return true;
                        })
                        .Do("action", () => {
                            _invokeCount++;
                            return TaskStatus.Success;
                        })
                    .Build();

                var sequence = tree.Root.Children[0] as Sequence;
                var condition = sequence.Children[0] as ConditionGeneric;

                Assert.AreEqual(sequence.Children.Count, 2);
                Assert.IsNotNull(condition);
                Assert.AreEqual(TaskStatus.Success, tree.Tick());
                Assert.AreEqual(2, _invokeCount);
            }

            [Test]
            public void It_should_add_a_condition_without_a_name () {
                var tree = _builder
                    .Condition(() => {
                        _invokeCount++;
                        return true;
                    })
                    .Build();

                var condition = tree.Root.Children[0] as ConditionGeneric;

                Assert.IsNotNull(condition);
                Assert.AreEqual(TaskStatus.Success, tree.Tick());
                Assert.AreEqual(1, _invokeCount);
            }
        }

        public class DoMethod : BehaviorTreeBuilderTest {
            [Test]
            public void It_should_add_an_action () {
                var tree = _builder
                    .Do("action", () => {
                        _invokeCount++;
                        return TaskStatus.Success;
                    })
                    .Build();

                var action = tree.Root.Children[0] as ActionGeneric;

                Assert.IsNotNull(action);
                Assert.AreEqual(TaskStatus.Success, tree.Tick());
                Assert.AreEqual(1, _invokeCount);
            }

            [Test]
            public void It_should_add_an_action_without_a_name () {
                var tree = _builder
                    .Do(() => {
                        _invokeCount++;
                        return TaskStatus.Success;
                    })
                    .Build();

                var action = tree.Root.Children[0] as ActionGeneric;

                Assert.IsNotNull(action);
                Assert.AreEqual(TaskStatus.Success, tree.Tick());
                Assert.AreEqual(1, _invokeCount);
            }
        }

        public class DecoratorMethod : BehaviorTreeBuilderTest {
            [Test]
            public void It_should_add_a_decorator () {
                var tree = _builder
                    .Decorator("decorator", child => {
                        _invokeCount++;
                        child.Update();
                        return TaskStatus.Failure;
                    })
                        .Do("action", () => {
                            _invokeCount++;
                            return TaskStatus.Success;
                        })
                    .Build();

                var decorator = tree.Root.Children[0] as DecoratorGeneric;

                Assert.IsNotNull(decorator);
                Assert.AreEqual(TaskStatus.Failure, tree.Tick());
                Assert.AreEqual(2, _invokeCount);
            }

            [Test]
            public void It_should_add_a_decorator_without_a_name () {
                var tree = _builder
                    .Decorator(child => {
                        _invokeCount++;
                        child.Update();
                        return TaskStatus.Failure;
                    })
                        .Do("action", () => {
                            _invokeCount++;
                            return TaskStatus.Success;
                        })
                    .Build();

                var decorator = tree.Root.Children[0] as DecoratorGeneric;

                Assert.IsNotNull(decorator);
                Assert.AreEqual(TaskStatus.Failure, tree.Tick());
                Assert.AreEqual(2, _invokeCount);
            }

            [Test]
            public void It_should_move_to_the_next_node_on_End () {
                var tree = _builder
                    .Sequence()
                        .Decorator(child => {
                            _invokeCount++;
                            child.Update();
                            return TaskStatus.Success;
                        })
                            .Do("action", () => {
                                _invokeCount++;
                                return TaskStatus.Success;
                            })
                        .End()
                        .Do("action", () => {
                            _invokeCount++;
                            return TaskStatus.Success;
                        })
                    .Build();

                var sequence = tree.Root.Children[0] as Sequence;
                var decorator = sequence.Children[0] as DecoratorGeneric;

                Assert.IsNotNull(decorator);
                Assert.AreEqual(TaskStatus.Success, tree.Tick());
                Assert.AreEqual(3, _invokeCount);
            }
        }

        public class InverterMethod : BehaviorTreeBuilderTest {
            [Test]
            public void It_should_create_an_inverter () {
                var tree = _builder
                    .Inverter("inverter")
                        .Do("action", () => {
                            _invokeCount++;
                            return TaskStatus.Success;
                        })
                    .Build();

                var decorator = tree.Root.Children[0] as Inverter;

                Assert.IsNotNull(decorator);
                Assert.AreEqual(TaskStatus.Failure, tree.Tick());
                Assert.AreEqual(1, _invokeCount);
            }
        }

        public class ReturnSuccessMethod : BehaviorTreeBuilderTest {
            [Test]
            public void It_should_create_a_ReturnSuccess () {
                var tree = _builder
                    .ReturnSuccess("return success")
                        .Do("action", () => {
                            _invokeCount++;
                            return TaskStatus.Failure;
                        })
                    .Build();

                var decorator = tree.Root.Children[0] as ReturnSuccess;

                Assert.IsNotNull(decorator);
                Assert.AreEqual(TaskStatus.Success, tree.Tick());
                Assert.AreEqual(1, _invokeCount);
            }
        }

        public class ReturnFailureMethod : BehaviorTreeBuilderTest {
            [Test]
            public void It_should_create_a_ReturnFailure () {
                var tree = _builder
                    .ReturnFailure("return failure")
                        .Do("action", () => {
                            _invokeCount++;
                            return TaskStatus.Success;
                        })
                    .Build();

                var decorator = tree.Root.Children[0] as ReturnFailure;

                Assert.IsNotNull(decorator);
                Assert.AreEqual(TaskStatus.Failure, tree.Tick());
                Assert.AreEqual(1, _invokeCount);
            }
        }

        public class RandomChanceMethod : BehaviorTreeBuilderTest {
            [Test]
            public void It_should_add_a_random_chance () {
                var tree = _builder
                    .Sequence()
                        .RandomChance("random", 1, 1)
                        .Do("action", () => {
                            _invokeCount++;
                            return TaskStatus.Success;
                        })
                    .Build();

                var sequence = tree.Root.Children[0] as Sequence;
                var condition = sequence.Children[0] as RandomChance;

                Assert.AreEqual(sequence.Children.Count, 2);
                Assert.IsNotNull(condition);
                Assert.AreEqual(TaskStatus.Success, tree.Tick());
                Assert.AreEqual(1, _invokeCount);
            }
        }

        public class WaitMethod : BehaviorTreeBuilderTest {
            [Test]
            public void It_should_add_a_wait_node () {
                var tree = _builder
                    .Sequence()
                        .Wait("wait", 2)
                        .Do("action", () => {
                            _invokeCount++;
                            return TaskStatus.Success;
                        })
                    .Build();

                var sequence = tree.Root.Children[0] as Sequence;
                var wait = sequence.Children[0] as Wait;

                Assert.IsNotNull(wait);
                Assert.AreEqual(sequence.Children.Count, 2);
                Assert.AreEqual(TaskStatus.Continue, tree.Tick());
                Assert.AreEqual(TaskStatus.Continue, tree.Tick());
                Assert.AreEqual(TaskStatus.Success, tree.Tick());
                Assert.AreEqual(1, _invokeCount);
            }
        }

        public class WaitTimeMethod : BehaviorTreeBuilderTest {
            [Test]
            public void It_should_add_a_WaitTime_action () {
                var tree = _builder
                    .WaitTime("Custom")
                    .Build();

                var waitTime = tree.Root.Children[0] as WaitTime;

                Assert.IsNotNull(waitTime);
            }

            [Test]
            public void It_should_set_WaitTime_duration () {
                var tree = _builder
                    .WaitTime(2f)
                    .Build();

                var waitTime = tree.Root.Children[0] as WaitTime;

                Assert.AreEqual(2f, waitTime.time);
            }
        }

        public class SpliceMethod : BehaviorTreeBuilderTest {
            [Test]
            public void It_should_not_fail_when_aborting_a_built_condition () {
                var treeAlt = new BehaviorTreeBuilder(null)
                    .Sequence()
                        .Condition(() => false)
                    .End()
                .Build();

                var tree = _builder
                    .Parallel()
                        .Splice(treeAlt)
                    .Build();

                Assert.DoesNotThrow(() => {
                    tree.Tick();
                });
            }
        }
    }
}
