using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace UVL.Scalar.Rendering.Editors
{
    [CustomEditor(typeof(ScalarStackRenderer))]
    public class StackRendererInspector : ScalarRendererInspector
    {
        public override void OnInspectorGUI()
        {
            if (!Application.isPlaying)
            {
                EnforceRendererComponent();
                EnforceMaterial(IsUiComponent() ? SCALAR_VOLUME_SLICE_2D : SCALAR_VOLUME_SLICE_3D);
            }

            var stackRenderer = target as ScalarStackRenderer;
            int index = EditorGUILayout.IntSlider(stackRenderer.index, 0, stackRenderer.length);
            if (index != stackRenderer.index)
                stackRenderer.ViewSliceAtIndex(index);

            base.OnInspectorGUI();
        }
    }
}