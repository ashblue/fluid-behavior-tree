using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using CleverCrow.Fluid.BTs.Trees;
using System.Collections.Generic;
using CleverCrow.Fluid.BTs.Tasks;

public class VisualFluidBT : EditorWindow
{
    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;

    VisualFluidBTView visualFuildBTView;
    InspectorView inspectorView;

    [MenuItem("Window/VisualFluidBT")]
    public static void OpenWindow()
    {
        VisualFluidBT wnd = GetWindow<VisualFluidBT>();
        wnd.titleContent = new GUIContent("VisualFluidBT");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // Instantiate UXML
        VisualElement labelFromUXML = m_VisualTreeAsset.Instantiate();
        root.Add(labelFromUXML);

        visualFuildBTView = root.Q<VisualFluidBTView>();
        inspectorView =  root.Q<InspectorView>();

        OnSelectionChange();
    }

    private void OnSelectionChange()
    {
        BehaviorTree tree = Selection.activeObject as BehaviorTree;

        if (tree)
        {
            visualFuildBTView.PopulateView(tree);
            tree.CreateRootNode();
        }
    }
}
