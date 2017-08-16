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
    }
}
