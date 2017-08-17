using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AntonioHR.TreeAsset
{
    [CustomEditor(typeof(TreeAsset))]
    public class TreeAssetInspector : Editor
    {

        public override void OnInspectorGUI()
        {
            var treeAsset = target as TreeAsset;
            base.OnInspectorGUI();
            if (GUILayout.Button("Generate Debug GO Hierarchy"))
            {
                treeAsset.CreateDebugGameObjectHierarchy();
            }
        }
    }
}