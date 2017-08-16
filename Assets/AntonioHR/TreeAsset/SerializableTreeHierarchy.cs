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

        public void RemoveSelfAndChildren(T node)
        {
            UnattachFromParent(node);

            foreach (var child in childrenDict[node])
            {
                RemoveSelfAndChildren(child);
            }
            childrenDict.Remove(node);
            parentsDict.Remove(node);
        }
        public void RemoveSelfOnly(T node)
        {
            UnattachFromParent(node);

            foreach (var child in childrenDict[node])
            {
                UnattachFromParent(child);
            }
            childrenDict.Remove(node);
            parentsDict.Remove(node);
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
        public void UnattachFromParent(T child)
        {
            UnlinkFromParent(child, GetParentOf(child));
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
