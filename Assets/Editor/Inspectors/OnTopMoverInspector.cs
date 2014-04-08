using Assets.Scripts.General;
using Flai.Editor;
using UnityEditor;

namespace Assets.Editor.Inspectors
{
    [CustomEditor(typeof(OnTopMover))]
    public class OnTopMoverInspector : InspectorBase<OnTopMover>
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.IntField("Count", this.Target.Count);
        }
    }
}
