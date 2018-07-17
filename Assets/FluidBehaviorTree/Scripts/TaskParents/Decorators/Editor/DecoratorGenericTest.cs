using Adnc.FluidBT.Decorators;
using Adnc.FluidBT.Tasks;
using NUnit.Framework;

namespace Adnc.FluidBT.Testing {
    public class DecoratorGenericTest {
        public class UpdateMethod {
            [Test]
            public void Can_invert_status_of_child () {
                var task = new DecoratorGeneric {
                    child = A.TaskStub().Build(),
                    updateLogic = (dec) => {
                        if (dec.child.Update() == TaskStatus.Success) {
                            return TaskStatus.Failure;
                        }

                        return TaskStatus.Success;
                    }
                };

                Assert.AreEqual(TaskStatus.Failure, task.Update());
            }

            [Test]
            public void Returns_child_status_without_update_logic () {
                var task = new DecoratorGeneric {
                    child = A.TaskStub().Build()
                };

                Assert.AreEqual(TaskStatus.Success, task.Update());
            }
        }
    }
}