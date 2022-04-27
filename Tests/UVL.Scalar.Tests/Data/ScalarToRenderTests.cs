using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;


namespace UVL.Scalar.Rendering
{
    using UVL.IO;

    public class ScalarToRenderTests
    {
        private readonly string DIRECTORY = ScalarTestData.ImageSequence.DIRECTORY;
        private readonly string FILEPATH = ScalarTestData.ImageSequence.FILEPATH;
        private readonly Vector3Int EXPECTED_VOXEL_COUNT = ScalarTestData.ImageSequence.EXPECTED_VOXEL_COUNT;
        private readonly float MAX_MS_PER_FRAME = ScalarTestData.MAX_MS_PER_FRAME;

        [UnityTest]
        public IEnumerator SliceScalarToSliceRender()
        {
            AsyncResult<Texture2D> textureResult = TextureSliceInput.ReadAsync(FILEPATH);

            while (textureResult.inProgress)
            {
                yield return null;
                Assert.That(Time.deltaTime < MAX_MS_PER_FRAME, $"Frame time limit exceeded {Time.deltaTime}.");
            }

            AsyncResult<ScalarSliceData> dataResult = TextureToScalarData.ToScalarSliceDataAsync(textureResult.Result);

            while (dataResult.inProgress)
            {
                yield return null;
                Assert.That(Time.deltaTime < MAX_MS_PER_FRAME, $"Frame time limit exceeded {Time.deltaTime}.");
            }

            AsyncResult<ScalarSliceRender> sliceResult = ScalarDataToScalarRender.ToScalarSliceRenderAsync(dataResult.Result);

            while (sliceResult.inProgress)
            {
                yield return null;
                Assert.That(Time.deltaTime < MAX_MS_PER_FRAME, $"Frame time limit exceeded {Time.deltaTime}.");
            }

            var render = sliceResult.Result;
            Color[] sliceColors = sliceResult.Result.texture.GetPixels();
            float maxValue = sliceColors.Max(x => x.r);
            float minValue = sliceColors.Min(x => x.r);

            Assert.That(render != null, "Render is null.");
            Assert.That(render.texture.width == EXPECTED_VOXEL_COUNT.x, $"Render width ({render.texture.width}) did not match expected width ({EXPECTED_VOXEL_COUNT.x}).");
            Assert.That(render.texture.height == EXPECTED_VOXEL_COUNT.y, $"Render height ({render.texture.height}) did not match expected height ({EXPECTED_VOXEL_COUNT.y}).");
            Assert.That(sliceColors.Length > 0, "Render contains no values.");
            Assert.That(!Mathf.Approximately(minValue, maxValue), "Render contains completely uniform data.");
        }

        [UnityTest]
        public IEnumerator StackScalarToStackRender()
        {
            AsyncResult<Texture2D[]> textureResult = TextureStackInput.ReadAsync(DIRECTORY);

            while (textureResult.inProgress)
            {
                yield return null;
                Assert.That(Time.deltaTime < MAX_MS_PER_FRAME, $"Frame time limit exceeded {Time.deltaTime}.");
            }

            AsyncResult<ScalarStackData> dataResult = TextureToScalarData.ToScalarStackDataAsync(textureResult.Result);

            while (dataResult.inProgress)
            {
                yield return null;
                Assert.That(Time.deltaTime < MAX_MS_PER_FRAME, $"Frame time limit exceeded {Time.deltaTime}.");
            }

            AsyncResult<ScalarStackRender> stackResult = ScalarDataToScalarRender.ToScalarStackRenderAsync(dataResult.Result);

            while (stackResult.inProgress)
            {
                yield return null;
                Assert.That(Time.deltaTime < MAX_MS_PER_FRAME, $"Frame time limit exceeded {Time.deltaTime}.");
            }

            var render = stackResult.Result;

            Color[][] stackColors = new Color[render.slices.Length][];
            float[] maxValues = new float[render.slices.Length];
            float[] minValues = new float[render.slices.Length];

            for (int i = 0; i < stackColors.Length; i++)
            {
                stackColors[i] = render.slices[i].texture.GetPixels();
                maxValues[i] = stackColors[i].Max(x => x.r);
                minValues[i] = stackColors[i].Min(x => x.r);
            }

            float maxValue = maxValues.Max();
            float minValue = minValues.Min();

            Assert.That(render != null, "Render is null.");
            Assert.That(render.slices[0].texture.width == EXPECTED_VOXEL_COUNT.x, $"Render width ({render.slices[0].texture.width}) did not match expected width ({EXPECTED_VOXEL_COUNT.x}).");
            Assert.That(render.slices[0].texture.height == EXPECTED_VOXEL_COUNT.y, $"Render height ({render.slices[0].texture.height}) did not match expected height ({EXPECTED_VOXEL_COUNT.y}).");
            Assert.That(render.slices.Length == EXPECTED_VOXEL_COUNT.z, $"Render depth ({render.slices.Length}) did not match expected height ({EXPECTED_VOXEL_COUNT.z})."); 
            Assert.That(stackColors.Length > 0, "Render contains no values.");
            Assert.That(!Mathf.Approximately(minValue, maxValue), "Render contains completely uniform data.");
        }

