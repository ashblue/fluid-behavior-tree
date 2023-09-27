using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Tasks.Actions;

public class DebugLogNode : ActionBase
{
    protected override TaskStatus OnUpdate()
    {
        Debug.Log("Mensagem");
        return TaskStatus.Success;
    }
}
