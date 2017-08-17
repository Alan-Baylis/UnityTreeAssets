using AntonioHR.TreeAsset;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleScript : MonoBehaviour {
    [Serializable]
    public class TreeSerializeTest : SerializableTreeHierarchy<GameObject> { }
    [Serializable]
    public class SerializedDict : SerializableDictionary<TreeNode, TreeNodeList> { }
    [Serializable]
    public class TreeNodeList : List<TreeNode> { }

    public SerializedDict dict;
    public TreeSerializeTest hierarchy;
	void Start () {
        hierarchy.Init(gameObject);
	}
}
