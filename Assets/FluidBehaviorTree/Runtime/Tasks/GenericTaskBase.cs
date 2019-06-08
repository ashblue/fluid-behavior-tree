namespace CleverCrow.Fluid.BTs.Tasks {
    public abstract class GenericTaskBase {
        private EditorRuntimeUtilities _editorUtils;
        protected const string ICON_TASK_PATH = "Assets/FluidBehaviorTree/Editor/Icons/Tasks";

        public virtual string IconPath => $"{ICON_TASK_PATH}/Play.png";
        public virtual float IconPadding { get; }
        
        public bool HasBeenActive { get; private set; }

        public EditorRuntimeUtilities EditorUtils => 
            _editorUtils ?? (_editorUtils = new EditorRuntimeUtilities());

        public virtual TaskStatus Update () {
#if UNITY_EDITOR
            EditorUtils.EventActive.Invoke();
#endif
            
            HasBeenActive = true;

            return TaskStatus.Success;
        }
    }
}
