using System;
using Adnc.FluidBT.Tasks;
using Adnc.FluidBT.Trees;
using NSubstitute;
using NUnit.Framework;

namespace Adnc.FluidBT.Testing {
    public class BehaviorTreeTests {
        BehaviorTree tree;

        public interface INodeAwakeExample : ITask, IEventAwake {
        }

        [SetUp]
        public void SetBehaviorTree () {
            tree = new BehaviorTree();
        }

        public class OnInit : BehaviorTreeTests {
            [Test]
            public void It_should_initialize () {
                Assert.IsNotNull(tree);
            }

            [Test]
            public void It_should_set_the_root_node_as_current_by_default () {
                Assert.AreEqual(tree.Root, tree.Current);
            }
        }

        public class AwakeEvent : BehaviorTreeTests {
            [Test]
            public void Trigger_on_all_nodes_with_the_awake_interface () {
                var node = Substitute.For<INodeAwakeExample>();
                node.Enabled.Returns(true);

                tree.AddNode(tree.Root, node);
                tree.Setup();

                node.Received().Awake();
            }

            [Test]
            public void Calling_setup_again_should_not_refire_awake () {
                var node = Substitute.For<INodeAwakeExample>();
                node.Enabled.Returns(true);

                tree.AddNode(tree.Root, node);
                tree.Setup();
                tree.Setup();

                node.Received(1).Awake();
            }
        }

        public class AddNodeMethod : BehaviorTreeTests {
            private ITask action;

            [SetUp]
            public void SetDefaultAction () {
                action = Substitute.For<INodeAwakeExample>();
                action.Enabled.Returns(true);
            }

            public class AddNodeMethodSuccess : AddNodeMethod {
                [SetUp]
                public void AddNode () {
                    tree.AddNode(tree.Root, action);
                }

                [Test]
                public void Parent_node_is_added_to_the_child () {
                    Assert.AreEqual(action, tree.Root.children[0]);
                }

                [Test]
                public void Add_child_node_to_node_list () {
                    Assert.Contains(action, tree.nodeAwake);
                }

                [Test]
                public void Add_child_node_to_nodes () {
                    Assert.IsTrue(tree.nodes.Contains(action));
                }

                [Test]
                public void Attaches_a_reference_to_the_behavior_tree () {
                    action.Received().Owner = tree;
                }
            }

            public class AddNodeMethodError : AddNodeMethod {
                [Test]
                public void Error_if_parent_is_null () {
                    Assert.Throws<ArgumentNullException>(
                        () => tree.AddNode(null, action),
                        "Parent cannot be null");
                }

                [Test]
                public void Error_if_child_is_null () {
                    Assert.Throws<ArgumentNullException>(
                        () => tree.AddNode(tree.Root, null),
                        "Child cannot be null");
                }
            }
        }

        public class TickMethod : BehaviorTreeTests {
            private ITask action;

            [SetUp]
            public void SetDefaultAction () {
                action = A.TaskStub().Build();

                tree.Root.AddChild(action);
            }

            [Test]
            public void Update_the_first_child_task_on_update () {
                tree.Update();

                action.Received().Update();
            }

            [Test]
            public void Update_sets_root_as_current_on_action_status_success () {
                tree.Update();

                Assert.AreEqual(tree.Current, tree.Root);
            }

            [Test]
            public void Update_sets_root_as_current_on_status_failure () {
                action.Update().Returns(TaskStatus.Failure);

                tree.Update();

                Assert.AreEqual(tree.Current, tree.Root);
            }

            [Test]
            public void Update_sets_root_as_current_on_status_continue () {
                action.Update().Returns(TaskStatus.Continue);

                tree.Update();

                Assert.AreEqual(tree.Current, tree.Root);
            }
        }
    }
}