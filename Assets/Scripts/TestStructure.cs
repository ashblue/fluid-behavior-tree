using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "ScriptableObjects/SpawnTestStructure")]
public class TestStructure : ScriptableObject
{
    [SerializeField] float importantField;
    [SerializeField] string importantString;
    [SerializeField] int[] importantArray;

    public List<TestStructure> nodes = new List<TestStructure>();
    public List<TestStructure> children = new List<TestStructure>();

    public string guid;
    public Vector2 position;

    public TestStructure CreateNode(System.Type type)
    {
        TestStructure node = ScriptableObject.CreateInstance(type) as TestStructure;
        node.name = type.Name;
        node.guid = GUID.Generate().ToString();

        nodes.Add(node);

        AssetDatabase.AddObjectToAsset(node, this);
        AssetDatabase.SaveAssets();

        return node;
    }

    public void DeleteNode(TestStructure node)
    {
        nodes.Remove(node);
        AssetDatabase.RemoveObjectFromAsset(node);
        AssetDatabase.SaveAssets();
    }

    public void AddChild(TestStructure parent, TestStructure child)
    {
        parent.children.Add(child);
    }

    public void RemoveChild(TestStructure parent, TestStructure child)
    {
        parent.children.Remove(child);
    }

    public List<TestStructure> GetChildren(TestStructure parent)
    {
        return parent.children;
    }
}
