using Adnc.FluidBT.Tasks;
using Adnc.FluidBT.Tasks.Actions;
using NUnit.Framework;

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
    }
}