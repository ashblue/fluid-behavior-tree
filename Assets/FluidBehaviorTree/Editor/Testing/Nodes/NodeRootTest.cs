using System.Collections;
using System.Collections.Generic;
using FluidBehaviorTree.Scripts.Nodes;
using NUnit.Framework;
using UnityEngine;

namespace Adnc.FluidBT.Testing {
    public class NodeRootTest {
        [Test]
        public void It_should_initialize () {
            var root = new NodeRoot();

            Assert.IsNotNull(root);
        }

        public class UpdateMethod {
            [Test]
            public void It_should_call_update_on_a_single_node () {
                var action = new NodeAction();
                var root = new NodeRoot { child = action };

                root.Update();

                // @TODO Assert Update was called via testing library
            }
        }
    }
}
