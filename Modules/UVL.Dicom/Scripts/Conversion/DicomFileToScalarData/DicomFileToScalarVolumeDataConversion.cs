using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

using FellowOakDicom;

namespace UVL.Dicom.Conversion
{
    using UVL.Scalar;

    public static class DicomFileToScalarVolumeDataConversion
    {
        public static AsyncResult<ScalarVolumeData> ConvertAsync(DicomFile[] dicomFiles)
        {
            var result = new AsyncResult<ScalarVolumeData>();
            AsyncFactory.Instance.StartCoroutine(CreateVolume(result, dicomFiles));
            return result;
        }

        private static IEnumerator CreateVolume(AsyncResult<ScalarVolumeData> result, DicomFile[] dicomFiles)
        {
            result.Start();

            DicomFile[] imageBearingFiles = DicomFileToScalarDataConversion.GetImageBearingDicomFiles(dicomFiles);

            AsyncResult<ScalarStackData> stackResult = DicomFileToScalarStackDataConversion.ConvertAsync(imageBearingFiles);

            while (stackResult.inProgress)
                yield return null;

            ScalarStackData stackData = stackResult.Result;

            if (StackTextureDimensionsAreNotUniform(stackData.slices))
            {
                result.Throw(new DataMisalignedException("Dicom stack has non-uniform dimensions"));
                yield break;
            }

            Vector3Int voxelCount = GetVoxelCount(stackData);
            Vector3 physicalSize = GetPhysicalSize(stackData, dicomFiles);
            float[] values = new float[voxelCount.x * voxelCount.y * voxelCount.z];

            // Start this parallel operation in a task so it doesn't lock the main thread
            Task task = Task.Run(() =>
            {
                Parallel.For(0, stackData.slices.Length, i =>
                {
                    stackData.slices[i].values.CopyTo(values, i * voxelCount.x * voxelCount.y);
                });
            });

            while (!task.IsCompleted)
                yield return null;

            ScalarVolumeData volumeData = new ScalarVolumeData()
            {
                voxelCount = voxelCount,
                physicalSize = physicalSize,
                values = values
            };

            result.Complete(volumeData);
        }

        private static bool StackTextureDimensionsAreNotUniform(ScalarSliceData[] sliceData)
        {
            Vector2Int pixelCount = sliceData[0].pixelCount;

            foreach (var slice in sliceData)
                if (slice.pixelCount != pixelCount)
                    return true;

            return false;
        }

        private static Vector3Int GetVoxelCount (ScalarStackData stackData)
        {
            return new Vector3Int()
            {
                x = stackData.slices[0].pixelCount.x,
                y = stackData.slices[0].pixelCount.y,
                z = stackData.slices.Length
            };
        }

        private static Vector3 GetPhysicalSize (ScalarStackData stackData, DicomFile[] dicomFiles)
        {
            bool containsSpacingBetweenSLices = dicomFiles[0].Dataset.Contains(DicomTag.SpacingBetweenSlices);
            decimal spacingBetweenSlices = containsSpacingBetweenSLices ? dicomFiles[0].Dataset.GetValue<decimal>(DicomTag.SpacingBetweenSlices, 0) : 1;

            return new Vector3()
            {
                x = stackData.slices[0].physicalSize.x,
                y = stackData.slices[0].physicalSize.y,
                z = Mathf.Abs((float)(spacingBetweenSlices * stackData.slices.Length))
            };
        }
    }
}