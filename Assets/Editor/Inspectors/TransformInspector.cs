using Flai;
using Flai.Editor;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor.Inspectors
{
    // not used atm
 //   [CustomEditor(typeof(Transform))]
    public class TransformInspector : InspectorBase<Transform>
    {
        public override void OnInspectorGUI()
        {
            bool is2dMode = EditorPrefs.GetBool("IsTransformInspectorIn2DMode", false);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            is2dMode = GUILayout.Toggle(is2dMode, "2D", "Button", GUILayout.Width(30));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            EditorPrefs.SetBool("IsTransformInspectorIn2DMode", is2dMode);

            if (is2dMode)
            {
                this.Draw2D();
            }
            else
            {
                this.Draw3D();
            }
        }

        private void Draw2D()
        {
            Vector2f localPosition = EditorGUILayout.Vector2Field("Position", this.Target.GetPosition2D());
            float localRotation = EditorGUILayout.FloatField("Rotation", this.Target.GetRotation2D());
            Vector2f localScale = EditorGUILayout.Vector2Field("Scale", this.Target.GetScale2D());
             
            this.Target.localPosition = new Vector3(localPosition.X, localPosition.Y, this.Target.localPosition.z);
            this.Target.localEulerAngles = new Vector3(this.Target.localEulerAngles.x, this.Target.localEulerAngles.y, localRotation);
            this.Target.localScale = new Vector3(localScale.X, localScale.Y, this.Target.localScale.z);
        }

        private void Draw3D()
        {
            this.Target.localPosition = EditorGUILayout.Vector3Field("Position", this.Target.localPosition);
            this.Target.localEulerAngles = EditorGUILayout.Vector3Field("Rotation", this.Target.localEulerAngles);
            this.Target.localScale = EditorGUILayout.Vector3Field("Scale", this.Target.localScale);
        }
    }
}
