using UnityEngine;

namespace UVL.Scalar.Rendering.Conversion
{
    public static class ScalarDataToScalarRenderConversion
    {
        public static Color ScalarToColor (float scalar)
        {
            return new Color(scalar, scalar, scalar);
        }

        public static bool StackTextureDimensionsAreNotUniform(ScalarStackData stack, Vector3Int voxelCount)
        {
            foreach (var slice in stack.slices)
                if (slice.pixelCount.x != voxelCount.x || slice.pixelCount.y != voxelCount.y)
                    return true;

            return false;
        }
    }
}