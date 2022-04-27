using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace UVL.Scalar.Conversion
{
    public static class Texture3DToScalarVolumeData
    {
        public static AsyncResult<ScalarVolumeData> ConvertAsync(Texture3D texture)
        {
            var result = new AsyncResult<ScalarVolumeData>();
            AsyncFactory.Instance.StartCoroutine(Create(result, texture));
            return result;
        }
       
        private static IEnumerator Create(AsyncResult<ScalarVolumeData> result, Texture3D texture)
        {
            result.Start();

            Vector3Int voxelCount = new Vector3Int(texture.width, texture.height, texture.depth);
            Vector3 physicalSize = new Vector3(texture.width, texture.height, texture.depth);
            float[] values = new float[voxelCount.x * voxelCount.y * voxelCount.z];

            Color[] colors = texture.GetPixels();

            // Start this parallel operation in a task so it doesn't lock the main thread
            Task task = Task.Run(() =>
            {
                Parallel.For(0, colors.Length, i =>
                {
                    values[i] = TextureToScalarData.ColorToScalar(colors[i]);
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