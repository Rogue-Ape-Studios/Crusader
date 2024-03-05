using UnityEngine;
using UnityEditor;
using RogueApeStudio.Crusader.Spawn;

namespace RogueApeStudio.Crusader.Spawn.Editor
{    
    [CustomEditor(typeof(SpawnManager))]
    public class SpawnManagerEditor : UnityEditor.Editor {
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            
        }
    }
}