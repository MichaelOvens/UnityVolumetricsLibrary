namespace UVL.Scalar.Conversion
{
    public static class MaskVolumeDataToScalarStackData
    {
        public static ScalarStackData Convert (MaskVolumeData maskVolume)
        {
            ScalarVolumeData scalarVolume = MaskVolumeDataToScalarVolumeData.Convert(maskVolume);
            ScalarStackData scalarStack = scalarVolume.ToStackData();
            return scalarStack;
        }
    }
}