using Assets.Scripts.General;
using Assets.Scripts.Objects.Button;
using Flai.Editor;
using UnityEditor;

namespace Assets.Editor.Inspectors
{
    [CustomEditor(typeof(ButtonInfo))]
    public class ButtonInfoInspector : InspectorBase<ButtonInfo>
    {
        public override void OnInspectorGUI()
        {
            this.Target.Response = EditorGUILayout.ObjectField("Response", this.Target.Response, typeof(Response), true) as Response;
            EditorUtility.SetDirty(this.Target);
        }
    }
}
