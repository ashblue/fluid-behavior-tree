using CleverCrow.Fluid.BTs.TaskParents;
using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Tasks.Actions;
using NSubstitute;
using NUnit.Framework;

namespace CleverCrow.Fluid.BTs.Testing {
    public class TaskParentTest {
        private TaskParentExample taskParent;

        [SetUp]
        public void SetTaskParent () {
            taskParent = new TaskParentExample();
        }

        public class TaskParentExample : TaskParentBase {
            public int childCount = -1;
            public int resetCount = 0;
            public int initCount = 0;
            public int startCount = 0;
            public int exitCount = 0;
            public TaskStatus status = TaskStatus.Success;

            protected override int MaxChildren => childCount;

            protected override TaskStatus OnUpdate () {
                return status;
            }

            public override void Reset () {
                base.Reset();
                resetCount += 1;
            }

            protected override void OnInit() {
                initCount += 1;
            }

            protected override void OnStart() {
                startCount += 1;
            }
            
            protected override void OnExit() {
                exitCount += 1;
            }
        }

        public class TaskExample : ActionBase {
            protected override TaskStatus OnUpdate () {
                return TaskStatus.Success;
            }
        }

        public class TriggeringOnInit : TaskParentTest {
            [Test]
            public void Triggers_on_1st_update () {
                taskParent.status = TaskStatus.Success;

                taskParent.Update();

                Assert.AreEqual(1, taskParent.initCount);
            }

            [Test]
            public void Does_not_trigger_on_2nd_update () {
                taskParent.status = TaskStatus.Success;

                taskParent.Update();
                taskParent.Update();

                Assert.AreEqual(1, taskParent.initCount);
            }

            [Test]
            public void Does_not_trigger_on_reset () {
                taskParent.status = TaskStatus.Success;

                taskParent.Reset();
                taskParent.Update();

                Assert.AreEqual(1, taskParent.initCount);
            }
        }
        
        public class TriggeringOnExit : TaskParentTest {
            [Test]
            public void Does_not_trigger_on_continue () {
                taskParent.status = TaskStatus.Continue;
                taskParent.Update();

                Assert.AreEqual(0, taskParent.exitCount);
            }

            [Test]
            public void Triggers_on_success () {
                taskParent.status = TaskStatus.Success;
                taskParent.Update();

                Assert.AreEqual(1, taskParent.exitCount);
            }

            [Test]
            public void Triggers_on_failure () {
                taskParent.status = TaskStatus.Failure;
                taskParent.Update();

                Assert.AreEqual(1, taskParent.exitCount);
            }
        }

        public class TriggeringOnStart : TaskParentTest {
            [Test]
            public void Trigger_on_1st_run () {
                taskParent.Update();

                Assert.AreEqual(1, taskParent.startCount);
            }

            [Test]
            public void Triggers_on_reset () {
                taskParent.status = TaskStatus.Continue;

                taskParent.Update();
                taskParent.Reset();
                taskParent.Update();

                Assert.AreEqual(2, taskParent.startCount);
            }
        }
        public class TriggeringReset : TaskParentTest {
            [Test]
            public void It_should_trigger_Reset_on_success () {
                taskParent.status = TaskStatus.Success;

                taskParent.Update();

                Assert.AreEqual(1, taskParent.resetCount);
            }

            [Test]
            public void It_should_trigger_Reset_on_failure () {
                taskParent.status = TaskStatus.Failure;

                taskParent.Update();

                Assert.AreEqual(1, taskParent.resetCount);
            }

            [Test]
            public void It_should_not_trigger_Reset_on_continue () {
                taskParent.status = TaskStatus.Continue;

                taskParent.Update();

                Assert.AreEqual(0, taskParent.resetCount);
            }
        }

        public class EnabledProperty : TaskParentTest {
            [Test]
            public void Returns_enabled_if_child () {
                taskParent.AddChild(A.TaskStub().Build());

                Assert.IsTrue(taskParent.Enabled);
            }

            [Test]
            public void Returns_disabled_if_child_and_set_to_disabled () {
                taskParent.AddChild(A.TaskStub().Build());
                taskParent.Enabled = false;

                Assert.IsFalse(taskParent.Enabled);
            }
        }

        public class AddChildMethod : TaskParentTest {
            [Test]
            public void Adds_a_child () {
                taskParent.AddChild(new TaskExample());

                Assert.AreEqual(1, taskParent.Children.Count);
            }

            [Test]
            public void Adds_two_children () {
                taskParent.AddChild(new TaskExample());
                taskParent.AddChild(new TaskExample());

                Assert.AreEqual(2, taskParent.Children.Count);
            }

            [Test]
            public void Ignores_overflowing_children () {
                taskParent.childCount = 1;

                taskParent.AddChild(new TaskExample());
                taskParent.AddChild(new TaskExample());

                Assert.AreEqual(1, taskParent.Children.Count);
            }

            [Test]
            public void Does_not_add_disabled_children () {
                var child = A.TaskStub()
                    .WithEnabled(false)
                    .Build();

                taskParent.AddChild(child);

                Assert.AreEqual(0, taskParent.Children.Count);
            }
        }
    }
}
