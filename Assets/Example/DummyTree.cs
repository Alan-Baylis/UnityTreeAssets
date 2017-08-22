using AntonioHR;
using AntonioHR.TreeAsset;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

public class DummyTree : TreeAsset<DummyTreeNode>
{

    protected override DummyTreeNode InstantiateRoot()
    {
        var result = ScriptableObject.CreateInstance<DummyTreeNode>();
        result.name = "Dummy Root";
        return result;
    }

    protected override DummyTreeNode InstantiateDefaultNode()
    {
        var result = ScriptableObject.CreateInstance<DummyTreeNode>();
        result.name = "Dummy Node";
        return result;
    }

    #region Debug
    public void CreateDebugGameObjectHierarchy()
    {
        var obj = new GameObject("Debug Hierarchy");

        var mainHierarchy = obj.CreateChild("Main");

        CreateDebugGameObjectHierarchyRecursion(mainHierarchy, Root);

        foreach (var floater in FloatingNodes)
        {
            CreateDebugGameObjectHierarchyRecursion(obj, floater);
        }

    }
    private void CreateDebugGameObjectHierarchyRecursion(GameObject parent, DummyTreeNode node)
    {
        var obj = parent.CreateChild(string.Format("Node: {0} Data: {1}", node.name, node.dummyData));

        foreach (var child in node.GetChildrenAs<DummyTreeNode>())
        {
            CreateDebugGameObjectHierarchyRecursion(obj, child);
        }
    }

    [MenuItem("Assets/Create/Example Tree")]
    public static void CreateExampleTree()
    {
        var tree = ScriptableObject.CreateInstance<DummyTree>();
        AssetDatabase.CreateAsset(tree, "Assets/NewTree.asset");

        AssetDatabase.SaveAssets();
        tree.Init();

        var compo = tree.CreateChildFor(tree.Root, "Sequence");

        var m1 = tree.CreateChildFor(compo, "Music 1");
        m1.dummyData = 1;

        var deco = tree.CreateChildFor(compo, "Decorator");
        var m2 = tree.CreateChildFor(deco, "Music 2");
        m2.dummyData = 42;

        var m3 = tree.CreateChildFor(tree.Root, "Music 3");
        m3.dummyData = 999;

        var m4 = tree.CreateNodeFloating("Music 4");
        m4.dummyData = 314;

        tree.CreateDebugGameObjectHierarchy();
    }
    #endregion
}
