using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace UVL.Scalar.Rendering.Conversion
{
    public static class ScalarVolumeDataToScalarVolumeRenderConversion
    {
        public static AsyncResult<ScalarVolumeRender> RenderAsync (ScalarVolumeData data)
        {
            var result = new AsyncResult<ScalarVolumeRender>();
            AsyncFactory.Instance.StartCoroutine(Create(result, data));
            return result;
        }

        private static IEnumerator Create (AsyncResult<ScalarVolumeRender> result, ScalarVolumeData data)
        {
            result.Start();

            Texture3D texture = new Texture3D(data.voxelCount.x, data.voxelCount.y, data.voxelCount.z, TextureFormat.RFloat, false);
            Vector3 physicalSize = data.physicalSize;
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

            ScalarVolumeRender render = new ScalarVolumeRender()
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