namespace FluidBehaviorTree.Scripts.Nodes {
    public interface INodeChild {
        INodeUpdate child { get; }
    }
}