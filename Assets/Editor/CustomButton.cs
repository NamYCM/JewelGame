using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LoadMapData))]
public class CustomLoadMapData : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        LoadMapData myScript = (LoadMapData)target;
        if (GUILayout.Button("Load Map To Firestore"))
        {
            myScript.LoadMapToFirestore();
        }

        if (GUILayout.Button("Get All Map"))
        {
            myScript.GetAllMap();
        }
    }
}
