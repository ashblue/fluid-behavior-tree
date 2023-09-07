using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;

public class NodeView : UnityEditor.Experimental.GraphView.Node
{
    public TestStructure node;
    public Port inputPort;
    public Port outputPort;

    public NodeView(TestStructure node)
    {
        this.node = node;
        this.title = node.name;
        this.viewDataKey = node.guid;

        style.left = node.position.x;
        style.top = node.position.y;

        CreateInputPorts();
        CreateOutputPorts();
    }

    void CreateInputPorts()
    {
        inputPort = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool));

        if (inputPort != null)
        {
            inputPort.portName = "";
            inputContainer.Add(inputPort);
        }
    }

    void CreateOutputPorts()
    {
        outputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(bool));

        if (outputPort != null)
        {
            outputPort.portName = "";
            outputContainer.Add(outputPort);
        }
    }

    public override void SetPosition(Rect newPos)
    {
        base.SetPosition(newPos);
        node.position.x = newPos.xMin;
        node.position.y = newPos.yMin;
    }
}
