using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UVL
{
    /// <summary>
    /// Provides a single point of access to both worldspace and canvas renderer materials.
    /// </summary>
    public abstract class UVLRenderer : MonoBehaviour
    {
        public Material RenderMaterial
        {
            get { return GetMaterial(); }
            set { SetMaterial(value); }
        }
        private Material material = null;

        private Material GetMaterial()
        {
            if (material == null)
            {
                RawImage image = GetComponent<RawImage>();
                MeshRenderer renderer = GetComponent<MeshRenderer>();

                if (image != null)
                    material = GetMaterial2D(image);
                else if (renderer != null)
                    material = GetMaterial3D(renderer);
                else
                    throw new System.NullReferenceException("No material component found!");
            }

            return material;
        }

        private Material GetMaterial2D(RawImage image)
        {
            // Instancing for RawTexture material must be done manually
            Material instance = new Material(image.material);
            image.material = instance;
            return instance;
        }

        private Material GetMaterial3D(MeshRenderer renderer)
        {
            return renderer.material;
        }

        private void SetMaterial(Material value)
        {
            RawImage image = GetComponent<RawImage>();
            MeshRenderer renderer = GetComponent<MeshRenderer>();

            if (image != null)
                SetMaterial2D(image, value);
            if (renderer != null)
                SetMaterial3D(renderer, value);
            else
                throw new System.NullReferenceException("No material component found!");

            material = value;
        }

        private void SetMaterial2D(RawImage image, Material value)
        {
            image.material = value;
        }

        private void SetMaterial3D(MeshRenderer renderer, Material value)
        {
            renderer.material = value;
        }
    }
}