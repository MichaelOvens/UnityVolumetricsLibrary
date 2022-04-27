namespace UVL.Scalar.Conversion
{
    public static class MaskStackDataToScalarVolumeData
    {
        public static ScalarVolumeData Convert (MaskStackData maskStack)
        {
            ScalarStackData scalarStack = MaskStackDataToScalarStackData.Convert(maskStack);
            ScalarVolumeData scalarVolume = scalarStack.ToVolumeData();
            return scalarVolume;
        }
    }
}