using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace UVL.Scalar.Conversion
{
    public static class Texture3DToScalarStackData
    {
        public static AsyncResult<ScalarStackData> ConvertAsync(Texture3D texture)
        {
            var result = new AsyncResult<ScalarStackData>();
            AsyncFactory.Instance.StartCoroutine(Create(result, texture));
            return result;
        }

        private static IEnumerator Create(AsyncResult<ScalarStackData> result, Texture3D texture)
        {
            result.Start();

            ScalarSliceData[] slices = new ScalarSliceData[texture.depth];

            Vector2Int pixelCount = new Vector2Int(texture.width, texture.height);
            Vector2 physicalSize = new Vector3(texture.width, texture.height);

            Color[] colors = texture.GetPixels();
            int sliceValuesLength = texture.width * texture.height;

            // Start this parallel operation in a task so it doesn't lock the main thread
            Task task = Task.Run(() =>
            {
                Parallel.For(0, slices.Length, i =>
                {
                    float[] values = new float[sliceValuesLength];
                    for (int v = 0, c = i * sliceValuesLength; v < sliceValuesLength; v++, c++)
                        values[v] = TextureToScalarData.ColorToScalar(colors[c]);

                    slices[i] = new ScalarSliceData()
                    {
                        pixelCount = pixelCount,
                        physicalSize = physicalSize,
                        values = values
                    };
                });
            });

            while (!task.IsCompleted)
                yield return null;

            ScalarStackData stackData = new ScalarStackData()
            {
                slices = slices
            };

            result.Complete(stackData);
        }
    }
}