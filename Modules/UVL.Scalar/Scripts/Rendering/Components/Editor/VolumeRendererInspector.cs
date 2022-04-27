using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace UVL.Scalar.Rendering.Editors
{
    [CustomEditor(typeof(ScalarVolumeRenderer))]
    public class VolumeRendererInspector : ScalarRendererInspector
    {
        public override void OnInspectorGUI()
        {
            if (!Application.isPlaying)
            {
                EnforceRendererComponent();
                EnforceMaterial(SCALAR_VOLUME_DIRECT);
            }

            base.OnInspectorGUI();
        }
    }
}