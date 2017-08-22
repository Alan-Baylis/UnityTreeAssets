using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace AntonioHR.TreeAsset.Internal
{
    public static class TreeHierarchyOperations
    {

        #region Error msgs
        private const string nodeInHierarchyMsg = "Node is already in hierarchy";
        #endregion

        public static void SetupRoot(this TreeHierarchyAsset tree, TreeNodeAsset root)
        {
            Debug.Assert(tree._root == null);

            var rootHierarchyNode = CreateHierarchyNodeAndLink(tree, root);
            tree._root = rootHierarchyNode;
            rootHierarchyNode._tree = tree;
        }
        public static void AddAsFloatingNode(this TreeHierarchyAsset tree, TreeNodeAsset newNode)
        {
            Debug.Assert(newNode._hierarchy == null, nodeInHierarchyMsg);

            var hierarchyNode = CreateHierarchyNodeAndLink(tree, newNode);
            hierarchyNode._tree = tree;
            tree._floaters.Add(hierarchyNode);
        }


        #region TreeHierarchyNode
        public static void ChangeParentTo(this TreeHierarchyNodeAsset child, TreeHierarchyNodeAsset newParent)
        {
            Debug.Assert(newParent._tree == child._tree);

            if (child._parent != null)
            {
                child._parent.Unattach(child);
            }

            child._parent = newParent;
            newParent._children.Add(child);
            child._isFloating = false;
            child._tree._floaters.Remove(child);
        }

        public static void UnattachFromParent(this TreeHierarchyNodeAsset child)
        {
            child._parent.Unattach(child);
        }

        private static void Unattach(this TreeHierarchyNodeAsset self, TreeHierarchyNodeAsset child)
        {
            Debug.Assert(self._children.Contains(child));
            self._children.Remove(child);
            child._isFloating = true;
            child._tree._floaters.Add(child);
        }
        #endregion

        #region Assets

        public static void DeleteHierarchy(this TreeHierarchyNodeAsset self)
        {
            if(self._parent != null)
            {
                self.UnattachFromParent();
            }
            foreach (var node in self.BottomUp())
            {
                GameObject.DestroyImmediate(self);
            }
        }

        private static TreeHierarchyNodeAsset CreateHierarchyNodeAndLink(this TreeHierarchyAsset tree, TreeNodeAsset node)
        {
            Debug.Assert(node._hierarchy == null, nodeInHierarchyMsg);

            var hieNode = CreateHierarchyNodeAsset(tree);
            hieNode.name = string.Format("_Node ({0})", node.name);

            node._hierarchy = hieNode;
            hieNode._content = node;

            return hieNode;
        }
        private static TreeHierarchyNodeAsset CreateHierarchyNodeAsset(TreeHierarchyAsset hierarchy)
        {
            var hieNode = ScriptableObject.CreateInstance<TreeHierarchyNodeAsset>();
            AssetDatabase.AddObjectToAsset(hieNode, hierarchy);
            return hieNode;
        }
        #endregion

    }
}
