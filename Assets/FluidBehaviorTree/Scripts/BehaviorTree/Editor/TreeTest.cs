using Adnc.FluidBT.TaskParents.Composites;
using Adnc.FluidBT.Tasks;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace Adnc.FluidBT.Trees.Testing {
    public class TreeTest {
        private GameObject _gameObject;
        private Tree _tree;
        
        [SetUp]
        public void SetBehaviorTree () {
            _gameObject = new GameObject("BT");
            _tree = new Tree(_gameObject);
        }

        [TearDown]
        public void AfterEach () {
            Object.DestroyImmediate(_gameObject);
        }

        public class RootSetup : TreeTest {
            [Test]
            public void It_should_set_the_root_node_Owner_by_default () {
                Assert.AreEqual(_gameObject, _tree.Root.Owner);
            }
        }

        public class AddNodeMethod : TreeTest {
            [Test]
            public void Parent_adds_the_child_reference () {
                var action = Substitute.For<ITask>();
                action.Enabled.Returns(true);
                
                _tree.AddNode(_tree.Root, action);
                
                Assert.AreEqual(action, _tree.Root.Children[0]);
            }

            [Test]
            public void Set_the_child_nodes_GameObject_reference () {
                var action = Substitute.For<ITask>();
                action.Enabled.Returns(true);
                
                _tree.AddNode(_tree.Root, action);

                _tree.Root.Children[0].Received(1).Owner = _gameObject;
            }
        }

        public class TickMethod : TreeTest {
            private ITask _action;

            [SetUp]
            public void ConfigureAction () {
                _action = Substitute.For<ITask>();
                _action.Enabled.Returns(true);
            }

            public class RootWithSingleNode : TickMethod {
                [SetUp]
                public void AddNode () {
                    _tree.AddNode(_tree.Root, _action);
                }
                
                [Test]
                public void Update_the_first_child_task_on_update () {
                    _tree.Tick();

                    _action.Received(1).Update();
                }

                [Test]
                public void Does_not_run_reset_on_2nd_run_if_continue_was_returned () {
                    _action.Update().Returns(TaskStatus.Continue);

                    _tree.Tick();
                    _tree.Tick();

                    _action.Received(0).Reset();
                }
                
                [Test]
                public void Update_reruns_node_on_status_failure () {
                    _action.Update().Returns(TaskStatus.Failure);

                    _tree.Tick();
                    _tree.Tick();

                    _action.Received(2).Update();
                }
                
                [Test]
                public void Update_reruns_node_on_status_success () {
                    _action.Update().Returns(TaskStatus.Failure);

                    _tree.Tick();
                    _tree.Tick();

                    _action.Received(2).Update();
                }
            }

            public class RootWithSequenceTasks : TickMethod {
                private ITask _actionAlt;
                private Sequence _sequence;

                [SetUp]
                public void AddNode () {
                    _sequence = new Sequence();
                    
                    _actionAlt = Substitute.For<ITask>();
                    _actionAlt.Enabled.Returns(true);
                    
                    _tree.AddNode(_tree.Root, _sequence);
                    _tree.AddNode(_sequence, _action);
                    _tree.AddNode(_sequence, _actionAlt);
                }
                
                [Test]
                public void Runs_all_actions () {
                    _tree.Tick();

                    _sequence.Children.ForEach((child) => child.Received(1).Update());
                }

                [Test]
                public void Runs_all_actions_again_on_2nd_pass () {
                    _tree.Tick();
                    _tree.Tick();

                    _sequence.Children.ForEach((child) => child.Received(2).Update());
                }
                
                [Test]
                public void Does_not_run_reset_on_2nd_action_if_1st_fails () {
                    _action.Update().Returns(TaskStatus.Failure);
                    
                    _tree.Tick();
                    _tree.Tick();
                    
                    _actionAlt.Received(0).Reset();
                }
            }
        }
    }
}