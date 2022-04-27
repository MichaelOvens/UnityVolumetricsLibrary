using System.Collections;
using UnityEngine;

namespace UVL.Demos.TextureToScalar
{
    using UVL.IO;
    using UVL.Scalar;
    using UVL.Scalar.Rendering;

    public class ScalarStackViewerDemo : MonoBehaviour
    {
        public string directory;
        public ScalarStackRenderer stackRenderer;
        public AsyncSpinner spinner;

        private void Awake()
        {
            StartCoroutine(Load());
        }

        private IEnumerator Load()
        {
            spinner.Show();

            if (directory.Length == 0)
                directory = ScalarDemoData.DIRECTORY;

            var textureResult = TextureStackInput.ReadAsync(directory);
            while (textureResult.inProgress)
                yield return null;

            var dataResult = TextureToScalarData.ToScalarStackDataAsync(textureResult.Result);
            while (dataResult.inProgress)
                yield return null;

            var renderResult = ScalarDataToScalarRender.ToScalarStackRenderAsync(dataResult.Result);
            while (renderResult.inProgress)
                yield return null;

            stackRenderer.Render(renderResult.Result);

            spinner.Hide();
        }
    }
}