using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UVL.IO
{
    public static class TextureVolumeInput
    {
        private const float VOLUME_WORK_ESTIMATE = 0.1f;
        private const float STACK_WORK_ESTIMATE = 0.9f;

        public static AsyncResult<Texture3D> ReadAsync(string directory)
        {
            var result = new AsyncResult<Texture3D>();
            AsyncFactory.Instance.StartCoroutine(ReadAsync(result, directory));
            return result;
        }

        private static IEnumerator ReadAsync(AsyncResult<Texture3D> result, string directory)
        {
            result.Start();

            AsyncResult<Texture2D[]> stackResult = TextureStackInput.ReadAsync(directory);

            while (stackResult.inProgress)
            {
                yield return null;
                result.Report(stackResult.ProgressValue * STACK_WORK_ESTIMATE);
            }

            Texture2D[] stack = stackResult.Result;

            Vector3Int voxelCount = new Vector3Int()
            {
                x = stack[0].width,
                y = stack[0].height,
                z = stack.Length
            };

            if (StackTextureDimensionsAreNotUniform(stack, voxelCount))
            {
                result.Throw(new DataMisalignedException("Texture stack has non-uniform dimensions"));
                yield break;
            }

            // Define loop variables
            DateTime lastFrame = DateTime.Now;
            int textureWidth = voxelCount.x * voxelCount.y;
            Color[] voxelColors = new Color[voxelCount.x * voxelCount.y * voxelCount.z];

            // Copy pixel colors from each texture into a continuous array
            for (int i = 0; i < stack.Length; i++)
            {
                Color[] textureColors = stack[i].GetPixels();
                Array.Copy(textureColors, 0, voxelColors, textureWidth * i, textureWidth);

                if (Async.FrameLimitExceeded(lastFrame))
                {
                    result.Report(((float)i / stack.Length) * VOLUME_WORK_ESTIMATE + STACK_WORK_ESTIMATE);
                    yield return null;
                    lastFrame = DateTime.Now;
                }
            }

            // Pack the voxels into a volume
            Texture3D volume = new Texture3D(voxelCount.x, voxelCount.y, voxelCount.z, TextureFormat.RGBA32, false);
            volume.SetPixels(voxelColors);
            volume.Apply();

            result.Complete(volume);
        }

        private static bool StackTextureDimensionsAreNotUniform(Texture2D[] textures, Vector3Int voxelCount)
        {
            foreach (var texture in textures)
                if (texture.width != voxelCount.x || texture.height != voxelCount.y)
                    return true;

            return false;
        }
    }
}