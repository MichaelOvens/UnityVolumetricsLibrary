using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace UVL.IO
{
    public class TextureInputTests
    {
        private readonly string DIRECTORY = CoreTestData.ImageSequence.DIRECTORY;
        private readonly string FILEPATH = CoreTestData.ImageSequence.FILEPATH;
        private readonly Vector3Int EXPECTED_VOXEL_COUNT = CoreTestData.ImageSequence.EXPECTED_VOXEL_COUNT;
        private readonly float MAX_MS_PER_FRAME = CoreTestData.MAX_MS_PER_FRAME;

        [UnityTest]
        public IEnumerator ReadImageAsTexture2D()
        {
            var sliceResult = TextureSliceInput.ReadAsync(FILEPATH);

            while (sliceResult.inProgress)
            {
                yield return null;
                Assert.That(Time.deltaTime < MAX_MS_PER_FRAME, $"Frame time limit exceeded {Time.deltaTime}.");
            }

            Texture2D sliceTexture = sliceResult.Result;
            Color[] pixels = sliceTexture.GetPixels();
            float maxLightness = pixels.Max(x => x.r + x.g + x.b);
            float minLightness = pixels.Min(x => x.r + x.g + x.b);

            Assert.That(sliceTexture != null, "Slice texture is null.");
            Assert.That(sliceTexture.width == EXPECTED_VOXEL_COUNT.x, $"Slice width ({sliceTexture.width}) did not match expected width ({EXPECTED_VOXEL_COUNT.x}).");
            Assert.That(sliceTexture.height == EXPECTED_VOXEL_COUNT.y, $"Slice height ({sliceTexture.height}) did not match expected height ({EXPECTED_VOXEL_COUNT.y}).");
            Assert.That(pixels.Length > 0, "Slice contains no pixel data.");
            Assert.That(!Mathf.Approximately(minLightness, maxLightness), "Slice contains completely uniform data.");
        }

        [UnityTest]
        public IEnumerator ReadImageSequenceAsTexture2DArray()
        {
            AsyncResult<Texture2D[]> stackResult = TextureStackInput.ReadAsync(DIRECTORY);

            while (stackResult.inProgress)
            {
                yield return null;
                Assert.That(Time.deltaTime < MAX_MS_PER_FRAME, $"Frame time limit exceeded {Time.deltaTime}.");
            }

            Texture2D[] stackTextures = stackResult.Result;
            Color[] pixels = stackTextures[stackTextures.Length / 2].GetPixels();
            float maxLightness = pixels.Max(x => x.r + x.g + x.b);
            float minLightness = pixels.Min(x => x.r + x.g + x.b);

            Assert.That(stackTextures != null, "Stack is null.");
            Assert.That(stackTextures[0].width == EXPECTED_VOXEL_COUNT.x, $"Stack width ({stackTextures[0].width}) did not match expected width ({EXPECTED_VOXEL_COUNT.x}).");
            Assert.That(stackTextures[0].height == EXPECTED_VOXEL_COUNT.y, $"Stack height ({stackTextures[0].height}) did not match expected height ({EXPECTED_VOXEL_COUNT.y}).");
            Assert.That(stackTextures.Length == EXPECTED_VOXEL_COUNT.z, $"Stack depth ({stackTextures.Length}) did not match expected height ({EXPECTED_VOXEL_COUNT.z}).");
            Assert.That(!Mathf.Approximately(minLightness, maxLightness), "Stack contains completely uniform data.");
        }

        [UnityTest]
        public IEnumerator ReadImageSequenceAsTexture3D()
        {
            AsyncResult<Texture3D> volumeResult = TextureVolumeInput.ReadAsync(DIRECTORY);

            while (volumeResult.inProgress)
            {
                yield return null;
                Assert.That(Time.deltaTime < MAX_MS_PER_FRAME, $"Frame time limit exceeded {Time.deltaTime}.");
            }

            Texture3D volumeTexture = volumeResult.Result;

            Color[] pixels = volumeTexture.GetPixels();
            float maxLightness = pixels.Max(x => x.r + x.g + x.b);
            float minLightness = pixels.Min(x => x.r + x.g + x.b);

            Assert.That(volumeTexture != null, "Volume is null.");
            Assert.That(volumeTexture.width == EXPECTED_VOXEL_COUNT.x, $"Volume width ({volumeTexture.width}) did not match expected width ({EXPECTED_VOXEL_COUNT.x}).");
            Assert.That(volumeTexture.height == EXPECTED_VOXEL_COUNT.y, $"Volume height ({volumeTexture.height}) did not match expected height ({EXPECTED_VOXEL_COUNT.y}).");
            Assert.That(volumeTexture.depth == EXPECTED_VOXEL_COUNT.z, $"Volume depth ({volumeTexture.depth}) did not match expected depth ({EXPECTED_VOXEL_COUNT.z}");
            Assert.That(!Mathf.Approximately(minLightness, maxLightness), "Stack contains completely uniform data.");
        }
    }
}