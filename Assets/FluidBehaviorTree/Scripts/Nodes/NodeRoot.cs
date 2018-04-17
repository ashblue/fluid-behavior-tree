namespace FluidBehaviorTree.Scripts.Nodes {
    public class NodeRoot : INodeChild, INodeUpdate {
        public INodeUpdate child { get; set; }

        public void Update () {
            child.Update();
        }
    }
}