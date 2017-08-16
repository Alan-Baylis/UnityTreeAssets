using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace AntonioHR.TreeAsset
{
    public class BehaviorTree: ScriptableObject, ISerializationCallbackReceiver
    {
        private List<TreeNode> allNodes;

        private TreeNode root;




        public T CreateNode<T>() where T: TreeNode
        {
            var newNode = ScriptableObject.CreateInstance<T>();
            AssetDatabase.AddObjectToAsset(newNode, this);
            allNodes.Add(newNode);
            return newNode;
        }

        public void DeleteNode(TreeNode node)
        {
            foreach (var child in node.Children)
            {
                DeleteNode(child);
            }

            node.BeforeDelete();
            DestroyImmediate(node);
        }


        public void OnAfterDeserialize()
        {
            throw new NotImplementedException();
        }

        public void OnBeforeSerialize()
        {
            throw new NotImplementedException();
        }
    }
    [Serializable]
    public class SerializedTreeHierachy
    {
        SerializableDictionary<TreeNode, List<TreeNode>> hierarchy;

    }
}
