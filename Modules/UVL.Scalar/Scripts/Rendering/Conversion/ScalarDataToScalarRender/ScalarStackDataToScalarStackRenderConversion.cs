using System.Collections;
using System.Linq;

namespace UVL.Scalar.Rendering.Conversion
{
    public static class ScalarStackDataToScalarStackRenderConversion
    {
        public static AsyncResult<ScalarStackRender> RenderAsync(ScalarStackData data)
        {
            var result = new AsyncResult<ScalarStackRender>();
            AsyncFactory.Instance.StartCoroutine(Create(result, data));
            return result;
        }

        private static IEnumerator Create(AsyncResult<ScalarStackRender> result, ScalarStackData data)
        {
            result.Start();
            
            ScalarSliceRender[] slices = new ScalarSliceRender[data.slices.Length];
            for (int i = 0; i < slices.Length; i++)
            {
                AsyncResult<ScalarSliceRender> sliceResult = ScalarDataToScalarRender.ToScalarSliceRenderAsync(data.slices[i]);
                while (sliceResult.inProgress)
                    yield return null;
                slices[i] = sliceResult.Result;
            }

            float minValue = slices.Min(slice => slice.minValue);
            float maxValue = slices.Max(slice => slice.maxValue);

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