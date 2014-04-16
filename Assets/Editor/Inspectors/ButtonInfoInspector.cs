using Assets.Scripts.Objects;
using Flai.Editor;
using UnityEditor;

namespace Assets.Editor.Inspectors
{
    [CustomEditor(typeof(ButtonInfo))]
    public class ButtonInfoInspector : InspectorBase<ButtonInfo>
    {
        public override void OnInspectorGUI()
        {
            this.DrawDefaultInspector();
           // this.Target.Response = EditorGUILayout.ObjectField("Response", this.Target.Response, typeof(Response), true) as Response;
           // EditorUtility.SetDirty(this.Target);
        }
    }
}
