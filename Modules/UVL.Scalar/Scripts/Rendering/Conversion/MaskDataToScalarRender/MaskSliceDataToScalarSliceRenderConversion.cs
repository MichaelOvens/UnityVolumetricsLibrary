using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace UVL.Scalar.Rendering.Conversion
{
    public static class MaskSliceDataToScalarSliceRenderConversion
    {
        public static AsyncResult<ScalarSliceRender> RenderAsync (MaskSliceData data)
        {
            float[] values = new float[data.values.Length];
            
            Parallel.For(0, values.Length, i =>
            {
                values[i] = data.values[i] ? 1f : 0f;
            });
            
            ScalarSliceData scalarSlice = new ScalarSliceData()
            {
                pixelCount = data.pixelCount,
                physicalSize = data.physicalSize,
                values = values
            };

            return ScalarSliceDataToScalarSliceRenderConversion.RenderAsync(scalarSlice);
        }
    }
}