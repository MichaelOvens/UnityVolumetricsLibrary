using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace UVL.Scalar.Rendering.Conversion
{
    public static class ScalarSliceDataToScalarSliceRenderConversion
    {
        public static AsyncResult<ScalarSliceRender> RenderAsync (ScalarSliceData data)
        {
            var result = new AsyncResult<ScalarSliceRender>();
            AsyncFactory.Instance.StartCoroutine(Create(result, data));
            return result;
        }

        private static IEnumerator Create (AsyncResult<ScalarSliceRender> result, ScalarSliceData data)
        {
            result.Start();

            Texture2D texture = new Texture2D(data.pixelCount.x, data.pixelCount.y, TextureFormat.RFloat, false);
            Vector2 physicalSize = data.physicalSize;
            float minValue = data.values.Min();
            float maxValue = data.values.Max();

            Color[] colors = new Color[data.values.Length];

            // Start this parallel operation in a task so it doesn't lock the main thread
            Task task = Task.Run(() =>
            {
                Parallel.For(0, colors.Length, i =>
                {
                    colors[i] = ScalarDataToScalarRenderConversion.ScalarToColor(data.values[i]);
                });
            });

            while (!task.IsCompleted)
                yield return null;

            texture.SetPixels(colors);
            texture.Apply();

            ScalarSliceRender render = new ScalarSliceRender()
            {
                texture = texture,
                physicalSize = physicalSize,
                minValue = minValue,
                maxValue = maxValue
            };

            result.Complete(render);
        }
    }
}