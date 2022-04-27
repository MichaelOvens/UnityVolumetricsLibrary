using System.Collections;
using UnityEngine;

namespace UVL.Demos.DicomToScalar
{
    using UVL.Dicom;
    using UVL.Dicom.IO;
    using UVL.Scalar.Rendering;

    public class DicomScalarSliceViewerDemo : MonoBehaviour
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
                filePath = DicomScalarDemoData.FILEPATH;

            var dicomResult = DicomFileInput.ReadAsync(filePath);
            while (dicomResult.inProgress)
                yield return null;

            var dataResult = DicomFileToScalarData.ToScalarSliceDataAsync(dicomResult.Result);
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