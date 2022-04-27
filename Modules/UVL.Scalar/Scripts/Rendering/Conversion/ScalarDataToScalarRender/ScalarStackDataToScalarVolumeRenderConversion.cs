using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace UVL.Scalar.Rendering.Conversion
{
    public static class ScalarStackDataToScalarVolumeRenderConversion
    {
        public static AsyncResult<ScalarVolumeRender> RenderAsync(ScalarStackData data)
        {
            var result = new AsyncResult<ScalarVolumeRender>();
            AsyncFactory.Instance.StartCoroutine(Create(result, data));
            return result;
        }

        private static IEnumerator Create(AsyncResult<ScalarVolumeRender> result, ScalarStackData data)
        {
            result.Start();

            Vector3Int voxelCount = new Vector3Int(data.slices[0].pixelCount.x, data.slices[0].pixelCount.y, data.slices.Length);
            Vector3 physicalSize = new Vector3(data.slices[0].physicalSize.x, data.slices[0].physicalSize.y, data.slices.Length);

            if (ScalarDataToScalarRenderConversion.StackTextureDimensionsAreNotUniform(data, voxelCount))
            {
                result.Throw(new DataMisalignedException("Texture stack has non-uniform dimensions"));
                yield break;
            }

            float minValue = data.slices.Min(slice => slice.values.Min());
            float maxValue = data.slices.Max(slice => slice.values.Max());

            Color[] colors = new Color[voxelCount.x * voxelCount.y * voxelCount.z];
            int sliceValuesLength = voxelCount.x * voxelCount.y;

            // Start this parallel operation in a task so it doesn't lock the main thread
            Task task = Task.Run(() =>
            {
                Parallel.For(0, data.slices.Length, i =>
                {
                    for (int input = 0, output = i * sliceValuesLength; input < sliceValuesLength; input++, output++)
                        colors[output] = ScalarDataToScalarRenderConversion.ScalarToColor(data.slices[i].values[input]);
                });
            });

            while (!task.IsCompleted)
                yield return null;

            Texture3D texture = new Texture3D(voxelCount.x, voxelCount.y, voxelCount.z, TextureFormat.RFloat, false);
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