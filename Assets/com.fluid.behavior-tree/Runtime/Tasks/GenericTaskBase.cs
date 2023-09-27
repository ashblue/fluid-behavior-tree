namespace CleverCrow.Fluid.BTs.Tasks {
    public abstract class GenericTaskBase : ITask{
        private EditorRuntimeUtilities _editorUtils;
        protected const string PACKAGE_ROOT = "ROOT/Editor/Icons/Tasks";

        /// <summary>
        /// For custom project icons provide a path from assets to a Texture2D asset. Example `Assets/MyIcon.png`.
        /// Be sure to NOT include the keyword `ROOT` in your path name. This will be replaced with a path
        /// to the package root for Fluid Behavior Tree.
        /// </summary>
        public override string IconPath => $"{PACKAGE_ROOT}/Play.png";
        public override float IconPadding { get; }
        public override bool HasBeenActive { get; set; }

        public override EditorRuntimeUtilities EditorUtils =>
            _editorUtils ?? (_editorUtils = new EditorRuntimeUtilities());

        public override TaskStatus Update () {
#if UNITY_EDITOR
            EditorUtils.EventActive.Invoke();
#endif
            
            HasBeenActive = true;

            return TaskStatus.Success;
        }
    }
}
