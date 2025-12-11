using System;
using System.Collections.Generic;
using Unity.Properties;
using UnityEditor;
using UnityEngine;

namespace CleverCrow.Fluid.BTs.Trees.Editors
{
    [CustomPropertyDrawer(typeof(BehaviorTree))]
    public class BehaviorTreeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            GUI.enabled = Application.isPlaying;
            if (GUI.Button(position, "View Tree"))
            {
                object value = fieldInfo.GetValue(property.serializedObject.targetObject);
                if (value is IBehaviorTree tree)
                {
                    BehaviorTreeWindow.ShowTree(tree, tree.Name ?? property.displayName);
                }
                else if (value is IList<BehaviorTree> list)
                {
                    if (TryGetArrayIndex(property.propertyPath, out int index) && list[index] is IBehaviorTree childTree)
                    {
                        BehaviorTreeWindow.ShowTree(childTree, childTree.Name ?? property.displayName);
                    }
                }
            }
            GUI.enabled = true;

            EditorGUI.EndProperty();
        }
        private bool TryGetArrayIndex(string path, out int index)
        {
            const string arrayData = ".Array.data[";
            int arrayIndex = path.IndexOf(arrayData, StringComparison.Ordinal);
            if (arrayIndex < 0)
            {
                index = -1;
                return false;
            }

            arrayIndex += arrayData.Length;
            int endIndex = path.IndexOf("]", arrayIndex, StringComparison.Ordinal);
            if (endIndex < 0)
            {
                index = -1;
                return false;
            }

            string indexStr = path.Substring(arrayIndex, endIndex - arrayIndex);
            if (int.TryParse(indexStr, out index))
                return true;

            index = -1;
            return false;
        }
    }
}
