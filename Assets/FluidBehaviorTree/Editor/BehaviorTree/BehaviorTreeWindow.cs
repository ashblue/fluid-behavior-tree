using UnityEditor;
using UnityEngine;

namespace CleverCrow.Fluid.BTs.Trees.Editors {
    public class BehaviorTreeWindow : EditorWindow {
        private BehaviorTreePrinter _printer;
        private string _name;

        public static void ShowTree (IBehaviorTree tree, string name) {
            var window = GetWindow(typeof(BehaviorTreeWindow)) as BehaviorTreeWindow;
            window.SetTree(tree, name);
        }

        private void SetTree (IBehaviorTree tree, string name) {
            _printer = new BehaviorTreePrinter(tree, position.size);
            _name = name;
        }

        private void OnGUI () {
            GUILayout.Label($"Behavior Tree: {_name}", EditorStyles.boldLabel);
            _printer?.Print(position.size);
        }
    }
}
