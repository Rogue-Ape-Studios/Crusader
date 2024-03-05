using System;
using UnityEditor;
using UnityEngine;

namespace RogueApeStudio.Crusader.Spawn.Editor
{
    [CustomEditor(typeof(SpawnManager))]
    public class SpawnManagerEditor : UnityEditor.Editor
    {
        private SpawnManager _spawnManager;
        private bool _enableEditorSettings;
        public override void OnInspectorGUI()
        {
            _spawnManager = target as SpawnManager;

            base.DrawDefaultInspector();

            DrawSpawnButtons();

            _enableEditorSettings = GUILayout.Toggle(_enableEditorSettings, "Enable Editor Settings");

            if(_enableEditorSettings)
            {
                DrawBezierSettings();
            }
        }

        private void DrawSpawnButtons()
        {
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();

            if(GUILayout.Button("Add Spawn"))
            {
                _spawnManager.AddSpawn();
            }   

            if(GUILayout.Button("Remove Spawn"))
            {
                _spawnManager.DestroyLastSpawn();
            }

            GUILayout.EndHorizontal();
            GUILayout.EndVertical();       
        }

        private void DrawBezierSettings()
        {
            _spawnManager.BeginTangent = EditorGUILayout.Vector3Field("Begin Tangent", _spawnManager.BeginTangent);
            _spawnManager.EndTangent = EditorGUILayout.Vector3Field("End Tangent", _spawnManager.EndTangent);
        }
    }
}