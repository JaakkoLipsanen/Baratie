using Assets.Scripts.Objects.Pillar;
using Flai.Editor;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor.Inspectors
{
    [CustomEditor(typeof(PillarHandler))]
    public class PillarHandlerInspector : InspectorBase<PillarHandler>
    {
        public override void OnInspectorGUI()
        {
            bool wasOnByDefault = this.Target.IsOnByDefault;
            float previousTargetScale = this.Target.TargetScale;
            this.Target.TargetScale = EditorGUILayout.FloatField("Target Scale", this.Target.TargetScale);
            this.Target.IsOnByDefault = EditorGUILayout.Toggle("Is On By Default", this.Target.IsOnByDefault);

            GUI.enabled = false;
            EditorGUILayout.TextField("Is On", this.Target.IsOn.ToString());

            if (wasOnByDefault != this.Target.IsOnByDefault || previousTargetScale != this.Target.TargetScale)
            {
                this.Target.RefreshFromInspector();
                EditorUtility.SetDirty(this.Target);
            }
        }
    }
}
