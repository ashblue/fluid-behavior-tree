namespace CleverCrow.Fluid.BTs.Tasks {
    public abstract class GenericTaskBase {
        private EditorRuntimeUtilities _editorUtils;

        public EditorRuntimeUtilities EditorUtils => 
            _editorUtils ?? (_editorUtils = new EditorRuntimeUtilities());
    }
}