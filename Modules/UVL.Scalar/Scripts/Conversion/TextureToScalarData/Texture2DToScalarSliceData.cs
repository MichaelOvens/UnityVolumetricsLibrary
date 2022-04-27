using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace UVL.Scalar.Conversion
{
    public static class Texture2DToScalarSliceData
    {
        public static AsyncResult<ScalarSliceData> ConvertAsync (Texture2D texture)
        {
            var result = new AsyncResult<ScalarSliceData>();
            AsyncFactory.Instance.StartCoroutine(Create(result, texture));
            return result;
        }

        private static IEnumerator Create (AsyncResult<ScalarSliceData> result, Texture2D texture)
        {
            result.Start();

            Vector2Int pixelCount = new Vector2Int(texture.width, texture.height);
            Vector2 physicalSize = new Vector2(texture.width, texture.height);
            float[] values = new float[pixelCount.x * pixelCount.y];

            Color[] colors = texture.GetPixels();

            // Start this parallel operation in a task so it doesn't lock the main thread
            Task task = Task.Run(() => {
                Parallel.For(0, values.Length, i =>
                {
                    values[i] = TextureToScalarData.ColorToScalar(colors[i]);
                }); 
            }); ;

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