using UnityEngine;

namespace UVL
{
    public class MaskSliceData
    {
        public Vector2Int pixelCount;
        public Vector2 physicalSize;
        public bool[] values;
    }

    public class MaskStackData
    {
        public MaskSliceData[] slices;
    }

    public class MaskVolumeData
    {
        public Vector3Int voxelCount;
        public Vector3 physicalSize;
        public bool[] values;
    }

    public static class MaskData
    {
        public static MaskStackData ToStackData(this MaskVolumeData volume)
            => Conversion.MaskVolumeDataToMaskStackData.Convert(volume); 
     
        public static MaskVolumeData ToVolumeData(this MaskStackData stack)
            => Conversion.MaskStackDataToMaskVolumeData.Convert(stack);
    }
}