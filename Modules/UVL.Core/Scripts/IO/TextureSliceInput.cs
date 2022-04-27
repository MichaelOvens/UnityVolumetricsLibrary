using System.Collections;
using System.IO;
using UnityEngine;

namespace UVL.IO
{
    public static class TextureSliceInput
    {
        public static AsyncResult<Texture2D> ReadAsync(string filePath)
        {
            var result = new AsyncResult<Texture2D>();
            AsyncFactory.Instance.StartCoroutine(ReadAsync(result, filePath));
            return result;
        }

        private static IEnumerator ReadAsync(AsyncResult<Texture2D> result, string filePath)
        {
            result.Start();

            Texture2D texture = new Texture2D(1, 1);
            var bytes = File.ReadAllBytes(filePath);
            texture.LoadImage(bytes);

            result.Complete(texture);

            yield return null;
        }
    }
}