        [UnityTest]
        public IEnumerator VolumeScalarToStackRender()
        {
            AsyncResult<Texture3D> textureResult = TextureVolumeInput.ReadAsync(DIRECTORY);

            while (textureResult.inProgress)
            {
                yield return null;
                Assert.That(Time.deltaTime < MAX_MS_PER_FRAME, $"Frame time limit exceeded {Time.deltaTime}.");
            }

            AsyncResult<ScalarVolumeData> dataResult = TextureToScalarData.ToScalarVolumeDataAsync(textureResult.Result);

            while (dataResult.inProgress)
            {
                yield return null;
                Assert.That(Time.deltaTime < MAX_MS_PER_FRAME, $"Frame time limit exceeded {Time.deltaTime}.");
            }

            AsyncResult<ScalarStackRender> renderResult = ScalarDataToScalarRender.ToScalarStackRenderAsync(dataResult.Result);

            while (renderResult.inProgress)
            {
                yield return null;
                Assert.That(Time.deltaTime < MAX_MS_PER_FRAME, $"Frame time limit exceeded {Time.deltaTime}.");
            }

            ScalarStackRender stack = renderResult.Result;

            Color[][] stackColors = new Color[stack.slices.Length][];
            float[] maxValues = new float[stack.slices.Length];
            float[] minValues = new float[stack.slices.Length];

            for (int i = 0; i < stackColors.Length; i++)
            {
                stackColors[i] = stack.slices[i].texture.GetPixels();
                maxValues[i] = stackColors[i].Max(x => x.r);
                minValues[i] = stackColors[i].Min(x => x.r);
            }

            float maxValue = maxValues.Max();
            float minValue = minValues.Min();

            Assert.That(stack != null, "Render is null.");
            Assert.That(stack.slices[0].texture.width == EXPECTED_VOXEL_COUNT.x, $"Render width ({stack.slices[0].texture.width}) did not match expected width ({EXPECTED_VOXEL_COUNT.x}).");
            Assert.That(stack.slices[0].texture.height == EXPECTED_VOXEL_COUNT.y, $"Render height ({stack.slices[0].texture.height}) did not match expected height ({EXPECTED_VOXEL_COUNT.y}).");
            Assert.That(stack.slices.Length == EXPECTED_VOXEL_COUNT.z, $"Render depth ({stack.slices.Length}) did not match expected height ({EXPECTED_VOXEL_COUNT.z}).");
            Assert.That(stackColors.Length > 0, "Render contains no values.");
            Assert.That(!Mathf.Approximately(minValue, maxValue), "Render contains completely uniform data.");

            AsyncResult<ScalarVolumeRender> volumeResult = ScalarDataToScalarRender.ToScalarVolumeRenderAsync(dataResult.Result);

            while (volumeResult.inProgress)
            {
                yield return null;
                Assert.That(Time.deltaTime < MAX_MS_PER_FRAME, $"Frame time limit exceeded {Time.deltaTime}.");
            }

            ScalarVolumeRender volume = volumeResult.Result;
            Color[] volumeColors = volume.texture.GetPixels();

            int valuesPerSlice = EXPECTED_VOXEL_COUNT.x * EXPECTED_VOXEL_COUNT.y;
            Color stackValue, volumeValue;
            for (int slice = 0; slice < stackColors.Length; slice++)
            {
                for (int i = 0; i < stackColors[slice].Length; i++)
                {
                    stackValue = stackColors[slice][i];
                    volumeValue = volumeColors[slice * valuesPerSlice + i];
                    Assert.That(Mathf.Approximately(stackValue.r, volumeValue.r));
                }
            }
        }

