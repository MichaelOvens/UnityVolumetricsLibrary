using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

using FellowOakDicom;

namespace UVL.Dicom
{
    using UVL.Dicom.IO;
    using UVL.Scalar;

    public class DicomToScalarDataTests
    {
        private readonly string DIRECTORY = DicomTestData.DicomSeries.DIRECTORY;
        private readonly string FILEPATH = DicomTestData.DicomSeries.FILEPATH;
        private static readonly Vector3Int EXPECTED_VOXEL_COUNT = DicomTestData.DicomSeries.EXPECTED_VOXEL_COUNT;
        private static readonly Vector3 EXPECTED_PHYSICAL_SIZE = DicomTestData.DicomSeries.EXPECTED_PHYSICAL_SIZE;
        private const float MAX_MS_PER_FRAME = DicomTestData.MAX_MS_PER_FRAME;

        [UnityTest]
        public IEnumerator DicomFileConvertsToScalarDataSlice()
        {
            AsyncResult<DicomFile> dicomResult = DicomFileInput.ReadAsync(FILEPATH);

            while (dicomResult.inProgress)
            {
                yield return null;
                Assert.That(Time.deltaTime <= MAX_MS_PER_FRAME);
            }

            AsyncResult<ScalarSliceData> sliceResult = DicomFileToScalarData.ToScalarSliceDataAsync(dicomResult.Result);

            while (sliceResult.inProgress)
            {
                yield return null;
                Assert.That(Time.deltaTime <= MAX_MS_PER_FRAME);
            }

            ScalarSliceData sliceData = sliceResult.Result;

            float maxValue = sliceData.values.Max();
            float minValue = sliceData.values.Min();

            Assert.That(sliceData != null);
            Assert.That(sliceData.pixelCount.x == EXPECTED_VOXEL_COUNT.x);
            Assert.That(sliceData.pixelCount.y == EXPECTED_VOXEL_COUNT.y);
            Assert.That(sliceData.physicalSize.x == EXPECTED_PHYSICAL_SIZE.x);
            Assert.That(sliceData.physicalSize.y == EXPECTED_PHYSICAL_SIZE.y);
            Assert.That(!Mathf.Approximately(minValue, maxValue));
        }

        [UnityTest]
        public IEnumerator DicomFilesConvertToScalarDataStack()
        {
            AsyncResult<DicomFile[]> dicomResult = DicomDirectoryInput.ReadAsync(DIRECTORY);

            while (dicomResult.inProgress)
            {
                yield return null;
                Assert.That(Time.deltaTime <= MAX_MS_PER_FRAME);
            }

            AsyncResult<ScalarStackData> stackResult = DicomFileToScalarData.ToScalarStackDataAsync(dicomResult.Result);

            while (stackResult.inProgress)
            {
                yield return null;
                Assert.That(Time.deltaTime <= MAX_MS_PER_FRAME);
            }

            ScalarStackData stackData = stackResult.Result;

            Assert.That(stackData != null);
            Assert.That(stackData.slices.Length == EXPECTED_VOXEL_COUNT.z);
        }

        [UnityTest]
        public IEnumerator DicomFilesConvertToScalarVolumeStack()
        {
            AsyncResult<DicomFile[]> dicomResult = DicomDirectoryInput.ReadAsync(DIRECTORY);

            while (dicomResult.inProgress)
            {
                yield return null;
                Assert.That(Time.deltaTime <= MAX_MS_PER_FRAME);
            }

            AsyncResult<ScalarVolumeData> volumeResult = DicomFileToScalarData.ToScalarVolumeDataAsync(dicomResult.Result);

            while (volumeResult.inProgress)
            {
                yield return null;
                Assert.That(Time.deltaTime <= MAX_MS_PER_FRAME);
            }

            ScalarVolumeData volumeData = volumeResult.Result;

            float maxValue = volumeData.values.Max();
            float minValue = volumeData.values.Min();

            Assert.That(volumeData != null);
            Assert.That(volumeData.voxelCount.x == EXPECTED_VOXEL_COUNT.x);
            Assert.That(volumeData.voxelCount.y == EXPECTED_VOXEL_COUNT.y);
            Assert.That(volumeData.voxelCount.z == EXPECTED_VOXEL_COUNT.z);
            Assert.That(volumeData.physicalSize.x == EXPECTED_PHYSICAL_SIZE.x);
            Assert.That(volumeData.physicalSize.y == EXPECTED_PHYSICAL_SIZE.y);
            Assert.That(volumeData.physicalSize.z == EXPECTED_PHYSICAL_SIZE.z);
            Assert.That(!Mathf.Approximately(minValue, maxValue));
        }
    }
}