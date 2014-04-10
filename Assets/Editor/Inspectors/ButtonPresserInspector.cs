using Assets.Scripts.Objects;
using Flai.Editor;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor.Inspectors
{
    [CustomEditor(typeof(ButtonPresser))]
    public class ButtonPresserInspector : InspectorBase<ButtonPresser>
    {
        public override void OnInspectorGUI()
        {
            GUI.enabled = false;
            EditorGUILayout.TextField("Is Pressed",this.Target.IsPressed.ToString());
            GUI.enabled = true;
        }
    }
}
