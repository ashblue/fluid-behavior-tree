using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Tasks.Actions;
using CleverCrow.Fluid.BTs.Trees;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace CleverCrow.Fluid.BTs.Testing {
    public class TaskTest {
        public class TaskExample : ActionBase {
            public int StartCount { get; private set; }
            public int InitCount { get; private set; }
            public int ExitCount { get; private set; }
            public TaskStatus status = TaskStatus.Success;

            protected override TaskStatus OnUpdate () {
                return status;
            }

            protected override void OnStart () {
                StartCount++;
            }

            protected override void OnInit () {
                InitCount++;
            }

            protected override void OnExit () {
                ExitCount++;
            }
        }

        public class UpdateMethod {
            TaskExample _node;

            [SetUp]
            public void SetUpNode () {
                _node = new TaskExample();
            }

            public class RegisteringContinueNodes : UpdateMethod {
                [Test]
                public void It_should_call_BehaviorTree_AddActiveTask_on_continue () {
                    var tree = Substitute.For<IBehaviorTree>();
                    _node.status = TaskStatus.Continue;
                    _node.ParentTree = tree;

                    _node.Update();

                    tree.Received(1).AddActiveTask(_node);
                }

                [Test]
                public void It_should_not_call_BehaviorTree_AddActiveTask_on_continue_twice () {
                    var tree = Substitute.For<IBehaviorTree>();
                    _node.status = TaskStatus.Continue;
                    _node.ParentTree = tree;

                    _node.Update();
                    _node.Update();

                    tree.Received(1).AddActiveTask(_node);
                }

                [Test]
                public void It_should_call_BehaviorTree_AddActiveTask_again_after_Reset () {
                    var tree = Substitute.For<IBehaviorTree>();
                    _node.status = TaskStatus.Continue;
                    _node.ParentTree = tree;

                    _node.Update();
                    _node.ResetTask();
                    _node.Update();

                    tree.Received(2).AddActiveTask(_node);
                }

                [Test]
                public void It_should_call_BehaviorTree_RemoveActiveTask_after_returning_continue () {
                    var tree = Substitute.For<IBehaviorTree>();
                    _node.status = TaskStatus.Continue;
                    _node.ParentTree = tree;

                    _node.Update();

                    _node.status = TaskStatus.Success;
                    _node.Update();

                    tree.Received(1).RemoveActiveTask(_node);
                }

                [Test]
                public void It_should_not_call_BehaviorTree_AddActiveTask_if_continue_was_not_returned () {
                    var tree = Substitute.For<IBehaviorTree>();
                    _node.status = TaskStatus.Success;
                    _node.ParentTree = tree;

                    _node.Update();

                    tree.Received(0).RemoveActiveTask(_node);
                }
            }

            public class TreeTickCountChange : UpdateMethod {
                private GameObject _go;

                [SetUp]
                public void BeforeEach () {
                    _go = new GameObject();
                }

                [TearDown]
                public void AfterEach () {
                    Object.DestroyImmediate(_go);
                }

                [Test]
                public void Retriggers_start_if_tick_count_changes () {
                    var tree = ScriptableObject.CreateInstance<BehaviorTree>();
                    tree.SetOwner(_go);
                    tree.AddNode(tree.Root, _node);

                    tree.Tick();
                    tree.Tick();

                    Assert.AreEqual(2, _node.StartCount);
                }

                [Test]
                public void Does_not_retrigger_start_if_tick_count_stays_the_same () {
                    _node.status = TaskStatus.Continue;

                    var tree = ScriptableObject.CreateInstance<BehaviorTree>();
                    tree.SetOwner(_go);
                    tree.AddNode(tree.Root, _node);

                    tree.Tick();
                    tree.Tick();

                    Assert.AreEqual(1, _node.StartCount);
                }
            }

            public class StartEvent : UpdateMethod {
                [Test]
                public void Trigger_on_1st_run () {
                    _node.Update();

                    Assert.AreEqual(1, _node.StartCount);
                }

                [Test]
                public void Triggers_on_reset () {
                    _node.status = TaskStatus.Continue;

                    _node.Update();
                    _node.ResetTask();
                    _node.Update();

                    Assert.AreEqual(2, _node.StartCount);
                }
            }

            public class InitEvent : UpdateMethod {
                [SetUp]
                public void TriggerUpdate () {
                    _node.Update();
                }

                [Test]
                public void Triggers_on_1st_update () {
                    Assert.AreEqual(1, _node.InitCount);
                }

                [Test]
                public void Does_not_trigger_on_2nd_update () {
                    _node.Update();

                    Assert.AreEqual(1, _node.InitCount);
                }

                [Test]
                public void Does_not_trigger_on_reset () {
                    _node.ResetTask();
                    _node.Update();

                    Assert.AreEqual(1, _node.InitCount);
                }
            }

            public class ExitEvent : UpdateMethod {
                [Test]
                public void Does_not_trigger_on_continue () {
                    _node.status = TaskStatus.Continue;
                    _node.Update();

                    Assert.AreEqual(0, _node.ExitCount);
                }

                [Test]
                public void Triggers_on_success () {
                    _node.status = TaskStatus.Success;
                    _node.Update();

                    Assert.AreEqual(1, _node.ExitCount);
                }

                [Test]
                public void Triggers_on_failure () {
                    _node.status = TaskStatus.Failure;
                    _node.Update();

                    Assert.AreEqual(1, _node.ExitCount);
                }
            }
        }

        public class EndMethod {
            private TaskExample task;

            [SetUp]
            public void CreateTask () {
                task = new TaskExample();
            }

            [Test]
            public void Does_not_call_exit_if_not_run () {
                task.End();

                Assert.AreEqual(0, task.ExitCount);
            }

            [Test]
            public void Does_not_call_exit_if_last_status_was_success () {
                task.status = TaskStatus.Success;

                task.Update();
                task.End();

                Assert.AreEqual(1, task.ExitCount);
            }

            [Test]
            public void Does_not_call_exit_if_last_status_was_failure () {
                task.status = TaskStatus.Failure;

                task.Update();
                task.End();

                Assert.AreEqual(1, task.ExitCount);
            }

            [Test]
            public void Calls_exit_if_last_status_was_continue () {
                task.status = TaskStatus.Continue;

                task.Update();
                task.End();

                Assert.AreEqual(1, task.ExitCount);
            }

            [Test]
            public void Reset_prevents_exit_from_being_called_when_it_should () {
                task.status = TaskStatus.Continue;

                task.Update();
                task.ResetTask();
                task.End();

                Assert.AreEqual(0, task.ExitCount);
            }
        }
    }
}
