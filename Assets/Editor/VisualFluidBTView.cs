using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using System.Linq;

public class VisualFluidBTView : GraphView
{
    public new class UxmlFactory : UxmlFactory<VisualFluidBTView, GraphView.UxmlTraits> {}

    TestStructure testStructure;

    public VisualFluidBTView()
    {
        Insert(0, new GridBackground());

        this.AddManipulator(new ContentZoomer());
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/VisualFluidBT.uss");
        styleSheets.Add(styleSheet);
    }

    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        // base.BuildContextualMenu(evt);

        var type = typeof(TestStructure);
        evt.menu.AppendAction("Create node", (a) => CreateNode(type));
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        return ports.ToList().Where(endPort =>
                                    endPort.direction != startPort.direction &&
                                    endPort.node != startPort.node).ToList();
    }

    void CreateNode(System.Type type)
    {
        TestStructure node = testStructure.CreateNode(type);
        CreateNodeView(node);
    }

    NodeView FindNodeView(TestStructure node)
    {
        return GetNodeByGuid(node.guid) as NodeView;
    }

    public void PopulateView(TestStructure testStructure)
    {
        this.testStructure = testStructure;

        graphViewChanged -= OnGraphViewChanged;
        DeleteElements(graphElements);
        graphViewChanged += OnGraphViewChanged;

        testStructure.nodes.ForEach(n => CreateNodeView(n));

        testStructure.nodes.ForEach(n => {
            var children = testStructure.GetChildren(n);
            children.ForEach(c => {
                NodeView parentView = FindNodeView(n);
                NodeView childView = FindNodeView(c);

                Edge edge = parentView.outputPort.ConnectTo(childView.inputPort);
                AddElement(edge);
            });
        });
    }

    GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
    {
        if (graphViewChange.elementsToRemove != null)
        {
            graphViewChange.elementsToRemove.ForEach(elem => {
                NodeView nodeView = elem as NodeView;
                if (nodeView != null)
                {
                    testStructure.DeleteNode(nodeView.node);
                }

                Edge edge = elem as Edge;
                if (edge != null)
                {
                    NodeView parentView = edge.output.node as NodeView;
                    NodeView childView = edge.input.node as NodeView;
                    testStructure.RemoveChild(parentView.node, childView.node);
                }
            });
        }

        if (graphViewChange.edgesToCreate != null)
        {
            graphViewChange.edgesToCreate.ForEach(edge => {
                NodeView parentView = edge.output.node as NodeView;
                NodeView childView = edge.input.node as NodeView;
                testStructure.AddChild(parentView.node, childView.node);
            });
        }

        return graphViewChange;
    }
    
    void CreateNodeView(TestStructure node)
    {
        NodeView nodeView = new NodeView(node);
        AddElement(nodeView);
    }
}
