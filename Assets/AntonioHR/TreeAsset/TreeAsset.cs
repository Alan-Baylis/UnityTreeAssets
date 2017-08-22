using AntonioHR.TreeAsset.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace AntonioHR.TreeAsset
{
    public abstract class TreeAsset<T>: ScriptableObject where T: TreeNodeAsset
    {
        [SerializeField]
        private TreeHierarchyAsset hierarchy;

        public T Root
        {
            get
            { 
                return hierarchy.Root as T; 
            }
        }
        public IEnumerable<T> FloatingNodes
        {
            get
            {
                return hierarchy.GetFloatingNodesAs<T>();
            }
        }

        protected abstract T InstantiateRoot();
        protected abstract T InstantiateDefaultNode();

        protected void Init()
        {
            hierarchy = ScriptableObject.CreateInstance<TreeHierarchyAsset>();
            AssetDatabase.AddObjectToAsset(hierarchy, this);
            hierarchy.name = "Hierarchy";

            var rootNode = InstantiateRoot();
            AssetDatabase.AddObjectToAsset(rootNode, this);
            hierarchy.SetupRoot(rootNode);
        }
        public T CreateNodeFloating(string name = "Node")
        {
            var newNode = InstantiateDefaultNode();
            newNode.name = name;
            AssetDatabase.AddObjectToAsset(newNode, this);
            hierarchy.AddAsFloatingNode(newNode);
            return newNode;
        }
        public T CreateChildFor(T parent, string name = "Node")
        {
            var newNode = InstantiateDefaultNode();
            newNode.name = name;
            AssetDatabase.AddObjectToAsset(newNode, this);

            hierarchy.AddAsFloatingNode(newNode);
            newNode.ChangeParentTo(parent);

            return newNode;
        }

        public void DeleteNodeAndAllChildren(T node)
        {
            node.DeleteNodeAndChildren();
        }



       
    }
}
