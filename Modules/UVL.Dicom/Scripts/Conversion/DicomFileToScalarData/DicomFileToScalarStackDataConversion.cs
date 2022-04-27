using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Dicom;

namespace UVL.Dicom.Conversion
{
    using UVL.Scalar;

    public static class DicomFileToScalarStackDataConversion
    {
        public static AsyncResult<ScalarStackData> ConvertAsync(DicomFile[] dicomFiles)
        {
            var result = new AsyncResult<ScalarStackData>();
            AsyncFactory.Instance.StartCoroutine(CreateStack(result, dicomFiles));
            return result;
        }

        private static IEnumerator CreateStack(AsyncResult<ScalarStackData> result, DicomFile[] dicomFiles)
        {
            result.Start();

            DicomFile[] imageBearingFiles = DicomFileToScalarDataConversion.GetImageBearingDicomFiles(dicomFiles);

            AsyncResult<ScalarSliceData>[] sliceResults = new AsyncResult<ScalarSliceData>[imageBearingFiles.Length];
            for (int i = 0; i < sliceResults.Length; i++)
                sliceResults[i] = DicomFileToScalarSliceDataConversion.ConvertAsync(imageBearingFiles[i]);

            while (SlicesStillProcessing(sliceResults))
                yield return null;

            ScalarSliceData[] sliceData = new ScalarSliceData[sliceResults.Length];
            for (int i = 0; i < sliceResults.Length; i++)
                sliceData[i] = sliceResults[i].Result;

            ScalarStackData stackData = new ScalarStackData()
            {
                slices = sliceData
            };

            result.Complete(stackData);
        }

        private static bool SlicesStillProcessing (AsyncResult<ScalarSliceData>[] sliceResults)
        {
            foreach (var result in sliceResults)
                if (result.inProgress)
                    return true;

            return false;
        }
    }
}