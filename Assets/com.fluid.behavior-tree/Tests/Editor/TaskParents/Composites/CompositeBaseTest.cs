using CleverCrow.Fluid.BTs.TaskParents.Composites;
using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Trees;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace CleverCrow.Fluid.BTs.Testing {
    public class CompositeBaseTest {
        private CompositeExample _composite;
        
        public class CompositeExample : CompositeBase {
            public TaskStatus status;
            
            public void SetChildIndex (int index) {
                ChildIndex = index;
            }

            protected override TaskStatus OnUpdate () {
                return status;
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

                Assert.AreEqual(0, _composite.Children.Count);
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
            private GameObject _go;
                
            [SetUp]
            public void BeforEach () {
                _go = new GameObject();
            }

            [TearDown]
            public void AfterEach () {
                Object.DestroyImmediate(_go);
            }
            
            [Test]
            public void Resets_child_node_pointer () {
                _composite.SetChildIndex(2);
                
                _composite.ResetTask();

                Assert.AreEqual(0, _composite.ChildIndex);
            }

            [Test]
            public void Resets_pointer_if_tick_count_changes () {
                var tree = ScriptableObject.CreateInstance<BehaviorTree>();
                tree.SetOwner(_go);
                tree.AddNode(tree.Root, _composite);
                tree.AddNode(_composite, Substitute.For<ITask>());
                    
                tree.Tick();
                _composite.SetChildIndex(2);
                tree.Tick();
                    
                Assert.AreEqual(0, _composite.ChildIndex);
            }

            [Test]
            public void Does_not_reset_pointer_if_tick_count_does_not_change () {
                var task = Substitute.For<ITask>();
                var tree = ScriptableObject.CreateInstance<BehaviorTree>();
                tree.SetOwner(_go);
                tree.AddNode(tree.Root, _composite);
                tree.AddNode(_composite, task);
                _composite.status = TaskStatus.Continue;
                    
                tree.Tick();
                _composite.SetChildIndex(1);
                tree.Tick();
                    
                Assert.AreEqual(1, _composite.ChildIndex);
            }
        }
    }
}
