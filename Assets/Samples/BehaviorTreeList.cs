using CleverCrow.Fluid.BTs.Trees;
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
                trees.Add(new BehaviorTreeBuilder(gameObject).Name($"Tree{i}").Sequence()
                    .WaitTime(1)
                    .ReturnSuccess()
                .End().Build());
            }
        }
        public List<BehaviorTree> trees = new List<BehaviorTree>();
    }
}
