using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace UVL.Scalar
{
    using UVL.IO;

    public class TextureToScalarTests
    {
        private readonly string DIRECTORY = ScalarTestData.ImageSequence.DIRECTORY;
        private readonly string FILEPATH = ScalarTestData.ImageSequence.FILEPATH;
        private readonly Vector3Int EXPECTED_VOXEL_COUNT = ScalarTestData.ImageSequence.EXPECTED_VOXEL_COUNT;
        private readonly float MAX_MS_PER_FRAME = ScalarTestData.MAX_MS_PER_FRAME;

        [UnityTest]
        public IEnumerator Texture2DToSliceScalarData()
        {
            AsyncResult<Texture2D> textureResult = TextureSliceInput.ReadAsync(FILEPATH);

            while (textureResult.inProgress)
            {
                yield return null;
                Assert.That(Time.deltaTime < MAX_MS_PER_FRAME, $"Frame time limit exceeded {Time.deltaTime}.");
            }

            AsyncResult<ScalarSliceData> sliceResult = TextureToScalarData.ToScalarSliceDataAsync(textureResult.Result);

            while (sliceResult.inProgress)
            {
                yield return null;
                Assert.That(Time.deltaTime < MAX_MS_PER_FRAME, $"Frame time limit exceeded {Time.deltaTime}.");
            }

            ScalarSliceData slice = sliceResult.Result;
            float maxValue = slice.values.Max();
            float minValue = slice.values.Min();

            Assert.That(slice != null, "Slice is null.");
            Assert.That(slice.pixelCount.x == EXPECTED_VOXEL_COUNT.x, $"Slice width ({slice.pixelCount.x}) did not match expected width ({EXPECTED_VOXEL_COUNT.x}).");
            Assert.That(slice.pixelCount.y == EXPECTED_VOXEL_COUNT.y, $"Slice height ({slice.pixelCount.y}) did not match expected height ({EXPECTED_VOXEL_COUNT.y}).");
            Assert.That(slice.values.Length > 0, "Slice contains no values.");
            Assert.That(!Mathf.Approximately(minValue, maxValue), "Slice contains completely uniform data.");
        }

        [UnityTest]
        public IEnumerator Texture2DArrayToStackScalarData()
        {
            AsyncResult<Texture2D[]> textureResult = TextureStackInput.ReadAsync(DIRECTORY);

            while (textureResult.inProgress)
            {
                yield return null;
                Assert.That(Time.deltaTime < MAX_MS_PER_FRAME, $"Frame time limit exceeded {Time.deltaTime}.");
            }

            AsyncResult<ScalarStackData> stackResult = TextureToScalarData.ToScalarStackDataAsync(textureResult.Result);

            while (stackResult.inProgress)
            {
                yield return null;
                Assert.That(Time.deltaTime < MAX_MS_PER_FRAME, $"Frame time limit exceeded {Time.deltaTime}.");
            }

            ScalarStackData stack = stackResult.Result;

            float[] maxValues = new float[stack.slices.Length];
            float[] minValues = new float[stack.slices.Length];
            for (int i = 0; i < stack.slices.Length; i++)
            {
                maxValues[i] = stack.slices[i].values.Max();
                minValues[i] = stack.slices[i].values.Min();
            }

            float maxValue = maxValues.Max();
            float minValue = minValues.Min();

            Assert.That(stack != null, "Stack is null.");
            Assert.That(stack.slices[0].pixelCount.x == EXPECTED_VOXEL_COUNT.x, $"Stack width ({stack.slices[0].pixelCount.x}) did not match expected width ({EXPECTED_VOXEL_COUNT.x}).");
            Assert.That(stack.slices[0].pixelCount.y == EXPECTED_VOXEL_COUNT.y, $"Stack height ({stack.slices[0].pixelCount.y}) did not match expected height ({EXPECTED_VOXEL_COUNT.y}).");
            Assert.That(stack.slices.Length == EXPECTED_VOXEL_COUNT.z, $"Stack depth ({stack.slices.Length}) did not match expected height ({EXPECTED_VOXEL_COUNT.z}).");
            Assert.That(stack.slices.Length > 0, "Stack contains no slices.");
            Assert.That(!Mathf.Approximately(minValue, maxValue), "Stack contains completely uniform data.");

            AsyncResult<ScalarVolumeData> volumeResult = TextureToScalarData.ToScalarVolumeDataAsync(textureResult.Result);

            while (volumeResult.inProgress)
            {
                yield return null;
                Assert.That(Time.deltaTime < MAX_MS_PER_FRAME, $"Frame time limit exceeded {Time.deltaTime}.");
            }
        }

        [UnityTest]
        public IEnumerator Texture3DToStackScalarData()
        {
            AsyncResult<Texture3D> textureResult = TextureVolumeInput.ReadAsync(DIRECTORY);

            while (textureResult.inProgress)
            {
                yield return null;
                Assert.That(Time.deltaTime < MAX_MS_PER_FRAME, $"Frame time limit exceeded {Time.deltaTime}.");
            }

            AsyncResult<ScalarStackData> stackResult = TextureToScalarData.ToScalarStackDataAsync(textureResult.Result);

            while (stackResult.inProgress)
            {
                yield return null;
                Assert.That(Time.deltaTime < MAX_MS_PER_FRAME, $"Frame time limit exceeded {Time.deltaTime}.");
            }

            ScalarStackData stack = stackResult.Result;

            float[] maxValues = new float[stack.slices.Length];
            float[] minValues = new float[stack.slices.Length];
            for (int i = 0; i < stack.slices.Length; i++)
            {
                maxValues[i] = stack.slices[i].values.Max();
                minValues[i] = stack.slices[i].values.Min();
            }

            float maxValue = maxValues.Max();
            float minValue = minValues.Min();

            Assert.That(stack != null, "Stack is null.");
            Assert.That(stack.slices[0].pixelCount.x == EXPECTED_VOXEL_COUNT.x, $"Stack width ({stack.slices[0].pixelCount.x}) did not match expected width ({EXPECTED_VOXEL_COUNT.x}).");
            Assert.That(stack.slices[0].pixelCount.y == EXPECTED_VOXEL_COUNT.y, $"Stack height ({stack.slices[0].pixelCount.y}) did not match expected height ({EXPECTED_VOXEL_COUNT.y}).");
            Assert.That(stack.slices.Length == EXPECTED_VOXEL_COUNT.z, $"Stack depth ({stack.slices.Length}) did not match expected height ({EXPECTED_VOXEL_COUNT.z}).");
            Assert.That(stack.slices.Length > 0, "Stack contains no slices.");
            Assert.That(!Mathf.Approximately(minValue, maxValue), "Stack contains completely uniform data.");

            AsyncResult<ScalarVolumeData> volumeResult = TextureToScalarData.ToScalarVolumeDataAsync(textureResult.Result);

            while (volumeResult.inProgress)
            {
                yield return null;
                Assert.That(Time.deltaTime < MAX_MS_PER_FRAME, $"Frame time limit exceeded {Time.deltaTime}.");
            }

            ScalarVolumeData volume = volumeResult.Result;

            int valuesPerSlice = EXPECTED_VOXEL_COUNT.x * EXPECTED_VOXEL_COUNT.y;
            float stackValue, volumeValue;
            for (int slice = 0; slice < stack.slices.Length; slice++)
            {
                for (int i = 0; i < stack.slices[slice].values.Length; i++)
                {
                    stackValue = stack.slices[slice].values[i];
                    volumeValue = volume.values[slice * valuesPerSlice + i];
                    Assert.That(Mathf.Approximately(stackValue, volumeValue));
                }
            }
        }

        [UnityTest]
        public IEnumerator Texture2DArrayToVolumeScalarData()
        {
            AsyncResult<Texture2D[]> textureResult = TextureStackInput.ReadAsync(DIRECTORY);

            while (textureResult.inProgress)
            {
                yield return null;
                Assert.That(Time.deltaTime < MAX_MS_PER_FRAME, $"Frame time limit exceeded {Time.deltaTime}.");
            }

            AsyncResult<ScalarVolumeData> volumeResult = TextureToScalarData.ToScalarVolumeDataAsync(textureResult.Result);

            while (volumeResult.inProgress)
            {
                yield return null;
                Assert.That(Time.deltaTime < MAX_MS_PER_FRAME, $"Frame time limit exceeded {Time.deltaTime}.");
            }

            ScalarVolumeData volume = volumeResult.Result;

            float maxValue = volume.values.Max();
            float minValue = volume.values.Min();

            Assert.That(volume != null, "Volume is null.");
            Assert.That(volume.voxelCount.x == EXPECTED_VOXEL_COUNT.x, $"Volume width ({volume.voxelCount.x}) did not match expected width ({EXPECTED_VOXEL_COUNT.x}).");
            Assert.That(volume.voxelCount.y == EXPECTED_VOXEL_COUNT.y, $"Volume height ({volume.voxelCount.y}) did not match expected height ({EXPECTED_VOXEL_COUNT.y}).");
            Assert.That(volume.voxelCount.z == EXPECTED_VOXEL_COUNT.z, $"Volume depth ({volume.voxelCount.z}) did not match expected height ({EXPECTED_VOXEL_COUNT.z}).");
            Assert.That(volume.values.Length > 0, "Volume contains no values.");
            Assert.That(!Mathf.Approximately(minValue, maxValue), "Volume contains completely uniform data.");

            AsyncResult<ScalarStackData> stackResult = TextureToScalarData.ToScalarStackDataAsync(textureResult.Result);

            while (stackResult.inProgress)
            {
                yield return null;
                Assert.That(Time.deltaTime < MAX_MS_PER_FRAME, $"Frame time limit exceeded {Time.deltaTime}.");
            }

            ScalarStackData stack = stackResult.Result;

            int valuesPerSlice = EXPECTED_VOXEL_COUNT.x * EXPECTED_VOXEL_COUNT.y;
            float stackValue, volumeValue;
            for (int slice = 0; slice < stack.slices.Length; slice++)
            {
                for (int i = 0; i < stack.slices[slice].values.Length; i++)
                {
                    stackValue = stack.slices[slice].values[i];
                    volumeValue = volume.values[slice * valuesPerSlice + i];
                    Assert.That(Mathf.Approximately(stackValue, volumeValue));
                }
            }
        }

        [UnityTest]
        public IEnumerator TextureVolumeToVolumeScalarData()
        {
            AsyncResult<Texture3D> textureResult = TextureVolumeInput.ReadAsync(DIRECTORY);

            while (textureResult.inProgress)
            {
                yield return null;
                Assert.That(Time.deltaTime < MAX_MS_PER_FRAME, $"Frame time limit exceeded {Time.deltaTime}.");
            }

            AsyncResult<ScalarVolumeData> volumeResult = TextureToScalarData.ToScalarVolumeDataAsync(textureResult.Result);

            while (volumeResult.inProgress)
            {
                yield return null;
                Assert.That(Time.deltaTime < MAX_MS_PER_FRAME, $"Frame time limit exceeded {Time.deltaTime}.");
            }

            ScalarVolumeData volume = volumeResult.Result;

            float maxValue = volume.values.Max();
            float minValue = volume.values.Min();

            Assert.That(volume != null, "Volume is null.");
            Assert.That(volume.voxelCount.x == EXPECTED_VOXEL_COUNT.x, $"Volume width ({volume.voxelCount.x}) did not match expected width ({EXPECTED_VOXEL_COUNT.x}).");
            Assert.That(volume.voxelCount.y == EXPECTED_VOXEL_COUNT.y, $"Volume height ({volume.voxelCount.y}) did not match expected height ({EXPECTED_VOXEL_COUNT.y}).");
            Assert.That(volume.voxelCount.z == EXPECTED_VOXEL_COUNT.z, $"Volume depth ({volume.voxelCount.z}) did not match expected height ({EXPECTED_VOXEL_COUNT.z}).");
            Assert.That(volume.values.Length > 0, "Volume contains no values.");
            Assert.That(!Mathf.Approximately(minValue, maxValue), "Volume contains completely uniform data.");
        }
    }
}