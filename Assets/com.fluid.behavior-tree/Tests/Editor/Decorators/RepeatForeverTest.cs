using CleverCrow.Fluid.BTs.Decorators;
using CleverCrow.Fluid.BTs.Tasks;
using NUnit.Framework;

namespace CleverCrow.Fluid.BTs.Testing {
    public class RepeatForeverTest {
        public class UpdateMethod {
            private RepeatForever Setup (ITask child) {
                var repeat = new RepeatForever();
                repeat.AddChild(child);

                return repeat;
            }

            public class WhenChildReturnsFailure : UpdateMethod {
                [Test]
                public void It_should_return_continue () {
                    var stub = A.TaskStub().WithUpdateStatus(TaskStatus.Failure).Build();

                    var repeater = Setup(stub);
                    var status = repeater.Update();

                    Assert.AreEqual(TaskStatus.Continue, status);
                }
            }

            public class WhenChildReturnsSuccess : UpdateMethod {
                [Test]
                public void It_should_return_continue () {
                    var stub = A.TaskStub().WithUpdateStatus(TaskStatus.Success).Build();

                    var repeater = Setup(stub);
                    var status = repeater.Update();

                    Assert.AreEqual(TaskStatus.Continue, status);
                }
            }

            public class WhenChildReturnsContinue : UpdateMethod {
                [Test]
                public void It_should_return_continue () {
                    var stub = A.TaskStub().WithUpdateStatus(TaskStatus.Continue).Build();

                    var repeater = Setup(stub);
                    var status = repeater.Update();

                    Assert.AreEqual(TaskStatus.Continue, status);
                }
            }
        }
    }
}
