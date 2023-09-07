using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

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
        TestStructure testStructure = Selection.activeObject as TestStructure;

        if (testStructure)
        {
            visualFuildBTView.PopulateView(testStructure);
        }
    }
}
