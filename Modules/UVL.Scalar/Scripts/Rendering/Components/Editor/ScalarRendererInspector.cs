using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

namespace UVL.Scalar.Rendering.Editors
{
    public abstract class ScalarRendererInspector : Editor
    {
        private const string VOLUME_DIRECTORY = "Materials/";
        protected const string SCALAR_VOLUME_DIRECT = "Scalar Volume Direct";
        protected const string SCALAR_VOLUME_SLICE_2D = "Scalar Volume Slice 2D";
        protected const string SCALAR_VOLUME_SLICE_3D = "Scalar Volume Slice 3D";
        protected const string SCALAR_VOLUME_SURFACE = "Scalar Volume Surface";

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Apply transfer function"))
                (target as ScalarRenderer).ApplyTransferFunction();
        }

        protected bool IsUiComponent ()
        {
            var component = target as MonoBehaviour;
            var transformType = component.transform.GetType();
            return transformType == typeof(RectTransform);
        }
        protected void EnforceMaterial (string materialName)
        {
            EditorGUILayout.LabelField($"Enforcing material '{materialName}'");

            var component = target as MonoBehaviour;

            if (IsUiComponent())
            {
                var rawImage = component.GetComponent<RawImage>();
                if (rawImage.material == null || rawImage.material.name != materialName)
                    rawImage.material = new Material(Resources.Load(VOLUME_DIRECTORY + materialName) as Material);
            }
            else
            {
                var meshRenderer = component.GetComponent<MeshRenderer>();
                if (meshRenderer.sharedMaterial == null || meshRenderer.sharedMaterial.name != materialName)
                    meshRenderer.material = Resources.Load(VOLUME_DIRECTORY + materialName) as Material;
            }
        }

        protected void EnforceRendererComponent ()
        {
            if (IsUiComponent())
                Enforce2DComponents();
            else
                Enforce3DComponents();
        }

        private void Enforce2DComponents()
        {
            EditorGUILayout.LabelField("Enforcing RawImage component");

            var component = target as MonoBehaviour;

            if (component.GetComponent<Image>() != null)
                DestroyImmediate(component.GetComponent<Image>());

            if (component.GetComponent<RawImage>() == null)
                component.gameObject.AddComponent<RawImage>();
        }

        private void Enforce3DComponents()
        {
            EditorGUILayout.LabelField("Enforcing MeshRenderer component");

            var component = target as MonoBehaviour;

            if (component.GetComponent<SkinnedMeshRenderer>() != null)
                DestroyImmediate(component.GetComponent<SkinnedMeshRenderer>());

            if (component.GetComponent<MeshRenderer>() == null)
                component.gameObject.AddComponent<MeshRenderer>();
        }
    }
}