using Adnc.FluidBT.TaskParents.Composites;
using Adnc.FluidBT.Tasks;
using NUnit.Framework;

namespace Adnc.FluidBT.Trees.Testing {
    public static class BehaviorTreeExtensionCompositeExamples {
        public static BehaviorTreeBuilder CustomSequence (this BehaviorTreeBuilder builder, string name) {
            return builder.ParentTask<Sequence>(name);
        }
    }
    
    public class BehaviorTreeExtensionCompositesTest {
        public class ExtensionComposite : CompositeBase {
        }
        
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
