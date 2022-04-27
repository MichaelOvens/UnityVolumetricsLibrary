using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace UVL.Scalar.Conversion
{
    public static class Texture2DArrayToScalarVolumeData
    {
        public static AsyncResult<ScalarVolumeData> ConvertAsync(Texture2D[] textures)
        {
            var result = new AsyncResult<ScalarVolumeData>();
            AsyncFactory.Instance.StartCoroutine(Create(result, textures));
            return result;
        }
        private static IEnumerator Create(AsyncResult<ScalarVolumeData> result, Texture2D[] textures)
        {
            result.Start();

            Vector3Int voxelCount = new Vector3Int(textures[0].width, textures[0].height, textures.Length);
            Vector3 physicalSize = new Vector3(textures[0].width, textures[0].height, textures.Length);
            float[] values = new float[voxelCount.x * voxelCount.y * voxelCount.z];

            if (TextureToScalarData.StackTextureDimensionsAreNotUniform(textures, voxelCount))
            {
                result.Throw(new DataMisalignedException("Texture stack has non-uniform dimensions"));
                yield break;
            }

            Color[][] slicePixels = new Color[textures.Length][];
            for (int i = 0; i < slicePixels.Length; i++)
                slicePixels[i] = textures[i].GetPixels();

            // Start this parallel operation in a task so it doesn't lock the main thread
            Task task = Task.Run(() =>
            {
                int sliceValuesLength = voxelCount.x * voxelCount.y;
                Parallel.For(0, slicePixels.Length, i =>
                {
                    for (int input = 0, output = i * sliceValuesLength; input < sliceValuesLength; input++, output++)
                        values[output] = TextureToScalarData.ColorToScalar(slicePixels[i][input]);
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
    }
}