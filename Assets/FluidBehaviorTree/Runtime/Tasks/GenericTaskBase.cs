namespace CleverCrow.Fluid.BTs.Tasks {
    public abstract class GenericTaskBase {
        private EditorRuntimeUtilities _editorUtils;
        protected const string _iconPath = "Assets/FluidBehaviorTree/Editor/Icons";

        public virtual string IconPath => $"{_iconPath}/Play.png";
        public virtual float IconPadding { get; }

        public EditorRuntimeUtilities EditorUtils => 
            _editorUtils ?? (_editorUtils = new EditorRuntimeUtilities());
    }
}
