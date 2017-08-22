using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AntonioHR.TreeAsset.Internal
{
    public class TreeHierarchyNodeAsset : ScriptableObject, ITreeNode<TreeHierarchyNodeAsset>
    {
        [SerializeField]
        internal TreeHierarchyAsset _tree;

        [SerializeField]
        internal List<TreeHierarchyNodeAsset> _children = new List<TreeHierarchyNodeAsset>();
        [SerializeField]
        internal TreeHierarchyNodeAsset _parent;
        [SerializeField]
        public bool _isFloating = false;

        public TreeNodeAsset _content;


        public bool IsRoot { get { return _tree._root == this; } }

        public IEnumerable<TreeHierarchyNodeAsset> Children
        {
            get { return _children.AsEnumerable(); }
        }
    }
}
