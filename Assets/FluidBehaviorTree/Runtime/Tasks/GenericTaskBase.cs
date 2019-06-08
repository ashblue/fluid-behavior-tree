namespace CleverCrow.Fluid.BTs.Tasks {
    public abstract class GenericTaskBase {
        private EditorRuntimeUtilities _editorUtils;
        protected const string ICON_TASK_PATH = "Assets/FluidBehaviorTree/Editor/Icons/Tasks";

        public virtual string IconPath => $"{ICON_TASK_PATH}/Play.png";
        public virtual float IconPadding { get; }

        public EditorRuntimeUtilities EditorUtils => 
            _editorUtils ?? (_editorUtils = new EditorRuntimeUtilities());
    }
}
