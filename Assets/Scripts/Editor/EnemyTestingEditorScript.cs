using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemyTestingUI))]
[CanEditMultipleObjects]
public class EnemyTestingEditorScript : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EnemyTestingUI eUI = (EnemyTestingUI)target;

        if(GUILayout.Button("Spawn Enemies"))
        {
            EditorUtility.SetDirty(FindObjectOfType<SAIM>());
            eUI.SpawnEnemies();
            EditorUtility.SetDirty(eUI);
            
        }

        if (GUILayout.Button("Clear Enemies"))
        {
            eUI.ClearEnemies();
        }
    }
}
