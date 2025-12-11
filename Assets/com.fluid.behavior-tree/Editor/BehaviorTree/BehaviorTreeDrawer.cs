using System;
using System.Collections;
using System.Reflection;
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
                object value = GetTargetObjectOfProperty(property);
                if (value is IBehaviorTree tree)
                {
                    BehaviorTreeWindow.ShowTree(tree, tree.Name ?? property.displayName);
                }
                else
                {
                    Debug.LogWarning($"BehaviorTreeDrawer: cannot resolve runtime value for property '{property.propertyPath}'.");
                }
            }
            GUI.enabled = true;

            EditorGUI.EndProperty();
        }
        public object GetTargetObjectOfProperty(SerializedProperty prop)
        {
            if (prop == null) return null;

            object obj = prop.serializedObject.targetObject;
            if (obj == null) return null;

            string path = prop.propertyPath.Replace(".Array.data[", "[");
            string[] elements = path.Split('.');

            foreach (string element in elements)
            {
                if (element.Contains("["))
                {
                    int indexStart = element.IndexOf("[", StringComparison.Ordinal);
                    string memberName = element.Substring(0, indexStart);
                    string indexStr = element.Substring(indexStart)
                                             .Trim('[', ']');

                    obj = GetMemberValue(obj, memberName);

                    if (obj is IList list && int.TryParse(indexStr, out int index) && index >= 0 && index < list.Count)
                    {
                        obj = list[index];
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    obj = GetMemberValue(obj, element);
                }

                if (obj == null)
                    return null;
            }

            return obj;
        }

        private object GetMemberValue(object source, string name)
        {
            if (source == null)
                return null;
            var type = source.GetType();

            const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

            FieldInfo field = type.GetField(name, flags);
            if (field != null)
                return field.GetValue(source);

            return null;
        }
    }
}
