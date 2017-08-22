using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AntonioHR.TreeAsset.Internal;

namespace AntonioHR.TreeAsset
{
    public static class TreeOperations
    {
        #region TreeNode
        public static void ChangeParentTo(this TreeNodeAsset node, TreeNodeAsset newParent)
        {
            node._hierarchy.ChangeParentTo(newParent._hierarchy);
        }
        public static void UnattachFromParent(this TreeNodeAsset node)
        {
            node._hierarchy.UnattachFromParent();
        }
  
        public static void DeleteNodeAndChildren(this TreeNodeAsset self)
        {
            self.UnattachFromParent();

            var removedContent = self.TopDown();

            self._hierarchy.DeleteHierarchy();

            foreach (var node in removedContent)
            {
                
            }
        }
        #endregion


        #region Tree Interface
        
        public static IEnumerable<T> TopDown<T>(this T self) where T : ITreeNode<T>
        {
            List<T> list = new List<T>();
            TopDownAdd(self, list);

            return list.AsEnumerable();
        }
        public static IEnumerable<T> BottomUp<T>(this T self) where T : ITreeNode<T>
        {
            List<T> list = new List<T>();
            BottomUpAdd(self, list);

            return list.AsEnumerable();
        }

        private static void TopDownAdd<T>(T current, List<T> list) where T : ITreeNode<T>
        {
            list.Add(current);
            foreach (var child in current.Children)
            {
                TopDownAdd(child, list);
            }
        }
        private static void BottomUpAdd<T>(T current, List<T> list) where T : ITreeNode<T>
        {
            foreach (var child in current.Children)
            {
                BottomUpAdd(child, list);
            }
            list.Add(current);
        }
        #endregion
    }
}
