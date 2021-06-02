using UnityEditor;
using UnityEngine;

namespace CleverCrow.Fluid.BTs.Trees.Editors {
    public class BehaviorTreeWindow : EditorWindow {
        private BehaviorTreePrinter _printer;
        private string _name;

        public static void ShowTree (IBehaviorTree tree, string name) {
            var window = GetWindow<BehaviorTreeWindow>(false);
            window.titleContent = new GUIContent($"Behavior Tree: {name}");
            window.SetTree(tree, name);
        }

        private void SetTree (IBehaviorTree tree, string name) {
            _printer?.Unbind();
            _printer = new BehaviorTreePrinter(tree, position.size);
            _name = name;
        }

        private void OnGUI () {
            if (!Application.isPlaying) {
                ClearView();
            }
            
            GUILayout.Label($"Behavior Tree: {_name}", EditorStyles.boldLabel);
            _printer?.Print(position.size);
        }

        private void ClearView () {
            _name = null;
            _printer = null;
        }

        private void Update () {
            if (Application.isPlaying) {
                Repaint();
            }
        }
    }
}
