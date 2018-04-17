using Adnc.FluidBT.Trees;
using NUnit.Framework;

namespace Adnc.FluidBT.Testing {
    public class BehaviorTreeTests {
        [Test]
        public void It_should_initialize () {
            var tree = new BehaviorTree();

            Assert.IsNotNull(tree);
        }

        [Test]
        public void It_should_set_the_root_node_as_current_by_default () {
            var tree = new BehaviorTree();

            Assert.AreEqual(tree.root, tree.current);
        }
    }
}