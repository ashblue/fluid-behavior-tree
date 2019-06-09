namespace CleverCrow.Fluid.BTs.Tasks {
    public abstract class GenericTaskBase {
        private EditorRuntimeUtilities _editorUtils;
        protected const string PACKAGE_ROOT = "ROOT/Editor/Icons/Tasks";

        /// <summary>
        /// For custom project icons provide a path from assets to a Texture2D asset. Example `Assets/MyIcon.png`
        /// </summary>
        public virtual string IconPath => $"{PACKAGE_ROOT}/Play.png";
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
