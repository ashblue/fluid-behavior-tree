using UnityEditor;
using UnityEngine;

namespace CleverCrow.Fluid.BTs.Trees.Editors
{
    public class BehaviorTreeWindow : EditorWindow
    {
        private const string TitleFormat = "Behavior Tree: {0}";

        private BehaviorTreePrinter _printer;
        private string _name;
        private string _titleText;

        public static void ShowTree(IBehaviorTree tree, string treeName)
        {
            var window = GetWindow<BehaviorTreeWindow>(false);

            var titleText = CreateTitleText(treeName);

            window.titleContent = new GUIContent(titleText);
            window.SetTree(tree, treeName, titleText);
        }

        private static string CreateTitleText(string treeName) => string.Format(TitleFormat, treeName);

        private void SetTree(IBehaviorTree tree, string treeName, string windowTitleText)
        {
            _printer?.Unbind();
            _printer = new BehaviorTreePrinter(tree, position.size);
            _name = treeName;
            _titleText = windowTitleText;
        }

        private void OnGUI()
        {
            if (!Application.isPlaying)
            {
                ClearView();
            }

            GUILayout.Label(_titleText, EditorStyles.boldLabel);
            _printer?.Print(position.size);
        }

        private void Update()
        {
            if (Application.isPlaying)
            {
                Repaint();
            }
        }

        private void ClearView()
        {
            _name = null;
            _printer = null;
        }
    }
}
