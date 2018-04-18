namespace FluidBehaviorTree.Scripts.Nodes {
    public interface INodeChild {
        INodeUpdate Child { get; set; }
    }
}