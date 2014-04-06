using Assets.Game.Model.Tilemap;
using Assets.Scripts;
using Flai.Diagnostics;
using Flai.Editor;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor.Inspectors
{
    [CustomEditor(typeof(TilemapData))]
    public class TilemapInspector : InspectorBase<TilemapData>
    {
        public override void OnInspectorGUI()
        {
            this.DrawTilemapField();
            this.DrawSize();
            this.DrawButtons();
        }

        private void DrawTilemapField()
        {
            TilemapData tilemap = this.Target;
            MapData mapData = (MapData)EditorGUILayout.ObjectField("Tilemap", tilemap.Map, typeof(MapData), false);

            if (tilemap.Map != null && mapData == null)
            {
                FlaiDebug.LogWithTypeTag<TilemapInspector>("Tilemap set null!");
            }

            if (tilemap.Map != mapData || (tilemap.Map != null && tilemap.Map.NeedsRefresh))
            {
                tilemap.Map = mapData;
                tilemap.OnMapUpdated();
                EditorUtility.SetDirty(tilemap);
            }
        }

        private void DrawSize()
        {
            TilemapData tilemap = this.Target;

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Size", GUILayout.Width(Screen.width / 2.9f));

            GUI.enabled = false;
            string width = (tilemap.Map == null) ? "" : tilemap.Map.Tilemap.Width.ToString();
            string height = (tilemap.Map == null) ? "" : tilemap.Map.Tilemap.Height.ToString();
            EditorGUILayout.TextField(width);
            EditorGUILayout.TextField(height);
            GUI.enabled = true;

            EditorGUILayout.EndHorizontal();
        }

        private void DrawButtons()
        {
            TilemapData tilemap = this.Target;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.Separator();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.LabelField("General", EditorStyles.boldLabel);

            if (GUILayout.Button("Refresh"))
            {
                tilemap.OnMapUpdated();
            }

            if (GUILayout.Button("Reset"))
            {
                tilemap.Map = null;
                tilemap.OnMapUpdated();
            }
        }
    }
}
