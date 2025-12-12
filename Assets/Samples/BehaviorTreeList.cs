using CleverCrow.Fluid.BTs.Trees;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CleverCrow.Fluid.BTs.Samples
{
    public class BehaviorTreeList : MonoBehaviour
    {
        protected void Awake()
        {
            for (int i = 0; i < 3; i++)
            {
                trees.Add(new NestedTree()
                {
                    tree = new BehaviorTreeBuilder(gameObject).Name($"Tree{i}").Sequence()
                        .WaitTime(1)
                        .ReturnSuccess()
                    .End().Build()
                });
            }
        }
        public List<NestedTree> trees = new List<NestedTree>();
    }
    [Serializable]
    public class NestedTree
    {
        public BehaviorTree tree;
    }
}
