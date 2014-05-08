using Flai.Editor.Inspectors;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor.Inspectors
{
    [CustomEditor(typeof(MonoBehaviour), true)]
    public class DefaultInspectorProxy : DefaultInspector { }
}
