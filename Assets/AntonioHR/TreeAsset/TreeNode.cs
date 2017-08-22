using AntonioHR.TreeAsset.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AntonioHR.TreeAsset
{
    public class TreeNode: ScriptableObject, ITreeNode<TreeNode>
    {
        public string dummyInfo;

        [SerializeField]
        internal TreeHierarchyNodeAsset _hierarchy;

        public TreeNode Parent { 
            get 
            { 
                return _hierarchy._parent._content; 
            }
        }

        public IEnumerable<TreeNode> Children { get { return _hierarchy._children.Select(x=>x._content); } }
    }
    public interface ITreeNode<T> where T : ITreeNode<T>
    {
        IEnumerable<T> Children { get; }
    }
}
