using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AntonioHR.TreeAsset
{
    [Serializable]
    public class SerializableTreeHierarchy: ISerializationCallbackReceiver
    {
        [SerializeField]
        private SerializedTree serialized;
        [SerializeField]
        private TreeNode root;
        [SerializeField]
        private bool initialized = false;

        private Dictionary<TreeNode, TreeNodeList> descendancyDict;
        private Dictionary<TreeNode, TreeNode> ascendancyDict;

        

        public SerializableTreeHierarchy()
        {
        }
        public void Init(TreeNode newRoot)
        {
            descendancyDict = new Dictionary<TreeNode, TreeNodeList>();
            ascendancyDict = new Dictionary<TreeNode, TreeNode>();
            Debug.Assert(!initialized);
            AddFloating(newRoot);
            this.root = newRoot;
            initialized = true;
        }

        public TreeNode Root
        {
            get
            {
                return root;
            }
        }
        public TreeNode GetParentOf(TreeNode node)
        {
            return ascendancyDict[node];
        }
        public IEnumerable<TreeNode> GetChildrenOf(TreeNode node)
        {
            return descendancyDict[node].AsEnumerable();
        }
        public IEnumerable<TreeNode> GetAllFloating()
        {
            return ascendancyDict.Where(x => x.Value == null && x.Key != root).Select(x => x.Key);
        }

        public IEnumerable<TreeNode> RemoveSelfAndChildren(TreeNode node)
        {
            var allRemoved = new TreeNodeList();
            allRemoved.Add(node);

            DetachFromParent(node);

            foreach (var child in descendancyDict[node])
            {
                allRemoved.AddRange(RemoveSelfAndChildren(child));
            }
            descendancyDict.Remove(node);
            ascendancyDict.Remove(node);
            return allRemoved;
        }
        public void DetatchAllChildrenOf(TreeNode node)
        {
            foreach (var child in descendancyDict[node])
            {
                DetachFromParent(child);
            }
        }

        public void AddFloating(TreeNode node)
        {
            descendancyDict.Add(node, new TreeNodeList());
            ascendancyDict.Add(node, null);
        }

        public void ChangeParentOfTo(TreeNode child, TreeNode newParent)
        {
            var oldParent = GetParentOf(child);
            if(oldParent != null)
                UnlinkFromParent(child, oldParent);
            LinkToParent(child, newParent);
        }
        public void DetachFromParent(TreeNode child)
        {
            UnlinkFromParent(child, GetParentOf(child));
        }

        public void Swap(TreeNode one, TreeNode other)
        {
            //Updating in parents dict
            var onesParent = GetParentOf(one);
            var othersParent = GetParentOf(other);

            ascendancyDict[one] = othersParent;
            ascendancyDict[other] = onesParent;

            //Updating in childrens Dict
            var othersParentsChildren = descendancyDict[othersParent];
            var othersIndex = othersParentsChildren.IndexOf(other);

            var onesParentsChildren = descendancyDict[onesParent];
            var onesIndex = onesParentsChildren.IndexOf(one);

            othersParentsChildren[othersIndex] = one;
            onesParentsChildren[onesIndex] = other;
        }

        private void LinkToParent(TreeNode child, TreeNode newParent)
        {
            descendancyDict[newParent].Add(child);
            ascendancyDict[child] = newParent;
        }
        private void UnlinkFromParent(TreeNode child, TreeNode oldParent)
        {
            descendancyDict[oldParent].Remove(child);
            ascendancyDict[child] = null;
        }


        #region Serialization
        public void OnAfterDeserialize()
        {
            descendancyDict = new Dictionary<TreeNode, TreeNodeList>();

            for (int i = 0; i < serialized.keys.Length; i++)
			{
                descendancyDict.Add(serialized.keys[i], serialized.values[i]);
            }

            GenerateAscendancyDictionary();
        }

        private void GenerateAscendancyDictionary()
        {
            ascendancyDict = new Dictionary<TreeNode, TreeNode>();
            foreach (var entry in descendancyDict)
            {
                foreach (var child in entry.Value)
                {
                    ascendancyDict.Add(child, entry.Key);
                }
            }
        }

        public void OnBeforeSerialize()
        {
            if(descendancyDict != null)
                serialized = new SerializedTree(descendancyDict);
        }

        [Serializable]
        public class TreeNodeList : List<TreeNode>
        {

        }

        [Serializable]
        public class SerializedTree
        {
            public TreeNode[] keys;
            public TreeNodeList[] values;
            public SerializedTree(){ }

            public SerializedTree(Dictionary<TreeNode, TreeNodeList> descendancyDict)
            {
                keys = new TreeNode[descendancyDict.Count];
                values = new TreeNodeList[descendancyDict.Count];

                int i = 0;
                foreach (var keyValuePair in descendancyDict)
                {
                    keys[i] = keyValuePair.Key;
                    values[i] = keyValuePair.Value;
                    i++;
                }
            }
        }
        #endregion

    }

}
