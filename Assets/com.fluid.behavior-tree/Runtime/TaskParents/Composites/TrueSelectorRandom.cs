using CleverCrow.Fluid.BTs.Tasks;
using Random = System.Random;

namespace CleverCrow.Fluid.BTs.TaskParents.Composites
{
    /// <summary>
    /// New version of SelectorRandom which shuffles children everytime it comes back
    /// </summary>
    public class TrueSelectorRandom : CompositeBase
    {
        private bool _init;

        public override string IconPath { get; } = $"{PACKAGE_ROOT}/LinearScale.png";

        protected override TaskStatus OnUpdate()
        {
            if (!_init)
            {
                ShuffleChildren();
                _init = true;
            }

            for (var i = ChildIndex; i < Children.Count; i++)
            {
                var child = Children[ChildIndex];

                switch (child.Update())
                {
                    case TaskStatus.Success:
                        _init = false;
                        return TaskStatus.Success;

                    case TaskStatus.Continue:
                        _init = false;
                        return TaskStatus.Continue;
                }

                ChildIndex++;
            }

            _init = false;
            return TaskStatus.Failure;
        }

        public override void ResetTask () {
            base.ResetTask();
                        
            ShuffleChildren();
        }

        private void ShuffleChildren()
        {
            var rng = new Random();
            var n = Children.Count;
            while (n > 1)
            {
                n--;
                var k = rng.Next(n + 1);
                var value = Children[k];
                Children[k] = Children[n];
                Children[n] = value;
            }
        }
    }
}
