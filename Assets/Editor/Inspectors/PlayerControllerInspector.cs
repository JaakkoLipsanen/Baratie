using Assets.Scripts.Player;
using Flai.Editor;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor.Inspectors
{
    [CustomEditor(typeof(PlayerController))]
    public class PlayerControllerInspector : InspectorBase<PlayerController>
    {
        public override void OnInspectorGUI()
        {
            this.DrawDefaultInspector();

            GUI.enabled = false;
            EditorGUILayout.TextField("Is On Ground", this.Target.IsOnGround.ToString());
            EditorGUILayout.TextField("Can Jump", this.Target.CanJump.ToString());

            GUI.enabled = true;
        }
    }
}
