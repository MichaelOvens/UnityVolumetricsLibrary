using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace UVL.Scalar.Rendering.Conversion
{
    public static class MaskStackDataToScalarStackRenderConversion
    {
        public static AsyncResult<ScalarStackRender> RenderAsync (MaskStackData data)
        {
            ScalarSliceData[] scalarSlices = new ScalarSliceData[data.slices.Length];

            Parallel.For(0, scalarSlices.Length, i =>
            {
                float[] values = new float[scalarSlices[i].values.Length];

                for (int j = 0; j < values.Length; j++)
                    values[j] = data.slices[i].values[j] ? 1f : 0f;

                scalarSlices[i] = new ScalarSliceData()
                {
                    pixelCount = data.slices[i].pixelCount,
                    physicalSize = data.slices[i].physicalSize,
                    values = values
                };
            });

            //ScalarStackData

            throw new System.NotImplementedException();
        }
    }
}