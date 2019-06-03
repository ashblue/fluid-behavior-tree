using System.Collections.Generic;
using CleverCrow.Fluid.BTs.TaskParents;
using CleverCrow.Fluid.BTs.Tasks;
using UnityEngine;
using EditorGUILayout = UnityEditor.EditorGUILayout;

namespace CleverCrow.Fluid.BTs.Trees.Editors {
    public class PrintNode {
        private readonly ITask _task;
        private readonly List<PrintNode> _children = new List<PrintNode>();
        
        public PrintNode (ITask task) {
            _task = task;

            var parent = task as ITaskParent;
            if (parent == null) return;
            foreach (var child in parent.Children) {
                _children.Add(new PrintNode(child));
            }
        }

        public void Print () {
            EditorGUILayout.BeginVertical();
            
            var centeredStyle = GUI.skin.GetStyle("Label");
            centeredStyle.alignment = TextAnchor.MiddleCenter;
            EditorGUILayout.LabelField(_task.Name, centeredStyle);

            EditorGUILayout.BeginHorizontal();
            foreach (var child in _children) {
                child.Print();
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.EndVertical();

        }
    }
}
