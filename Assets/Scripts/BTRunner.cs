using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CleverCrow.Fluid.BTs.Trees;

public class BTRunner : MonoBehaviour
{
    [SerializeField] BehaviorTree tree;

    void Start()
    {
        tree.isRunning = true;
    }

    void Update()
    {
        tree.Tick();
    }
}
