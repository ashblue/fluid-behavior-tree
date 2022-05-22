using CleverCrow.Fluid.BTs.Trees.Core.Interfaces;

namespace CleverCrow.Fluid.BTs.Trees.Core.Implementations
{
    public class DefaultOwner : IOwner
    {
        public string Name { get; set; }

        public DefaultOwner(string name)
        {
            Name = name;
        }
    }
}
