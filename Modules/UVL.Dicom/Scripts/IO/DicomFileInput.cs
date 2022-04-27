using System.Collections;
using System.Threading.Tasks;

using Dicom;

namespace UVL.Dicom.IO
{
    public static class DicomFileInput
    {
        public static AsyncResult<DicomFile> ReadAsync(string filePath)
        {
            var result = new AsyncResult<DicomFile>();
            AsyncFactory.Instance.StartCoroutine(ReadAsync(result, filePath));
            return result;
        }

        private static IEnumerator ReadAsync(AsyncResult<DicomFile> result, string filePath)
        {
            result.Start();

            DicomFile dicomFile = null;

            Task task = Task.Run(() => { dicomFile = DicomFile.Open(filePath); });

            while (!task.IsCompleted)
                yield return null;

            result.Complete(dicomFile);
        }
    }
}