using Flai.Editor;
using Flai.Scripts.Character;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor.Inspectors
{
    [CustomEditor(typeof(CharacterController2D))]
    public class PlayerControllerInspector : InspectorBase<CharacterController2D>
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
