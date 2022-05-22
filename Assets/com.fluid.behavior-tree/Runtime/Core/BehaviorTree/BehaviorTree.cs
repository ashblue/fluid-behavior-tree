using System;
using System.Collections.Generic;
using CleverCrow.Fluid.BTs.TaskParents;
using CleverCrow.Fluid.BTs.Tasks;
#if UNITY_2021_3_OR_NEWER
using UnityEngine;
#endif

namespace CleverCrow.Fluid.BTs.Trees
{
    [Serializable]
    public class BehaviorTree : IBehaviorTree
    {
        public IReadOnlyList<ITask> ActiveTasks => _tasks;

        public int TickCount { get; private set; }

        public string Name { get; set; }
        public TaskRoot Root { get; } = new();

        private readonly List<ITask> _tasks = new(); //TODO: Starting size can reduce allocations

#if UNITY_2021_3_OR_NEWER
        private readonly GameObject _owner;

        public BehaviorTree(GameObject owner)
        {
            _owner = owner;
            SyncNodes(Root);
        }
#else
        public BehaviorTree()
        {
            SyncNodes(Root);
        }
#endif

        public void AddActiveTask(ITask task)
        {
            _tasks.Add(task);
        }

        public void RemoveActiveTask(ITask task)
        {
            _tasks.Remove(task);
        }

        public TaskStatus Tick()
        {
            var status = Root.Update();

            if (status != TaskStatus.Continue)
            {
                Reset();
            }

            return status;
        }

        public void Reset()
        {
            foreach (var task in _tasks)
            {
                task.End();
            }

            _tasks.Clear();
            TickCount++;
        }

        public void AddNode(ITaskParent parent, ITask child)
        {
            parent.AddChild(child);
            child.ParentTree = this;

#if UNITY_2021_3_OR_NEWER
            child.Owner = _owner;
#endif
        }

        public void Splice(ITaskParent parent, BehaviorTree tree)
        {
            parent.AddChild(tree.Root);

            SyncNodes(tree.Root);
        }

        private void SyncNodes(ITaskParent taskParent)
        {
#if UNITY_2021_3_OR_NEWER
            taskParent.Owner = _owner;
#endif
            taskParent.ParentTree = this;

            foreach (var child in taskParent.Children)
            {
#if UNITY_2021_3_OR_NEWER
                child.Owner = _owner;
#endif
                child.ParentTree = this;

                if (child is ITaskParent parent)
                {
                    SyncNodes(parent);
                }
            }
        }
    }
}
