using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AntonioHR.TreeAsset
{
    public abstract class TreeNode : ScriptableObject
    {
        protected abstract int MaximumChildrenCount { get; }

        public bool IsLeaf { get { return MaximumChildrenCount == 0; } }
        public bool IsDecorator { get { return MaximumChildrenCount == 1; } }
        public bool IsComposite { get { return MaximumChildrenCount > 1; } }

        public abstract IEnumerable<TreeNode> Children { get; }
        public abstract void AddChild(TreeNode child);

        public abstract void BeforeDelete();
    }
}
