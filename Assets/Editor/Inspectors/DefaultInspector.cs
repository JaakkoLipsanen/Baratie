﻿using Flai.Editor;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor.Inspectors
{
    [CustomEditor(typeof(MonoBehaviour), true)]
    public class DefaultInspector : InspectorBase<MonoBehaviour>
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.TextField("DEFAULT INSPECTOR \\O/");
        }
    }
}
