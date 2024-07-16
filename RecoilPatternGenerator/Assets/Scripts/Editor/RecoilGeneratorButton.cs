using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RecoilGenerator))]
public class RecoilGeneratorButton : Editor
{
    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();

        GUILayout.Space(10);
        RecoilGenerator generator = (RecoilGenerator)target;

        // File setting section
        EditorGUILayout.LabelField("File settings", EditorStyles.boldLabel);
        generator.JsonFile = (TextAsset)EditorGUILayout.ObjectField("Json File", generator.JsonFile, typeof(TextAsset), false);
        generator.fileName = EditorGUILayout.TextField("File Name", generator.fileName);

        GUILayout.Space(10);
        if (GUILayout.Button("Save"))
        {
            generator.SaveScripts();
        }
        if (GUILayout.Button("Load"))
        {
            generator.LoadScripts();
        }
        GUILayout.Space(15);

        // Generator option section
        EditorGUILayout.LabelField("Generator options", EditorStyles.boldLabel);
        generator.pointPrefab = (GameObject)EditorGUILayout.ObjectField("Point Prefab", generator.pointPrefab, typeof(GameObject), false);
        generator.pointCount = EditorGUILayout.IntField("Point Count", generator.pointCount);

        GUILayout.Space(10);
        if (GUILayout.Button("Apply"))
        {
            generator.SetPointCount();
        }
    }
}
