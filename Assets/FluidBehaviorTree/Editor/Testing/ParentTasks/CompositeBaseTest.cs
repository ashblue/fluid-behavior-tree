using Adnc.FluidBT.TaskParents;
using NSubstitute;
using NUnit.Framework;

namespace Adnc.FluidBT.Testing {
    public class CompositeBaseTest {
        private CompositeExample _composite;
        
        public class CompositeExample : CompositeBase {
            public void SetChildIndex (int index) {
                ChildIndex = index;
            }
        }

        [SetUp]
        public void Set_composite () {
            _composite = new CompositeExample();
        }
        
        public class EndMethod : CompositeBaseTest {
            [Test]
            public void Does_not_fail_if_children_empty () {                
                _composite.End();
            }

            [Test]
            public void Calls_end_on_current_child () {
                var child = A.TaskStub().Build();
                _composite.AddChild(child);
                
                _composite.End();
                child.Received(1).End();
            }
        }
        
        public class ResetMethod : CompositeBaseTest {
            [Test]
            public void Resets_child_node_pointer () {
                _composite.SetChildIndex(2);
                
                _composite.Reset();

                Assert.AreEqual(0, _composite.ChildIndex);
            }

            [Test]
            public void Clears_all_lower_priorities () {
                _composite.AddChild(A.TaskStub().Build());
                
                _composite.Reset();
                
                Assert.AreEqual(0, _composite.AbortLowerPriorities.Count);
            }

            [Test]
            public void Clears_self_abort_task () {
                var condition = A.TaskStub().WithAbortConditionSelf(true).Build();
                _composite.AddChild(condition);

                _composite.Reset();
                
                Assert.AreEqual(null, _composite.SelfAbortTask);
            }
        }
    }
}