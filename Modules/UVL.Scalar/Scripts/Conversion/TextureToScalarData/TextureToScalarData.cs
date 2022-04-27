using UnityEngine;

namespace UVL.Scalar.Conversion
{
    public static class TextureToScalarData
    {
        public static float ColorToScalar(Color color)
        {
            return (color.r + color.g + color.b) / 3f;
        }

        public static bool StackTextureDimensionsAreNotUniform(Texture2D[] textures, Vector3Int voxelCount)
        {
            foreach (var texture in textures)
                if (texture.width != voxelCount.x || texture.height != voxelCount.y)
                    return true;

            return false;
        }
    }
}