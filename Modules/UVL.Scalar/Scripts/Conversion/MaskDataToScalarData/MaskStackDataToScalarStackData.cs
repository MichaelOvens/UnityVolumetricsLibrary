using System.Threading.Tasks;

namespace UVL.Scalar.Conversion
{
    public static class MaskStackDataToScalarStackData
    {
        public static ScalarStackData Convert(MaskStackData maskStack)
        {
            ScalarSliceData[] slices = new ScalarSliceData[maskStack.slices.Length];

            Parallel.For(0, slices.Length, i =>
            {
                slices[i] = maskStack.slices[i].ToScalarSliceData();
            });

            ScalarStackData scalarStack = new ScalarStackData()
            {
                slices = slices
            };

            return scalarStack;
        }
    }
}