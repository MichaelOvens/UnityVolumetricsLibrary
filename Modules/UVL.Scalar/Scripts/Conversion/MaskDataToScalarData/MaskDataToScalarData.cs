using System.Threading.Tasks;

namespace UVL.Scalar.Conversion
{
    public static class MaskDataToScalarData
    {
        public static float[] BoolValuesToFloatValues(bool[] bools)
        {
            float[] floats = new float[bools.Length];
            Parallel.For(0, floats.Length, i =>
            {
                floats[i] = bools[i] ? 1f : 0f;
            });
            return floats;
        }
    }
}