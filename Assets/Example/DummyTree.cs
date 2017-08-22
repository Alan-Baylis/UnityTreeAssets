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

        var floatersObj = new GameObject("Floaters");
        floatersObj.transform.parent = obj.transform;
        foreach (var floater in FloatingNodes)
        {
            CreateDebugGameObjectHierarchyRecursion(floatersObj, floater);
        }

        var mainHierarchy = new GameObject("Main");
        mainHierarchy.transform.parent = obj.transform;

        CreateDebugGameObjectHierarchyRecursion(mainHierarchy, Root);
    }
    private void CreateDebugGameObjectHierarchyRecursion(GameObject parent, TreeNodeAsset node)
    {
        var obj = new GameObject(string.Format("Node: {0}", node.name));
        obj.transform.parent = parent.transform;
        foreach (var child in node.Children)
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
        var deco = tree.CreateChildFor(compo, "Decorator");
        var m2 = tree.CreateChildFor(deco, "Music 2");


        var m3 = tree.CreateChildFor(tree.Root, "Music 3");

        var m4 = tree.CreateNodeFloating("Music 4");

        tree.CreateDebugGameObjectHierarchy();
    }
    #endregion
}
