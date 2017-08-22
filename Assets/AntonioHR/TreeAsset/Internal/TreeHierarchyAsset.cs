using AntonioHR.TreeAsset.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace AntonioHR.TreeAsset.Internal
{
    public class TreeHierarchyAsset: ScriptableObject
    {
        #region SerializedFields
        [SerializeField]
        internal TreeHierarchyNodeAsset _root;
        [SerializeField]
        internal List<TreeHierarchyNodeAsset> _floaters = new List<TreeHierarchyNodeAsset>();
        #endregion

        public TreeNode Root
        {
            get
            {
                return _root == null ? null : _root._content;
            }
        }
        public IEnumerable<TreeNode> FloatingNodes
        {
            get
            {
                return _floaters.Select(x => x._content);
            }
        }
    }

}
