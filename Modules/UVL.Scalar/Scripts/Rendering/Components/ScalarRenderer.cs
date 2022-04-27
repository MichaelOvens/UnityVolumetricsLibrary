using UnityEngine;

namespace UVL.Scalar.Rendering
{
    /// <summary>
    /// Single point of access to shared shader properties.
    /// </summary>

    public abstract class ScalarRenderer : UVLRenderer
    {
        public TransferFunction transferFunction;

        public void Render(Texture dataTexture, Vector3 scale)
        {
            RenderMaterial.SetTexture("_DataTex", dataTexture);

            ApplyNoise();
            ApplyTransferFunction();

            transform.localScale = NormaliseScale(scale);
        }

        public void ApplyNoise()
        {
            Texture2D noiseTexture = Noise.GenerateMonochromatic2D(512, 512);
            RenderMaterial.SetTexture("_NoiseTex", noiseTexture);
        }

        public void ApplyTransferFunction()
        {
            Texture2D transferTexture = transferFunction.GenerateTexture();
            RenderMaterial.SetTexture("_TFTex", transferTexture);
        }

        public void SetWindow(float min, float max)
        {
            RenderMaterial.SetFloat("_WindowMin", min);
            RenderMaterial.SetFloat("_WindowMax", max);
        }

        public void SetCutoff(float min, float max)
        {
            RenderMaterial.SetFloat("_CutMin", min);
            RenderMaterial.SetFloat("_CutMax", max);
        }

        public void Clear()
        {
            RenderMaterial.SetTexture("_DataTex", null);
            RenderMaterial.SetTexture("_NoiseTex", null);
            RenderMaterial.SetTexture("_TFTex", null);

            transform.localScale = Vector3.one;
        }

        private Vector3 NormaliseScale(Vector3 scale)
        {
            // Ensure there are no zero or negative terms
            for (int i = 0; i < 3; i++)
                if (scale[i] <= Mathf.Epsilon)
                    scale[i] = Mathf.Epsilon;

            // Find the largest term
            float maxTerm = Mathf.NegativeInfinity;
            for (int i = 0; i < 3; i++)
                if (scale[i] > maxTerm)
                    maxTerm = scale[i];

            // Normalise according to the largest term
            return new Vector3()
            {
                x = scale.x / maxTerm,
                y = scale.y / maxTerm,
                z = scale.z / maxTerm
            };
        }
    }
}