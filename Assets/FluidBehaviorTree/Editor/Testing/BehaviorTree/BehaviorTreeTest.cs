using System;
using System.Linq;
using Adnc.FluidBT.Trees;
using FluidBehaviorTree.Scripts.Nodes;
using NSubstitute;
using NUnit.Framework;

namespace Adnc.FluidBT.Testing {
    public class BehaviorTreeTests {
        BehaviorTree tree;
        
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

        public class AddNodeMethod : BehaviorTreeTests {
            private INodeUpdate action;

            [SetUp]
            public void SetDefaultAction () {
                action = Substitute.For<INodeUpdate>();
            }

            public class AddNodeMethodSuccess : AddNodeMethod {
                [SetUp]
                public void AddNode () {
                    tree.AddNode(tree.Root, action);
                }

                [Test]
                public void Parent_node_is_added_to_the_child () {
                    Assert.AreEqual(action, tree.Root.Child);
                }

                [Test]
                public void Add_child_node_to_node_list () {
                    Assert.Contains(action, tree.nodeList);
                }

                [Test]
                public void Add_child_node_to_nodes () {
                    Assert.IsTrue(tree.nodes.Contains(action));
                }
            }

            public class AddNodeMethodError : AddNodeMethod {
                [Test]
                public void Cannot_add_child_nodes_that_have_already_been_added () {
                    var actionChild = Substitute.For<INodeRoot>();

                    tree.AddNode(tree.Root, actionChild);

                    Assert.Throws<ArgumentException>(
                        () => tree.AddNode(actionChild, actionChild),
                        "Cannot set a child node that has already been added");
                }

                [Test]
                public void Cannot_overwrite_an_already_set_child_node () {
                    var actionChild = Substitute.For<INodeRoot>();

                    tree.AddNode(tree.Root, action);

                    Assert.Throws<ArgumentException>(
                        () => tree.AddNode(tree.Root, actionChild),
                        "Cannot overwrite an already set child node");
                }

                [Test]
                public void Cannot_add_child_nodes_to_a_parent_not_in_the_bt () {
                    var actionChild = Substitute.For<INodeRoot>();

                    Assert.Throws<ArgumentException>(
                        () => tree.AddNode(actionChild, action),
                        "Cannot add a node to a parent that is not in the BT");
                }

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
            private INodeUpdate action;

            [SetUp]
            public void SetDefaultAction () {
                action = Substitute.For<INodeUpdate>();
                action.Update().Returns(NodeStatus.Success);
                action.Enabled.Returns(true);

                tree.Root.Child = action;
            }

            [Test]
            public void Update_the_current_node_on_tick () {
                var root = Substitute.For<INodeRoot>();
                tree.Current = root;

                tree.Tick();

                tree.Current.Received().Update();
            }

            [Test]
            public void Tick_the_next_action_from_root () {
                tree.Tick();

                action.Received().Update();
            }

            [Test]
            public void Tick_sets_root_as_current_on_action_status_success () {
                tree.Tick();

                Assert.AreEqual(tree.Current, tree.Root);
            }

            [Test]
            public void Tick_sets_root_as_current_on_status_failure () {
                action.Update().Returns(NodeStatus.Failure);

                tree.Tick();

                Assert.AreEqual(tree.Current, tree.Root);
            }

            [Test]
            public void Tick_sets_root_as_current_on_status_continue () {
                action.Update().Returns(NodeStatus.Continue);

                tree.Tick();

                Assert.AreEqual(tree.Current, tree.Root);
            }
        }
    }
}