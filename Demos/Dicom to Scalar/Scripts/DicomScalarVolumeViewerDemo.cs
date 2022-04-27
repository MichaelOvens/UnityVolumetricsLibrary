using System.Collections;
using UnityEngine;

namespace UVL.Demos.DicomToScalar
{
    using UVL.Dicom;
    using UVL.Dicom.IO;
    using UVL.Scalar.Rendering;

    public class DicomScalarVolumeViewerDemo : MonoBehaviour
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
                directory = DicomScalarDemoData.DIRECTORY;

            var dicomResult = DicomDirectoryInput.ReadAsync(directory);
            while (dicomResult.inProgress)
                yield return null;

            var dataResult = DicomFileToScalarData.ToScalarVolumeDataAsync(dicomResult.Result);
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