using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace UVL.Scalar.Rendering.Editors
{
    [CustomEditor(typeof(ScalarVolumeRenderer))]
    public class VolumeRendererInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }
}