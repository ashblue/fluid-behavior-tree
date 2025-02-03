using CleverCrow.Fluid.BTs.TaskParents.Composites;
using CleverCrow.Fluid.BTs.Tasks;
using NUnit.Framework;

namespace CleverCrow.Fluid.BTs.Trees.Testing {
    public static class BehaviorTreeExtensionCompositeExamples {
        public static BehaviorTreeBuilder CustomSequence (this BehaviorTreeBuilder builder, string name) {
            return builder.ParentTask<Sequence>(name);
        }
    }
    
    public class BehaviorTreeExtensionCompositesTest {
        [Test]
        public void It_should_run_the_custom_action () {
            var result = false;
            var tree = new BehaviorTreeBuilder(null)
                .CustomSequence("test")
                    .Do(() => {
                        result = true;
                        return TaskStatus.Success;
                    })
                .End()
                .Build();

            tree.Tick();
            
            Assert.IsTrue(result);
        }
    }
}
