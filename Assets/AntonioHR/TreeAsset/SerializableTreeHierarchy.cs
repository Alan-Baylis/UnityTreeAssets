using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AntonioHR.TreeAsset
{
    [Serializable]
    public class SerializableTreeHierarchy<T>: ISerializationCallbackReceiver where T:class
    {
        [SerializeField]
        private SerializedTree<T> serialized;
        [SerializeField]
        private T root;

        Dictionary<T, List<T>> childrenDict;
        Dictionary<T, T> parentsDict;


        public T GetParentOf(T node)
        {
            return parentsDict[node];
        }
        public IEnumerable<T> GetChildrenOf(T node)
        {
            return childrenDict[node].AsEnumerable();
        }

        public IEnumerable<T> RemoveSelfAndChildren(T node)
        {
            var allRemoved = new List<T>();
            allRemoved.Add(node);

            DetachFromParent(node);

            foreach (var child in childrenDict[node])
            {
                allRemoved.AddRange(RemoveSelfAndChildren(child));
            }
            childrenDict.Remove(node);
            parentsDict.Remove(node);
            return allRemoved;
        }
        public void DetatchAllChildrenOf(T node)
        {
            foreach (var child in childrenDict[node])
            {
                DetachFromParent(child);
            }
        }

        public void AddFloating(T node)
        {
            childrenDict.Add(node, new List<T>());
            parentsDict.Add(node, null);
        }

        public void ChangeParentOfTo(T child, T newParent)
        {
            var oldParent = GetParentOf(child);
            if(oldParent != null)
                UnlinkFromParent(child, oldParent);
            LinkToParent(child, newParent);
        }
        public void DetachFromParent(T child)
        {
            UnlinkFromParent(child, GetParentOf(child));
        }

        public void Swap(T one, T other)
        {
            //Updating in parents dict
            var onesParent = GetParentOf(one);
            var othersParent = GetParentOf(other);

            parentsDict[one] = othersParent;
            parentsDict[other] = onesParent;

            //Updating in childrens Dict
            var othersParentsChildren = childrenDict[othersParent];
            var othersIndex = othersParentsChildren.IndexOf(other);

            var onesParentsChildren = childrenDict[onesParent];
            var onesIndex = onesParentsChildren.IndexOf(one);

            othersParentsChildren[othersIndex] = one;
            onesParentsChildren[onesIndex] = other;
        }

        private void LinkToParent(T child, T newParent)
        {
            childrenDict[newParent].Add(child);
            parentsDict[child] = newParent;
        }
        private void UnlinkFromParent(T child, T oldParent)
        {
            childrenDict[oldParent].Remove(child);
            parentsDict[child] = null;
        }


        #region Serialization
        public void OnAfterDeserialize()
        {
            childrenDict = new Dictionary<T, List<T>>(serialized.hierarchy);

            GenerateChildrenDictionary();
        }

        private void GenerateChildrenDictionary()
        {
            parentsDict = new Dictionary<T, T>();
            foreach (var entry in childrenDict)
            {
                foreach (var child in entry.Value)
                {
                    parentsDict.Add(child, entry.Key);
                }
            }
        }

        public void OnBeforeSerialize()
        {
            serialized.hierarchy = new SerializableDictionary<T, List<T>>(childrenDict);
        }


        [Serializable]
        class SerializedTree<T>
        {
            public SerializableDictionary<T, List<T>> hierarchy;
        }
        #endregion
    }

}
