using CleverCrow.Fluid.BTs.Tasks;
using Random = System.Random;

namespace CleverCrow.Fluid.BTs.TaskParents.Composites {
    /// <summary>
    /// Randomly selects a child node with a shuffle algorithm
    /// </summary>
    public class SelectorRandom : CompositeBase {
        private bool _init;
        
        public override string IconPath { get; } = $"{PACKAGE_ROOT}/LinearScale.png";

        protected override TaskStatus OnUpdate () {
            if (!_init) {
                ShuffleChildren();
                _init = true;
            }
            
            for (var i = ChildIndex; i < Children.Count; i++) {
                var child = Children[ChildIndex];

                switch (child.Update()) {
                    case TaskStatus.Success:
                        return TaskStatus.Success;
                    case TaskStatus.Continue:
                        return TaskStatus.Continue;
                }

                ChildIndex++;
            }

            return TaskStatus.Failure;
        }

        public override void ResetTask () {
            base.ResetTask();
                        
            ShuffleChildren();
        }

        private void ShuffleChildren () {
            var rng = new Random();
            var n = Children.Count;  
            while (n > 1) {  
                n--;  
                var k = rng.Next(n + 1);  
                var value = Children[k];  
                Children[k] = Children[n];  
                Children[n] = value;  
            }
        }
    }
}
