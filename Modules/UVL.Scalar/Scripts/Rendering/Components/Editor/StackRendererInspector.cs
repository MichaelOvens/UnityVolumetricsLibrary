using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace UVL.Scalar.Rendering.Editors
{
    [CustomEditor(typeof(ScalarStackRenderer))]
    public class StackRendererInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            var stackRenderer = target as ScalarStackRenderer;
            int index = EditorGUILayout.IntSlider(stackRenderer.index, 0, stackRenderer.length);
            if (index != stackRenderer.index)
                stackRenderer.ViewSliceAtIndex(index);

            base.OnInspectorGUI();
        }
    }
}