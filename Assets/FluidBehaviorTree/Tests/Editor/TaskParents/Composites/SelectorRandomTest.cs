using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Testing;
using NUnit.Framework;

namespace CleverCrow.Fluid.BTs.TaskParents.Composites.Editors.Tests {
    public class SelectorRandomTest {
        public class UpdateMethod {
            [Test]
            public void It_should_return_success_if_a_child_returns_success () {
                var child = A.TaskStub().WithUpdateStatus(TaskStatus.Success).Build();
                var selectorRandom = new SelectorRandom();
                selectorRandom.AddChild(child);
                
                Assert.AreEqual(TaskStatus.Success, selectorRandom.Update());
            }

            [Test]
            public void It_should_return_continue_if_a_child_returns_continue () {
                var child = A.TaskStub().WithUpdateStatus(TaskStatus.Continue).Build();
                var selectorRandom = new SelectorRandom();
                selectorRandom.AddChild(child);
                
                Assert.AreEqual(TaskStatus.Continue, selectorRandom.Update());
            }
            
            [Test]
            public void It_should_return_failure_if_empty () {
                var selectorRandom = new SelectorRandom();
                
                Assert.AreEqual(TaskStatus.Failure, selectorRandom.Update());

            }

            [Test]
            public void It_should_return_failure_if_child_returns_failure () {
                var child = A.TaskStub().WithUpdateStatus(TaskStatus.Failure).Build();
                var selectorRandom = new SelectorRandom();
                selectorRandom.AddChild(child);
                
                Assert.AreEqual(TaskStatus.Failure, selectorRandom.Update());

            }
        }
    }
}