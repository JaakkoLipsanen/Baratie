using Assets.Game.Model.Tilemap;
using Assets.Scripts.Tiles;
using Flai.Diagnostics;
using Flai.Editor;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor.Inspectors
{
    [CustomEditor(typeof(TilemapContainer))]
    public class TilemapContainerInspector : InspectorBase<TilemapContainer>
    {
        public override void OnInspectorGUI()
        {
            this.DrawMapDataField();
            this.DrawTilemapPrefabField();
            this.DrawSize();
            this.DrawButtons();
        }

        private void DrawTilemapPrefabField()
        {
            this.Target.TilemapPrefab = (GameObject) EditorGUILayout.ObjectField("Tilemap Prefab", this.Target.TilemapPrefab, typeof(GameObject), false);
            EditorUtility.SetDirty(this.Target);
        }

        private void DrawMapDataField()
        {
            TilemapContainer tilemap = this.Target;
            MapData mapData = (MapData)EditorGUILayout.ObjectField("Map Data", tilemap.Map, typeof(MapData), false);

            if (tilemap.Map != null && mapData == null)
            {
                FlaiDebug.LogWithTypeTag<TilemapContainerInspector>("Map Data set null!");
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
            TilemapContainer tilemap = this.Target;

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Size", GUILayout.Width(Screen.width / 2.9f));

            GUI.enabled = false;
            string width = (tilemap.Map == null) ? "" : tilemap.Map.TilemapData.Width.ToString();
            string height = (tilemap.Map == null) ? "" : tilemap.Map.TilemapData.Height.ToString();
            EditorGUILayout.TextField(width);
            EditorGUILayout.TextField(height);
            GUI.enabled = true;

            EditorGUILayout.EndHorizontal();
        }

        private void DrawButtons()
        {
            TilemapContainer tilemap = this.Target;
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
