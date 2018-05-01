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

        public class AddChild : CompositeBaseTest {
            [Test]
            public void Adds_first_node_to_self_abort_if_condition () {
                var child = A.TaskStub().WithAbortConditionSelf(true).Build();

                _composite.AbortType = AbortType.Self;
                _composite.AddChild(child);

                Assert.AreEqual(child, _composite.SelfAbortTask);
            }

            [Test]
            public void Does_not_add_first_node_as_self_abort_if_non_condition () {
                var child = A.TaskStub().Build();

                _composite.AddChild(child);

                Assert.AreEqual(null, _composite.SelfAbortTask);
            }

            [Test]
            public void Does_not_add_disabled_nodes () {
                var child = A.TaskStub()
                    .WithEnabled(false)
                    .WithAbortConditionSelf(true)
                    .Build();

                _composite.AbortType = AbortType.Self;
                _composite.AddChild(child);

                Assert.AreEqual(null, _composite.SelfAbortTask);
            }
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