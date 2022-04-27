using System.Collections;
using UnityEngine;

namespace UVL.Demos.DicomToScalar
{
    using UVL.Dicom;
    using UVL.Dicom.IO;
    using UVL.Scalar.Rendering;

    public class DicomScalarStackViewerDemo : MonoBehaviour
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
                directory = DicomScalarDemoData.DIRECTORY;

            var dicomResult = DicomDirectoryInput.ReadAsync(directory);
            while (dicomResult.inProgress)
                yield return null;

            var dataResult = DicomFileToScalarData.ToScalarStackDataAsync(dicomResult.Result);
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