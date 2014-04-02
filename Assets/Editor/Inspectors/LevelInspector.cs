/*using Assets.Scripts.General;
using Flai.Editor;
using Tiled;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor
{
    [CustomEditor(typeof(Level))]
    public class LevelInspector : InspectorBase<Level>
    {
        private bool _isImporting = false;
        private TmxData _tmx;

        public override void OnInspectorGUI()
        {
            Level level = this.Target;
            this.DrawLevelInfoGUI(level);

            _isImporting = GUILayout.Toggle(_isImporting, "Import Tilemap", "Button");
            if (_isImporting)
            {

                _tmx = (TmxData)EditorGUILayout.ObjectField("Tilemap", _tmx, typeof(TmxData), false);
                GUI.enabled = _tmx != null;
                if (GUILayout.Button("Import!"))
                {
                    level.LoadLevel(_tmx);
                    _tmx = null;
                    _isImporting = false;

                }
                GUI.enabled = true;

                EditorGUILayout.Space();
            }

            GUI.enabled = !_isImporting;
            GUILayout.Button("Reset");
            GUI.enabled = true;
        }

        private void DrawLevelInfoGUI(Level level)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Size", EditorStyles.boldLabel, GUILayout.Width(40));
            GUI.enabled = false;
            EditorGUILayout.IntField(level.MapSize.Width);
            EditorGUILayout.IntField(level.MapSize.Height);
            GUI.enabled = true;
            EditorGUILayout.EndHorizontal();
        }
    }
}*/