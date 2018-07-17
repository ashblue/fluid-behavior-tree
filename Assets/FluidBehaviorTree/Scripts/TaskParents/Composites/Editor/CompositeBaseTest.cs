using Adnc.FluidBT.TaskParents.Composites;
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
            public void Does_not_add_disabled_nodes () {
                var child = A.TaskStub()
                    .WithEnabled(false)
                    .Build();

                _composite.AddChild(child);

                Assert.AreEqual(0, _composite.children.Count);
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
        }
    }
}