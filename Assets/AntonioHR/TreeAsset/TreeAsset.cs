using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace AntonioHR.TreeAsset
{
    public class TreeAsset: ScriptableObject
    {
        [SerializeField]
        NodeTreeHierarchy hierarchy;

        public TreeNode Root
        {
            get
            { 
                return hierarchy.Root; 
            }
        }

        private void Init()
        {
            TreeNode rootNode = ScriptableObject.CreateInstance<TreeNode>();
            rootNode.name = "Root";
            AssetDatabase.AddObjectToAsset(rootNode, this);
            hierarchy.Init(rootNode);
        }
        public TreeNode CreateNodeFloating(string name = "Node")
        {
            TreeNode newNode = ScriptableObject.CreateInstance<TreeNode>();
            newNode.name = name;
            AssetDatabase.AddObjectToAsset(newNode, this);
            hierarchy.AddFloating(newNode);
            return newNode;
        }
        public TreeNode CreateChildFor(TreeNode parent, string name = "Node")
        {
            TreeNode newNode = ScriptableObject.CreateInstance<TreeNode>();
            newNode.name = name;
            AssetDatabase.AddObjectToAsset(newNode, this);
            hierarchy.AddFloating(newNode);
            hierarchy.ChangeParentOfTo(newNode, parent);
            return newNode;
        }

        public void DeleteNodeAndAllChildren(TreeNode node)
        {
            var allRemoved = hierarchy.RemoveSelfAndChildren(node);

            foreach (var removed in allRemoved)
            {
                GameObject.DestroyImmediate(node);
            }
        }



        #region Debug
        public void CreateDebugGameObjectHierarchy()
        {
            var obj = new GameObject("Debug Hierarchy");

            var floatersObj = new GameObject("Floaters");
            floatersObj.transform.parent = obj.transform;
            foreach (var floater in hierarchy.GetAllFloating())
            {
                CreateDebugGameObjectHierachyRecursion(floatersObj, floater);
            }

            var mainHierarchy = new GameObject("Main");
            mainHierarchy.transform.parent = obj.transform;

            CreateDebugGameObjectHierachyRecursion(mainHierarchy, hierarchy.Root);
        }
        private void CreateDebugGameObjectHierachyRecursion(GameObject parent, TreeNode node)
        {
            var obj = new GameObject(string.Format("Node: {0}", node.name));
            obj.transform.parent = parent.transform;
            foreach (var child in hierarchy.GetChildrenOf(node))
            {
                CreateDebugGameObjectHierachyRecursion(obj, child);
            }
        }
        
        [MenuItem("Assets/Create/Example Tree")]
        public static void CreateExampleTree()
        {
            TreeAsset tree = ScriptableObject.CreateInstance<TreeAsset>();
            AssetDatabase.CreateAsset(tree, "Assets/NewTree.asset");

            AssetDatabase.SaveAssets();
            tree.Init();

            var compo = tree.CreateChildFor(tree.Root, "Sequence");

            var m1 = tree.CreateChildFor(compo, "Music 1");
            var deco = tree.CreateChildFor(compo, "Decorator");
            var m2 = tree.CreateChildFor(deco, "Music 2");


            var m3 = tree.CreateChildFor(tree.Root, "Music 3");

            tree.CreateDebugGameObjectHierarchy();
        }
        #endregion
    }
}
