using Assets.Scripts.General;
using Flai.Diagnostics;
using Flai.Editor.Inspectors;
using Flai.Tilemap;
using System;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor.Inspectors
{
    [CustomEditor(typeof(Camera))]
    public class CameraInspectorProxy : CameraInspector
    {
        protected override void Draw2DMode()
        {
            base.Draw2DMode();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Fit Camera To Room", GUILayout.Width(140)))
            {
                this.FitCameraToRoom();
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        private void FitCameraToRoom()
        {
            GameObject levelGameObject = GameObject.Find("Level");
            if (levelGameObject == null)
            {
                FlaiDebug.LogWithTypeTag<CameraInspector>("Could not find the Level gameobject!");
                return;
            }

            Level level = levelGameObject.Get<Level>();
            TilemapContainer tilemapContainer = level.TilemapContainer;
            if (tilemapContainer == null || tilemapContainer.TmxAsset == null)
            {
                FlaiDebug.LogWithTypeTag<CameraInspector>("Could not find the Tilemap!");
                return;
            }

            int width = tilemapContainer.Size.Width;
            int height = tilemapContainer.Size.Height;
            int max = Math.Max(width, height);

            Camera camera = this.Target;
            camera.gameObject.SetPosition2D(width * Tile.Size / 2f, height * Tile.Size / 2f);
            camera.orthographicSize = max / 2f;
        }
    }
}
