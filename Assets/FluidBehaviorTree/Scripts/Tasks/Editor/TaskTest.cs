using Adnc.FluidBT.Tasks;
using Adnc.FluidBT.Tasks.Actions;
using Adnc.FluidBT.Trees;
using NUnit.Framework;
using UnityEngine;

namespace Adnc.FluidBT.Testing {
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
            TaskExample node;

            [SetUp]
            public void SetUpNode () {
                node = new TaskExample();
            }

            public class TreeTickCountChange : UpdateMethod {
                private GameObject _go;
                
                [SetUp]
                public void BeforEach () {
                    _go = new GameObject();
                }

                [TearDown]
                public void AfterEach () {
                    Object.DestroyImmediate(_go);
                }
                
                [Test]
                public void Retriggers_start_if_tick_count_changes () {
                    var tree = new BehaviorTree(_go);
                    tree.AddNode(tree.Root, node);
                    
                    tree.Tick();
                    tree.Tick();
                    
                    Assert.AreEqual(2, node.StartCount);
                }
                
                [Test]
                public void Does_not_retrigger_start_if_tick_count_stays_the_same () {
                    node.status = TaskStatus.Continue;
                    
                    var tree = new BehaviorTree(_go);
                    tree.AddNode(tree.Root, node);
                    
                    tree.Tick();
                    tree.Tick();
                    
                    Assert.AreEqual(1, node.StartCount);
                }
            }

            public class StartEvent : UpdateMethod {
                [Test]
                public void Trigger_on_1st_run () {
                    node.Update();

                    Assert.AreEqual(1, node.StartCount);
                }

                [Test]
                public void Do_not_trigger_on_2nd_run () {
                    node.Update();
                    node.Update();

                    Assert.AreEqual(1, node.StartCount);
                }

                [Test]
                public void Triggers_on_reset () {
                    node.status = TaskStatus.Continue;

                    node.Update();
                    node.Reset();
                    node.Update();
                    
                    Assert.AreEqual(2, node.StartCount);
                }
            }

            public class InitEvent : UpdateMethod {
                [SetUp]
                public void TriggerUpdate () {
                    node.Update();
                }

                [Test]
                public void Triggers_on_1st_update () {
                    Assert.AreEqual(1, node.InitCount);
                }
                
                [Test]
                public void Does_not_trigger_on_2nd_update () {
                    node.Update();

                    Assert.AreEqual(1, node.InitCount);
                }

                [Test]
                public void Does_not_trigger_on_reset () {
                    node.Reset();
                    node.Update();

                    Assert.AreEqual(1, node.InitCount);
                }

                [Test]
                public void Triggers_on_reset_hard () {
                    node.Reset(true);
                    node.Update();

                    Assert.AreEqual(2, node.InitCount);
                }
            }

            public class ExitEvent : UpdateMethod {
                [Test]
                public void Does_not_trigger_on_continue () {
                    node.status = TaskStatus.Continue;
                    node.Update();

                    Assert.AreEqual(0, node.ExitCount);
                }

                [Test]
                public void Triggers_on_success () {
                    node.status = TaskStatus.Success;
                    node.Update();

                    Assert.AreEqual(1, node.ExitCount);
                }

                [Test]
                public void Triggers_on_failure () {
                    node.status = TaskStatus.Failure;
                    node.Update();

                    Assert.AreEqual(1, node.ExitCount);
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
                task.Reset();
                task.End();

                Assert.AreEqual(0, task.ExitCount);
            }
        }
    }
}