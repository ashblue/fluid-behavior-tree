using CleverCrow.Fluid.BTs.Decorators;
using CleverCrow.Fluid.BTs.Tasks;
using NUnit.Framework;

namespace CleverCrow.Fluid.BTs.Testing {
    public class DecoratorGenericTest {
        public class UpdateMethod {
            [Test]
            public void Can_invert_status_of_child () {
                var task = new DecoratorGeneric {
                    updateLogic = (child) => {
                        if (child.Update() == TaskStatus.Success) {
                            return TaskStatus.Failure;
                        }

                        return TaskStatus.Success;
                    }
                };
                task.AddChild(A.TaskStub().Build());

                Assert.AreEqual(TaskStatus.Failure, task.Update());
            }

            [Test]
            public void Returns_child_status_without_update_logic () {
                var task = new DecoratorGeneric();
                task.AddChild(A.TaskStub().Build());

                Assert.AreEqual(TaskStatus.Success, task.Update());
            }
        }
    }
}