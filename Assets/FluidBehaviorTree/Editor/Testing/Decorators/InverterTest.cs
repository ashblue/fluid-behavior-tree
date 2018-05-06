using Adnc.FluidBT.Decorators;
using Adnc.FluidBT.Tasks;
using NSubstitute;
using NUnit.Framework;

namespace Adnc.FluidBT.Testing {
    public class InverterTest {
        public class EnabledProperty {
            [Test]
            public void Returns_true_if_child_is_set () {
                var inverter = new Inverter { child = A.TaskStub().Build() };

                Assert.IsTrue(inverter.Enabled);
            }

            [Test]
            public void Returns_false_if_child_is_not_set () {
                var inverter = new Inverter();

                Assert.IsFalse(inverter.Enabled);
            }

            [Test]
            public void Returns_false_if_child_is_disabled () {
                var inverter = new Inverter { child = A.TaskStub().Build() };
                inverter.child.Enabled.Returns(false);

                Assert.IsTrue(inverter.Enabled);
            }

            [Test]
            public void Returns_false_if_child_is_set_but_set_to_false () {
                var inverter = new Inverter { child = A.TaskStub().Build() };
                inverter.Enabled = false;

                Assert.IsFalse(inverter.Enabled);
            }
        }

        public class UpdateMethod {
            [Test]
            public void Returns_failure_when_child_returns_success () {
                var inverter = new Inverter {
                    child = A.TaskStub().Build()
                };

                Assert.AreEqual(TaskStatus.Failure, inverter.Update());
            }

            [Test]
            public void Returns_true_when_child_returns_false () {
                var inverter = new Inverter {
                    child = A.TaskStub().WithUpdateStatus(TaskStatus.Failure).Build()
                };

                Assert.AreEqual(TaskStatus.Success, inverter.Update());
            }

            [Test]
            public void Returns_continue_when_child_returns_continue () {
                var inverter = new Inverter {
                    child = A.TaskStub().WithUpdateStatus(TaskStatus.Continue).Build()
                };

                Assert.AreEqual(TaskStatus.Continue, inverter.Update());
            }

            [Test]
            public void Does_not_crash_if_no_child () {
                var inverter = new Inverter();

                inverter.Update();
            }

            [Test]
            public void Sets_last_status_to_inverted_value () {
                var inverter = new Inverter {
                    child = A.TaskStub().Build()
                };

                var status = inverter.Update();

                Assert.AreEqual(status, inverter.LastStatus);
            }
        }

        public class GetAbortConditionMethod {
            [Test]
            public void Returns_itself_as_an_abort_condition_if_child_returns () {
                var task = A.TaskStub()
                    .WithAbortConditionSelf(true)
                    .Build();

                var inverter = new Inverter { child = task };

                Assert.AreEqual(inverter, inverter.GetAbortCondition());
            }

            [Test]
            public void Does_not_return_an_abort_condition_if_child_does_not_return () {
                var inverter = new Inverter { child = A.TaskStub().Build() };

                Assert.AreEqual(null, inverter.GetAbortCondition());
            }
        }

        public class GetAbortStatusMethod {
            [Test]
            public void Returns_status_from_update () {
                var task = A.TaskStub()
                    .WithAbortConditionSelf(true)
                    .Build();

                var inverter = new Inverter { child = task };

                Assert.AreEqual(TaskStatus.Failure, inverter.GetAbortStatus());
            }
        }

        public class EndMethod {
            [Test]
            public void Calls_end_on_child () {
                var task = A.TaskStub().Build();
                var inverter = new Inverter { child = task };

                inverter.End();

                task.Received(1).End();
            }
        }

        public class ResetMethod {
            [Test]
            public void Runs_reset_on_child () {
                var task = A.TaskStub().Build();
                var inverter = new Inverter { child = task };

                inverter.Reset();

                task.Received(1).Reset();
            }
        }
    }
}