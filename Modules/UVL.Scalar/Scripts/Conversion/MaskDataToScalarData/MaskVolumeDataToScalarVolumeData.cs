namespace UVL.Scalar.Conversion
{
    public class MaskVolumeDataToScalarVolumeData
    {
        public static ScalarVolumeData Convert(MaskVolumeData maskVolume)
        {
            float[] values = MaskDataToScalarData.BoolValuesToFloatValues(maskVolume.values);

            ScalarVolumeData scalarVolume = new ScalarVolumeData()
            {
                voxelCount = maskVolume.voxelCount,
                physicalSize = maskVolume.physicalSize,
                values = values
            };

            return scalarVolume;
        }
    }
}