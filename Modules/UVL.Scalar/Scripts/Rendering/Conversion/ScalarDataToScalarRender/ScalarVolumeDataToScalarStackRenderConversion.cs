using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace UVL.Scalar.Rendering.Conversion
{
    public static class ScalarVolumeDataToScalarStackRenderConversion
    {
        public static AsyncResult<ScalarStackRender> RenderAsync(ScalarVolumeData data)
        {
            var result = new AsyncResult<ScalarStackRender>();
            AsyncFactory.Instance.StartCoroutine(Create(result, data));
            return result;
        }

        private static IEnumerator Create(AsyncResult<ScalarStackRender> result, ScalarVolumeData data)
        {
            result.Start();

            ScalarSliceRender[] slices = new ScalarSliceRender[data.voxelCount.z];

            Color[][] slicePixels = new Color[data.voxelCount.z][];
            int sliceValuesLength = data.voxelCount.x * data.voxelCount.y;

            // Start this parallel operation in a task so it doesn't lock the main thread
            Task task = Task.Run(() =>
            {
                Parallel.For(0, data.voxelCount.z, i =>
                {
                    slicePixels[i] = new Color[sliceValuesLength];

                    float minValue = Mathf.Infinity;
                    float maxValue = Mathf.NegativeInfinity;
                    
                    for (int input = i * sliceValuesLength, output = 0; output < sliceValuesLength; input++, output++)
                    {
                        slicePixels[i][output] = ScalarDataToScalarRenderConversion.ScalarToColor(data.values[input]);
                        minValue = Mathf.Min(data.values[input], minValue);
                        maxValue = Mathf.Max(data.values[input], maxValue);
                    }

                    slices[i] = new ScalarSliceRender()
                    {
                        texture = null,
                        physicalSize = new Vector2(data.voxelCount.x, data.voxelCount.y),
                        minValue = minValue,
                        maxValue = maxValue
                    };
                });
            });

            while (!task.IsCompleted)
                yield return null;

            // Unity requires that creation of textures be done on the main thread
            DateTime lastFrame = DateTime.Now;
            for (int i = 0; i < slices.Length; i++)
            {
                slices[i].texture = new Texture2D(data.voxelCount.x, data.voxelCount.y, TextureFormat.RFloat, false);
                slices[i].texture.SetPixels(slicePixels[i]);
                slices[i].texture.Apply();

                if (Async.FrameLimitExceeded(lastFrame))
                {
                    yield return null;
                    lastFrame = DateTime.Now;
                }
            }

            float minValue = data.values.Min();
            float maxValue = data.values.Max();

            ScalarStackRender render = new ScalarStackRender()
            {
                slices = slices,
                minValue = minValue,
                maxValue = maxValue
            };

            result.Complete(render);
        }
    }
}