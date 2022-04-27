using UnityEngine;

namespace UVL.Scalar
{
    public class ScalarSliceData
    {
        public Vector2Int pixelCount;
        public Vector2 physicalSize;
        public float[] values;
    }

    public class ScalarStackData
    {
        public ScalarSliceData[] slices;
    }

    public class ScalarVolumeData
    {
        public Vector3Int voxelCount;
        public Vector3 physicalSize;
        public float[] values;
    }

    public static class ScalarData
    {
        public static ScalarStackData ToStackData(this ScalarVolumeData volume)
            => Conversion.ScalarVolumeDataToScalarStackData.Convert(volume);
    
        public static ScalarVolumeData ToVolumeData(this ScalarStackData stack)
            => Conversion.ScalarStackDataToScalarVolumeData.Convert(stack);
    }

    public static class MaskDataToScalarData
    {
        public static ScalarSliceData ToScalarSliceData(this MaskSliceData slice)
            => Conversion.MaskSliceDataToScalarSliceData.Convert(slice);

        public static ScalarStackData ToScalarStackData(this MaskStackData stack)
            => Conversion.MaskStackDataToScalarStackData.Convert(stack);

        public static ScalarStackData ToScalarStackData(this MaskVolumeData volume)
            => Conversion.MaskVolumeDataToScalarStackData.Convert(volume);

        public static ScalarVolumeData ToScalarVolumeData(this MaskStackData stack)
            => Conversion.MaskStackDataToScalarVolumeData.Convert(stack);

        public static ScalarVolumeData ToScalarVolumeData(this MaskVolumeData volume)
            => Conversion.MaskVolumeDataToScalarVolumeData.Convert(volume);
    }

    public static class TextureToScalarData
    {
        public static AsyncResult<ScalarSliceData> ToScalarSliceDataAsync(this Texture2D texture)
            => Conversion.Texture2DToScalarSliceData.ConvertAsync(texture);

        public static AsyncResult<ScalarStackData> ToScalarStackDataAsync(this Texture2D[] textures)
            => Conversion.Texture2DArrayToScalarStackData.ConvertAsync(textures);

        public static AsyncResult<ScalarStackData> ToScalarStackDataAsync(this Texture3D texture)
            => Conversion.Texture3DToScalarStackData.ConvertAsync(texture);

        public static AsyncResult<ScalarVolumeData> ToScalarVolumeDataAsync(this Texture2D[] textures)
            => Conversion.Texture2DArrayToScalarVolumeData.ConvertAsync(textures);

        public static AsyncResult<ScalarVolumeData> ToScalarVolumeDataAsync(this Texture3D texture)
            => Conversion.Texture3DToScalarVolumeData.ConvertAsync(texture);
    }
}