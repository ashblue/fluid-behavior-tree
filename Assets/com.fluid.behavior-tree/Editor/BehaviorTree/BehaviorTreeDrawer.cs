using UnityEditor;
using UnityEngine;

namespace CleverCrow.Fluid.BTs.Trees.Editors
{
    [CustomPropertyDrawer(typeof(BehaviorTree))]
    public class BehaviorTreeDrawer : PropertyDrawer
    {
        private const string ViewTreeButtonText = "View Tree";

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            GUI.enabled = Application.isPlaying;

            if (GUI.Button(position, ViewTreeButtonText))
            {
                var tree = fieldInfo.GetValue(property.serializedObject.targetObject) as IBehaviorTree;
                BehaviorTreeWindow.ShowTree(tree, tree?.Name ?? property.displayName);
            }

            GUI.enabled = true;

            EditorGUI.EndProperty();
        }
    }
}
