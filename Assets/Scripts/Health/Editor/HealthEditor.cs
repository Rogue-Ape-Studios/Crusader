using UnityEngine;
using UnityEditor;

namespace RogueApeStudio.Crusader.HealthSystem.Editor
{
    [CustomEditor(typeof(Health))]
    public class HealthEditor : UnityEditor.Editor
    {
        private Health _health;
        private float _hitpoints = 1;

        public override void OnInspectorGUI()
        {
            _health = target as Health;
            DrawDefaultInspector();

            EditorGUILayout.Space();
            _hitpoints = EditorGUILayout.FloatField("Hitpoint Change", _hitpoints);

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Hit"))
                _health.Hit(_hitpoints);

            if (GUILayout.Button("Heal"))
                _health.Heal(_hitpoints);

            GUILayout.EndHorizontal();
        }
    }
}