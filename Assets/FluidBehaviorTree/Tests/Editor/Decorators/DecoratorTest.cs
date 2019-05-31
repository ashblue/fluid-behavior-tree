using CleverCrow.Fluid.BTs.Decorators;
using CleverCrow.Fluid.BTs.Tasks;
using NSubstitute;
using NUnit.Framework;

namespace CleverCrow.Fluid.BTs.Testing {
    public class DecoratorTest {
        public class DecoratorExample : DecoratorBase {
            public TaskStatus status;

            protected override TaskStatus OnUpdate () {
                return status;
            }
        }

        public class EnabledProperty {
            [Test]
            public void Returns_true_if_child_is_set () {
                var decorator = new DecoratorExample();
                decorator.AddChild(A.TaskStub().Build());
                
                Assert.IsTrue(decorator.Enabled);
            }

            [Test]
            public void Returns_false_if_child_is_set_but_set_to_false () {
                var decorator = new DecoratorExample();
                decorator.AddChild(A.TaskStub().Build());
                decorator.Enabled = false;

                Assert.IsFalse(decorator.Enabled);
            }
        }

        public class UpdateMethod {
            [Test]
            public void Sets_LastUpdate_to_returned_status_value () {
                var decorator = new DecoratorExample();
                decorator.status = TaskStatus.Failure;

                decorator.Update();

                Assert.AreEqual(TaskStatus.Failure, decorator.LastStatus);
            }
        }

        public class EndMethod {
            [Test]
            public void Calls_end_on_child () {
                var decorator = new DecoratorExample();
                decorator.AddChild(A.TaskStub().Build());

                decorator.End();

                decorator.Child.Received(1).End();
            }
        }
    }
}