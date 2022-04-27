using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace UVL.Scalar.Rendering.Editors
{
    [CustomEditor(typeof(ScalarSliceRenderer))]
    public class SliceRendererInspector : ScalarRendererInspector
    {
        public override void OnInspectorGUI()
        {
            if (!Application.isPlaying)
            {
                EnforceRendererComponent();
                EnforceMaterial(IsUiComponent() ? SCALAR_VOLUME_SLICE_2D : SCALAR_VOLUME_SLICE_3D);
            }

            base.OnInspectorGUI();
        }
    }
}