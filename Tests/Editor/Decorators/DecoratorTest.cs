using CleverCrow.Fluid.BTs.Decorators;
using CleverCrow.Fluid.BTs.Tasks;
using NSubstitute;
using NUnit.Framework;

namespace CleverCrow.Fluid.BTs.Testing {
    public class DecoratorTest {
        private DecoratorExample _decorator;
        
        public class DecoratorExample : DecoratorBase {
            public TaskStatus status;

            protected override TaskStatus OnUpdate () {
                return status;
            }
        }

        [SetUp]
        public void BeforeEach () {
            _decorator = new DecoratorExample();
            _decorator.AddChild(A.TaskStub().Build());
        }

        public class EnabledProperty : DecoratorTest {
            [Test]
            public void Returns_true_if_child_is_set () {
                Assert.IsTrue(_decorator.Enabled);
            }

            [Test]
            public void Returns_false_if_child_is_set_but_set_to_false () {
                _decorator.Enabled = false;

                Assert.IsFalse(_decorator.Enabled);
            }
        }

        public class UpdateMethod : DecoratorTest {
            [Test]
            public void Sets_LastUpdate_to_returned_status_value () {
                _decorator.status = TaskStatus.Failure;

                _decorator.Update();

                Assert.AreEqual(TaskStatus.Failure, _decorator.LastStatus);
            }

            [Test]
            public void It_should_return_failure_if_a_child_is_missing () {
                _decorator.Children.RemoveAt(0);
                Assert.AreEqual(TaskStatus.Failure, _decorator.Update());
            }
        }

        public class EndMethod : DecoratorTest {
            [Test]
            public void Calls_end_on_child () {
                _decorator.End();

                _decorator.Child.Received(1).End();
            }
        }
    }
}