using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace UVL.IO
{
    public static class TextureStackInput
    {
        public static AsyncResult<Texture2D[]> ReadAsync(string directory)
        {
            var result = new AsyncResult<Texture2D[]>();
            AsyncFactory.Instance.StartCoroutine(ReadAsync(result, directory));
            return result;
        }

        private static IEnumerator ReadAsync(AsyncResult<Texture2D[]> result, string directory)
        {
            var filePaths = GetImageFilePathsInDirectory(directory);
            Texture2D[] output = new Texture2D[filePaths.Count()];
            DateTime lastFrame = DateTime.Now;

            result.Start();

            for (int i = 0; i < output.Length; i++)
            {
                var sliceResult = TextureSliceInput.ReadAsync(filePaths.ElementAt(i));
                while (sliceResult.inProgress)
                    yield return null;

                output[i] = sliceResult.Result;

                if (Async.FrameLimitExceeded(lastFrame))
                {
                    result.Report((float)i / output.Length);
                    yield return null;
                    lastFrame = DateTime.Now;
                }
            }

            result.Complete(output);
        }

        private static IEnumerable<string> GetImageFilePathsInDirectory(string directory)
        {
            // Gets all files in the directory that end with one of the specified image file types
            IEnumerable<string> filePaths = Directory.EnumerateFiles(directory, "*.*", SearchOption.TopDirectoryOnly)
                    .Where(p => p.EndsWith(".jpg") || p.EndsWith(".png"));

            if (filePaths.Count() == 0)
                throw new IndexOutOfRangeException("Directory contains no files with a supported image extension (.jpg, .png).");

            // Order the paths alphabetically
            filePaths.OrderBy(path => path);

            return filePaths;
        }
    }
}