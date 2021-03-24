using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Tasks.Actions;
using NUnit.Framework;

namespace CleverCrow.Fluid.BTs.Testing {
    public class ActionGenericTest {
        public class UpdateMethod {
            [Test]
            public void It_should_execute_a_generic_function () {
                var task = new ActionGeneric {
                    updateLogic = () => TaskStatus.Failure
                };

                Assert.AreEqual(TaskStatus.Failure, task.Update());
            }

            [Test]
            public void It_should_not_fail_without_a_generic_function () {
                var task = new ActionGeneric();
                task.Update();
            }

            [Test]
            public void It_should_execute_a_start_hook () {
                var test = 0;
                var task = new ActionGeneric {
                    startLogic = () => { test++; }
                };

                task.Update();

                Assert.AreEqual(1, test);
            }

            [Test]
            public void It_should_execute_a_init_hook () {
                var test = 0;
                var task = new ActionGeneric {
                    initLogic = () => { test++; }
                };

                task.Update();

                Assert.AreEqual(1, test);
            }

            [Test]
            public void It_should_execute_init_hook_on_continue () {
                var test = 0;
                var task = new ActionGeneric {
                    initLogic = () => { test++; },
                    updateLogic = () => TaskStatus.Continue,
                };

                task.Update();

                Assert.AreEqual(1, test);
            }

            [Test]
            public void It_should_execute_a_exit_hook () {
                var test = 0;
                var task = new ActionGeneric {
                    exitLogic = () => { test++; }
                };

                task.Update();

                Assert.AreEqual(1, test);
            }
        }
    }
}
