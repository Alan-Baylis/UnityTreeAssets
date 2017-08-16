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
        SerializableTreeHierarchy<TreeNode> hierarchy;

        public TreeNode CreateNodeFloating()
        {
            TreeNode newNode = ScriptableObject.CreateInstance<TreeNode>();
            AssetDatabase.AddObjectToAsset(newNode, this);
            hierarchy.AddFloating(newNode);
            return newNode;
        }
        public TreeNode CreateChildFor(TreeNode parent)
        {
            TreeNode newNode = ScriptableObject.CreateInstance<TreeNode>();
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
                CreateDebugGameObjectHierachyRecursion(obj, node);
            }
        }
        #endregion
    }
}
