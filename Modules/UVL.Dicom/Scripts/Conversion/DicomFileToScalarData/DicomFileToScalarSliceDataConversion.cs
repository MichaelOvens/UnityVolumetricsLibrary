using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

using Dicom;

namespace UVL.Dicom.Conversion
{
    using UVL.Scalar;

    public static class DicomFileToScalarSliceDataConversion
    {
        public static AsyncResult<ScalarSliceData> ConvertAsync(DicomFile dicomFile)
        {
            var result = new AsyncResult<ScalarSliceData>();
            AsyncFactory.Instance.StartCoroutine(CreateSlice(result, dicomFile));
            return result;
        }

        private static IEnumerator CreateSlice(AsyncResult<ScalarSliceData> result, DicomFile dicomFile)
        {
            result.Start();

            Vector2Int pixelCount = DicomFileToScalarDataConversion.GetPixelCount(dicomFile);
            Vector2 physicalSize = DicomFileToScalarDataConversion.GetPhysicalSize(dicomFile, pixelCount);
            float[] values = null;

            Task task = Task.Run(() =>
            {
                values = DicomFileToScalarDataConversion.GetValues(dicomFile, pixelCount);
            });

            while (!task.IsCompleted)
                yield return null;

            ScalarSliceData sliceData = new ScalarSliceData()
            {
                pixelCount = pixelCount,
                physicalSize = physicalSize,
                values = values
            };

            result.Complete(sliceData);
        }
    }
}