namespace CleverCrow.Fluid.BTs.Trees.Editors {
    public class NodeFaders {
        public ColorFader BackgroundFader { get; } = BehaviorTreeDisplayTheme.GetBackgroundFader();

        public ColorFader TextFader { get; } = BehaviorTreeDisplayTheme.GetTextFader();

        public ColorFader MainIconFader { get; } = BehaviorTreeDisplayTheme.GetMainIconFader();

        public void Update (bool active) {
            BackgroundFader.Update(active);
            TextFader.Update(active);
            MainIconFader.Update(active);
        }
    }
}
