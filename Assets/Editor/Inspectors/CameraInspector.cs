using System;
using Assets.Scripts;
using Assets.Scripts.General;
using Flai.Diagnostics;
using Flai.Editor;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor.Inspectors
{
    [CustomEditor(typeof(Camera))]
    public class CameraInspector : InspectorBase<Camera>
    {
        public override void OnInspectorGUI()
        {
            bool is2dMode = EditorPrefs.GetBool("IsCameraInspectorIn2DMode", false);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            is2dMode = GUILayout.Toggle(is2dMode, "2D", "Button", GUILayout.Width(30));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            if (is2dMode)
            {
                this.Draw2DMode();
            }
            else
            {
                base.OnInspectorGUI();
            }

            EditorPrefs.SetBool("IsCameraInspectorIn2DMode", is2dMode);
        }

        private void Draw2DMode()
        {
            var labelWidth = GUILayout.Width(Screen.width / 2.5f);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Background Color", labelWidth);
            this.Target.backgroundColor = EditorGUILayout.ColorField(this.Target.backgroundColor);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Ortographics Size", labelWidth);
            this.Target.orthographicSize = EditorGUILayout.FloatField(this.Target.orthographicSize);
            EditorGUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Setup 2D", GUILayout.Width(80)))
            {
                this.Setup2D();
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            // todo: this is the only game specific thing :/
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Fit Camera To Room", GUILayout.Width(140)))
            {
                this.FitCameraToRoom();
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        private void Setup2D()
        {
            Camera camera = this.Target;
            camera.isOrthoGraphic = true;
            camera.orthographic = true;
            camera.transform.eulerAngles = Vector3.zero;
            camera.orthographicSize = 10;
            if (camera.transform.position.z >= 0)
            {
                camera.transform.SetPositionZ(-5);
            }
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
            TilemapData tilemapData = level.TilemapData;
            if (tilemapData == null || tilemapData.Map == null)
            {
                FlaiDebug.LogWithTypeTag<CameraInspector>("Could not find the Tilemap!");
                return;
            }

            int width = tilemapData.Tilemap.Width;
            int height = tilemapData.Tilemap.Height;
            int max = Math.Max(width, height);

            Camera camera = this.Target;
            camera.gameObject.SetPosition2D(width * Tile.Size / 2f, height * Tile.Size / 2f);
            camera.orthographicSize = max / 2f;
        }
    }
}
