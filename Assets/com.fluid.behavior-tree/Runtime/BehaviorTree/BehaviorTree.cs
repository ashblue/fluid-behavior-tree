using System.Collections.Generic;
using CleverCrow.Fluid.BTs.TaskParents;
using CleverCrow.Fluid.BTs.Tasks;
using UnityEngine;
using UnityEditor;

namespace CleverCrow.Fluid.BTs.Trees {
    public class IBehaviorTree : ScriptableObject{
        public string Name { get; set; }
        public TaskRoot Root { get; set; }
        public int TickCount { get; set; }
        
        public virtual void AddActiveTask (ITask task) {}
        public virtual void RemoveActiveTask (ITask task) {}
    }

    [CreateAssetMenu(menuName = "ScriptableObjects/BehaviorTree")]
    public class BehaviorTree : IBehaviorTree {
        private readonly GameObject _owner;
        [SerializeField] public List<ITask> _tasks = new List<ITask>();
        [SerializeField] public List<ITask> allNodes = new List<ITask>();

        public List<int> teste = new List<int>();

        public IReadOnlyList<ITask> ActiveTasks => _tasks;
        public bool isRunning = false;

        void OnEnable()
        {
            Debug.Log(_tasks);
        }

        public void CreateRootNode()
        {
            if (Root != null) return;

            Root = CreateNode(typeof(TaskRoot)) as TaskRoot;
        }

        public BehaviorTree (GameObject owner) {
            _owner = owner;
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

                AssetDatabase.AddObjectToAsset(node, this);
                AssetDatabase.SaveAssets();

                return node;
            }
            else if (type.BaseType.BaseType == typeof(TaskParentBase))
            {
                TaskParentBase node = ScriptableObject.CreateInstance(type) as TaskParentBase;
                node.name = type.Name;
                node.guid = GUID.Generate().ToString();
                node.Name = type.Name;

                allNodes.Add(node);

                AssetDatabase.AddObjectToAsset(node, this);
                AssetDatabase.SaveAssets();

                return node;
            }
            else if (type == typeof(TaskRoot)) // Root node
            {
                TaskRoot node = ScriptableObject.CreateInstance(type) as TaskRoot;
                node.name = type.Name;
                node.guid = GUID.Generate().ToString();
                node.Name = type.Name;

                allNodes.Add(node);

                AssetDatabase.AddObjectToAsset(node, this);
                AssetDatabase.SaveAssets();

                return node;
            }
            else // if (type.BaseType == typeof(Decorators.DecoratorBase))
            {
                Decorators.DecoratorBase node = ScriptableObject.CreateInstance(type) as Decorators.DecoratorBase;
                node.name = type.Name;
                node.guid = GUID.Generate().ToString();
                node.Name = type.Name;

                allNodes.Add(node);

                AssetDatabase.AddObjectToAsset(node, this);
                AssetDatabase.SaveAssets();

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
            if (isRunning)
            {
                var status = Root.Update();
                if (status != TaskStatus.Continue) {
                    Reset();
                }

                return status;
            }

            return TaskStatus.Success;
        }

        public void Reset () {
            if (!isRunning) return;

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
    }
}
