namespace UVL.Scalar.Conversion
{
    public class MaskSliceDataToScalarSliceData
    {
        public static ScalarSliceData Convert (MaskSliceData maskSlice)
        {
            float[] values = MaskDataToScalarData.BoolValuesToFloatValues(maskSlice.values);

            ScalarSliceData scalarSlice = new ScalarSliceData()
            {
                pixelCount = maskSlice.pixelCount,
                physicalSize = maskSlice.physicalSize,
                values = values
            };

            return scalarSlice;
        }
    }
}