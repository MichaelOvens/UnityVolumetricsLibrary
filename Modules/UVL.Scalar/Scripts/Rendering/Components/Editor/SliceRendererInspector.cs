using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace UVL.Scalar.Rendering.Editors
{
    [CustomEditor(typeof(ScalarSliceRenderer))]
    public class SliceRendererInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }
}