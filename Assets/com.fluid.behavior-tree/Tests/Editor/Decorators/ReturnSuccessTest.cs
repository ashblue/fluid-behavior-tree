﻿using CleverCrow.Fluid.BTs.Decorators;
using CleverCrow.Fluid.BTs.Tasks;
using NUnit.Framework;
using Assert = UnityEngine.Assertions.Assert;

namespace CleverCrow.Fluid.BTs.Testing {
    public class ReturnSuccessTest {
        public class UpdateMethod {
            [Test]
            public void Returns_success_on_child_failure () {
                var returnSuccess = new ReturnSuccess();
                returnSuccess.AddChild(A.TaskStub().WithUpdateStatus(TaskStatus.Failure).Build());

                Assert.AreEqual(TaskStatus.Success, returnSuccess.Update());
            }

            [Test]
            public void Returns_success_on_child_success () {
                var returnSuccess = new ReturnSuccess();
                returnSuccess.AddChild(A.TaskStub().WithUpdateStatus(TaskStatus.Success).Build());

                Assert.AreEqual(TaskStatus.Success, returnSuccess.Update());
            }

            [Test]
            public void Returns_continue_on_child_continue () {
                var returnSuccess = new ReturnSuccess();
                returnSuccess.AddChild(A.TaskStub().WithUpdateStatus(TaskStatus.Continue).Build());

                Assert.AreEqual(TaskStatus.Continue, returnSuccess.Update());
            }
        }
    }
}