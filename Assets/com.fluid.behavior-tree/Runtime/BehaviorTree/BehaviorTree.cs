using System.Collections.Generic;
using CleverCrow.Fluid.BTs.TaskParents;
using CleverCrow.Fluid.BTs.Tasks;
using UnityEngine;
using UnityEditor;

namespace CleverCrow.Fluid.BTs.Trees {
    public abstract class IBehaviorTree : ScriptableObject {
        public string Name { get; set; }

        [SerializeField] private TaskRoot _Root;
        public virtual TaskRoot Root { get => _Root;  set => _Root = value; }
        public int TickCount { get; set; }
        
        public abstract void AddActiveTask (ITask task);
        public abstract void RemoveActiveTask (ITask task);
    }

    [CreateAssetMenu(menuName = "ScriptableObjects/BehaviorTree")]
    public class BehaviorTree : IBehaviorTree {
        private GameObject _owner = null;
        [SerializeField] public List<ITask> _tasks = new List<ITask>();
        [SerializeField] public List<ITask> allNodes = new List<ITask>();
        [SerializeField] public List<ITask> nodesToSave = new List<ITask>();

        public IReadOnlyList<ITask> ActiveTasks => _tasks;

        // private void Awake()
        // {
        //     if (!hasBeenInitialized)
        //     {
        //         CreateRootNode();
        //         Debug.Log("Inicializei");
        //         hasBeenInitialized = true;
        //     }
        // }

        public void CreateRootNode()
        {
            if (Root != null) return;

            Root = CreateNode(typeof(TaskRoot)) as TaskRoot;
        }

        public void SetOwner(GameObject owner) {
            _owner = owner;
            CreateRootNode();
            SyncNodes(Root);
        }

        public ITask CreateNode(System.Type type)
        {

            if (type.BaseType.BaseType == typeof(TaskBase))
            {
                TaskBase node = ScriptableObject.CreateInstance(type) as TaskBase;
                node.name = type.Name;
                node.guid = GUID.Generate().ToString();
                node.Name = type.Name;

                allNodes.Add(node);
                nodesToSave.Add(node);

                return node;
            }
            else if (type.BaseType.BaseType == typeof(TaskParentBase))
            {
                TaskParentBase node = ScriptableObject.CreateInstance(type) as TaskParentBase;
                node.name = type.Name;
                node.guid = GUID.Generate().ToString();
                node.Name = type.Name;

                allNodes.Add(node);
                nodesToSave.Add(node);

                return node;
            }
            else if (type == typeof(TaskRoot)) // Root node
            {
                TaskRoot node = ScriptableObject.CreateInstance(type) as TaskRoot;
                node.name = type.Name;
                node.guid = GUID.Generate().ToString();
                node.Name = type.Name;

                allNodes.Add(node);
                nodesToSave.Add(node);

                return node;
            }
            else // if (type.BaseType == typeof(Decorators.DecoratorBase))
            {
                Decorators.DecoratorBase node = ScriptableObject.CreateInstance(type) as Decorators.DecoratorBase;
                node.name = type.Name;
                node.guid = GUID.Generate().ToString();
                node.Name = type.Name;

                allNodes.Add(node);
                nodesToSave.Add(node);

                return node;
            }
        }

        public List<ITask> GetChildren(ITask parent)
        {
            return parent.Children;
        }

        public void DeleteNode(ITask node)
        {
            allNodes.Remove(node);

            if (nodesToSave.Contains(node)) nodesToSave.Remove(node);

            if (node is TaskBase)
            {
                AssetDatabase.RemoveObjectFromAsset(node as TaskBase);
                AssetDatabase.SaveAssets();
            }
            else if (node is TaskParentBase)
            {
                AssetDatabase.RemoveObjectFromAsset(node as TaskParentBase);
                AssetDatabase.SaveAssets();
            }
            else // if (node is Decorators.DecoratorBase)
            {
                AssetDatabase.RemoveObjectFromAsset(node as Decorators.DecoratorBase);
                AssetDatabase.SaveAssets();
            }
        }

        public void RemoveChild(ITask parent, ITask child)
        {
            parent.Children.Remove(child);
        }

        public void AddChild(ITask parent, ITask child)
        {
            parent.Children.Add(child);
        }

        public TaskStatus Tick () {
            var status = Root.Update();
            if (status != TaskStatus.Continue) {
                ResetTree();
            }

            return status;
        }

        public void ResetTree () {
            foreach (var task in _tasks) {
                task.End();
            }
            _tasks.Clear();
            TickCount++;
        }

        public void AddNode (ITaskParent parent, ITask child) {
            parent.AddChild(child);
            child.ParentTree = this;
            child.Owner = _owner;
        }

        public void Splice (ITaskParent parent, BehaviorTree tree) {
            parent.AddChild(tree.Root);

            SyncNodes(tree.Root);
        }

        private void SyncNodes (ITaskParent taskParent) {
            taskParent.Owner = _owner;
            taskParent.ParentTree = this;
            
            foreach (var child in taskParent.Children) {
                child.Owner = _owner;
                child.ParentTree = this;

                var parent = child as ITaskParent;
                if (parent != null) {
                    SyncNodes(parent);
                }
            }
        }
        
        public override void AddActiveTask (ITask task) {
            _tasks.Add(task);
        }

        public override void RemoveActiveTask (ITask task) {
            _tasks.Remove(task);
        }

        public void SaveTree()
        {
            foreach (var node in nodesToSave)
            {
                AssetDatabase.AddObjectToAsset(node, this);
            }

            nodesToSave.Clear();
            AssetDatabase.SaveAssets();
        }
    }
}