        [UnityTest]
        public IEnumerator StackScalarToVolumeRender()
        {
            AsyncResult<Texture2D[]> textureResult = TextureStackInput.ReadAsync(DIRECTORY);

            while (textureResult.inProgress)
            {
                yield return null;
                Assert.That(Time.deltaTime < MAX_MS_PER_FRAME, $"Frame time limit exceeded {Time.deltaTime}.");
            }

            AsyncResult<ScalarStackData> dataResult = TextureToScalarData.ToScalarStackDataAsync(textureResult.Result);

            while (dataResult.inProgress)
            {
                yield return null;
                Assert.That(Time.deltaTime < MAX_MS_PER_FRAME, $"Frame time limit exceeded {Time.deltaTime}.");
            }

            AsyncResult<ScalarVolumeRender> volumeResult = ScalarDataToScalarRender.ToScalarVolumeRenderAsync(dataResult.Result);

            while (volumeResult.inProgress)
            {
                yield return null;
                Assert.That(Time.deltaTime < MAX_MS_PER_FRAME, $"Frame time limit exceeded {Time.deltaTime}.");
            }

            ScalarVolumeRender volume = volumeResult.Result;
            Color[] volumeColors = volumeResult.Result.texture.GetPixels();
            float maxValue = volumeColors.Max(x => x.r);
            float minValue = volumeColors.Min(x => x.r);

            Assert.That(volume != null, "Render is null.");
            Assert.That(volume.texture.width == EXPECTED_VOXEL_COUNT.x, $"Render width ({volume.texture.width}) did not match expected width ({EXPECTED_VOXEL_COUNT.x}).");
            Assert.That(volume.texture.height == EXPECTED_VOXEL_COUNT.y, $"Render height ({volume.texture.height}) did not match expected height ({EXPECTED_VOXEL_COUNT.y}).");
            Assert.That(volume.texture.depth == EXPECTED_VOXEL_COUNT.z, $"Render depth ({volume.texture.depth}) did not match expected height ({EXPECTED_VOXEL_COUNT.z}).");
            Assert.That(volumeColors.Length > 0, "Render contains no values.");
            Assert.That(!Mathf.Approximately(minValue, maxValue), "Render contains completely uniform data.");

            AsyncResult<ScalarStackRender> stackResult = ScalarDataToScalarRender.ToScalarStackRenderAsync(dataResult.Result);

            while (stackResult.inProgress)
            {
                yield return null;
                Assert.That(Time.deltaTime < MAX_MS_PER_FRAME, $"Frame time limit exceeded {Time.deltaTime}.");
            }

            ScalarStackRender stack = stackResult.Result;

            Color[][] stackColors = new Color[stack.slices.Length][];
            for (int i = 0; i < stackColors.Length; i++)
                stackColors[i] = stack.slices[i].texture.GetPixels();

            int valuesPerSlice = EXPECTED_VOXEL_COUNT.x * EXPECTED_VOXEL_COUNT.y;
            Color stackValue, volumeValue;
            for (int slice = 0; slice < stackColors.Length; slice++)
            {
                for (int i = 0; i < stackColors[slice].Length; i++)
                {
                    stackValue = stackColors[slice][i];
                    volumeValue = volumeColors[slice * valuesPerSlice + i];
                    Assert.That(Mathf.Approximately(stackValue.r, volumeValue.r));
                }
            }
        }

        [UnityTest]
        public IEnumerator VolumeScalarToVolumeRender()
        {
            AsyncResult<Texture3D> textureResult = TextureVolumeInput.ReadAsync(DIRECTORY);

            while (textureResult.inProgress)
            {
                yield return null;
                Assert.That(Time.deltaTime < MAX_MS_PER_FRAME, $"Frame time limit exceeded {Time.deltaTime}.");
            }

            AsyncResult<ScalarVolumeData> dataResult = TextureToScalarData.ToScalarVolumeDataAsync(textureResult.Result);

            while (dataResult.inProgress)
            {
                yield return null;
                Assert.That(Time.deltaTime < MAX_MS_PER_FRAME, $"Frame time limit exceeded {Time.deltaTime}.");
            }

            AsyncResult<ScalarVolumeRender> volumeResult = ScalarDataToScalarRender.ToScalarVolumeRenderAsync(dataResult.Result);

            while (volumeResult.inProgress)
            {
                yield return null;
                Assert.That(Time.deltaTime < MAX_MS_PER_FRAME, $"Frame time limit exceeded {Time.deltaTime}.");
            }

            ScalarVolumeRender volume = volumeResult.Result;
            Color[] volumeColors = volumeResult.Result.texture.GetPixels();
            float maxValue = volumeColors.Max(x => x.r);
            float minValue = volumeColors.Min(x => x.r);

            Assert.That(volume != null, "Render is null.");
            Assert.That(volume.texture.width == EXPECTED_VOXEL_COUNT.x, $"Render width ({volume.texture.width}) did not match expected width ({EXPECTED_VOXEL_COUNT.x}).");
            Assert.That(volume.texture.height == EXPECTED_VOXEL_COUNT.y, $"Render height ({volume.texture.height}) did not match expected height ({EXPECTED_VOXEL_COUNT.y}).");
            Assert.That(volume.texture.depth == EXPECTED_VOXEL_COUNT.z, $"Render depth ({volume.texture.depth}) did not match expected height ({EXPECTED_VOXEL_COUNT.z}).");
            Assert.That(volumeColors.Length > 0, "Render contains no values.");
            Assert.That(!Mathf.Approximately(minValue, maxValue), "Render contains completely uniform data.");
        }
    }
}