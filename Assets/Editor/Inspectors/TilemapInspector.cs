using Assets.Game.Model.Tilemap;
using Assets.Scripts;
using Flai.Editor;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor.Inspectors
{
    [CustomEditor(typeof(TilemapData))]
    public class TilemapInspector : InspectorBase<TilemapData>
    {
        private int? _previousGuid;
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

            int? currentGuid = (mapData == null) ? default(int?) : mapData.UniqueGUID;
            // todo: this doesn't work when updating a tilemap (not adding new) !! The reference (tilemap.Map) is updated automatically, so mapData and tilemap.Data are in that case the same.
            // todo: need to do this some other way. for example subscribe to "AssetChanged" etc event or always save t

            tilemap.Map = mapData;
            if (_previousGuid != currentGuid)
            {
                tilemap.OnMapUpdated();
            }

            _previousGuid = currentGuid;
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
                _previousGuid = null;
            }
        }
    }
}
