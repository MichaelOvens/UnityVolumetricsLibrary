using System.Collections;
using UnityEngine;

namespace UVL.Demos.TextureToScalar
{
    using UVL.IO;
    using UVL.Scalar;
    using UVL.Scalar.Rendering;

    public class ScalarVolumeViewerDemo : MonoBehaviour
    {
        public string directory;
        public ScalarVolumeRenderer volumeRenderer;
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

            var textureResult = TextureVolumeInput.ReadAsync(directory);
            while (textureResult.inProgress)
                yield return null;

            var dataResult = TextureToScalarData.ToScalarVolumeDataAsync(textureResult.Result);
            while (dataResult.inProgress)
                yield return null;

            var renderResult = ScalarDataToScalarRender.ToScalarVolumeRenderAsync(dataResult.Result);
            while (renderResult.inProgress)
                yield return null;

            volumeRenderer.Render(renderResult.Result);

            spinner.Hide();
        }
    }
}