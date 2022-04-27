using System.Collections;
using UnityEngine;

namespace UVL.Demos.TextureToScalar
{
    using UVL.IO;
    using UVL.Scalar;
    using UVL.Scalar.Rendering;

    public class ScalarSliceViewerDemo : MonoBehaviour
    {
        public string filePath;
        public ScalarSliceRenderer sliceRenderer;
        public AsyncSpinner spinner;

        private void Awake()
        {
            StartCoroutine(Load());
        }

        private IEnumerator Load()
        {
            spinner.Show();

            if (filePath.Length == 0)
                filePath = ScalarDemoData.FILEPATH;

            var textureResult = TextureSliceInput.ReadAsync(filePath);
            while (textureResult.inProgress)
                yield return null;

            var dataResult = TextureToScalarData.ToScalarSliceDataAsync(textureResult.Result);
            while (dataResult.inProgress)
                yield return null;

            var renderResult = ScalarDataToScalarRender.ToScalarSliceRenderAsync(dataResult.Result);
            while (renderResult.inProgress)
                yield return null;

            sliceRenderer.Render(renderResult.Result);

            spinner.Hide();
        }
    }
}