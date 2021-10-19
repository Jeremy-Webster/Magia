using UnityEngine;
using UnityEditor;

namespace MageiaGame
{
    [CustomEditor(typeof(Stage))]
    public class StageEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            Stage stage = (Stage)target;

            if (GUILayout.Button("Generate Enemy Spawners"))
            {
                stage.GenerateEnemySpawners();
            }
        }
    }
}

