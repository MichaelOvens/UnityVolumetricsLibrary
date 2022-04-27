using System.Collections;
using UnityEngine;

namespace UVL.Scalar.Conversion
{
    public static class Texture2DArrayToScalarStackData
    {
        public static AsyncResult<ScalarStackData> ConvertAsync(Texture2D[] textures)
        {
            var result = new AsyncResult<ScalarStackData>();
            AsyncFactory.Instance.StartCoroutine(Create(result, textures));
            return result;
        }

        private static IEnumerator Create(AsyncResult<ScalarStackData> result, Texture2D[] textures)
        {
            result.Start();

            ScalarSliceData[] slices = new ScalarSliceData[textures.Length];
            for (int i = 0; i < slices.Length; i++)
            {
                AsyncResult<ScalarSliceData> sliceResult = Scalar.TextureToScalarData.ToScalarSliceDataAsync(textures[i]);
                while (sliceResult.inProgress)
                    yield return null;
                slices[i] = sliceResult.Result;
            }

            ScalarStackData stackData = new ScalarStackData()
            {
                slices = slices
            };

            result.Complete(stackData);
        }
    }
}