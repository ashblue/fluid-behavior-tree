using System.Linq;
using Adnc.FluidBT.Trees;
using FluidBehaviorTree.Scripts.Nodes;
using NSubstitute;
using NUnit.Framework;

namespace Adnc.FluidBT.Testing {
    public class BehaviorTreeTests {
        readonly BehaviorTree tree = new BehaviorTree();

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

        public class TickMethod : BehaviorTreeTests {
            [Test]
            public void Update_the_current_node_on_tick () {
                var root = Substitute.For<INodeRoot>();
                tree.Current = root;
                tree.Root = root;

                tree.Tick();

                tree.Root.Received().Update();
            }

            [Test]
            public void Tick_the_next_action_from_root () {
                var action = Substitute.For<INodeUpdate>();

                action.Update().Returns(NodeStatus.Success);
                action.Enabled.Returns(true);
                tree.Root.Child = action;

                tree.Tick();

                action.Received().Update();
            }

            [Test]
            public void Tick_sets_root_as_current_on_action_status_success () {
                var action = Substitute.For<INodeUpdate>();

                action.Update().Returns(NodeStatus.Success);
                action.Enabled.Returns(true);
                tree.Root.Child = action;

                tree.Tick();

                Assert.AreEqual(tree.Current, tree.Root);
            }

            [Test]
            public void Tick_sets_root_as_current_on_status_failure () {
                var action = Substitute.For<INodeUpdate>();

                action.Update().Returns(NodeStatus.Failure);
                action.Enabled.Returns(true);
                tree.Root.Child = action;

                tree.Tick();

                Assert.AreEqual(tree.Current, tree.Root);
            }

//            [Test]
//            public void Tick_sets_action_as_current_on_status_continue () {
//                var action = Substitute.For<INodeUpdate>();
//
//                action.Update().Returns(NodeStatus.Continue);
//                action.Enabled.Returns(true);
//                tree.Root.Child = action;
//
//                tree.Tick();
//
//                Assert.AreEqual(tree.Current, action);
//            }
        }
    }
}