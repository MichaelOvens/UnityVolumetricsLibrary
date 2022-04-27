using System.Threading.Tasks;
using UnityEngine;

namespace UVL.Conversion
{
    public static class MaskVolumeDataToMaskStackData
    {
        public static MaskStackData Convert(MaskVolumeData volume)
        {
            Vector2Int pixelCount = new Vector2Int(volume.voxelCount.x, volume.voxelCount.y);
            Vector2 physicalSize = new Vector2(volume.physicalSize.x, volume.physicalSize.y);

            MaskSliceData[] slices = new MaskSliceData[volume.voxelCount.z];
            int valuesPerSlice = pixelCount.x * pixelCount.y;

            Parallel.For(0, slices.Length, i =>
            {
                bool[] values = new bool[valuesPerSlice];
                for (int output = 0, input = i * valuesPerSlice; output < valuesPerSlice; output++, input++)
                    values[output] = volume.values[input];

                slices[i] = new MaskSliceData()
                {
                    pixelCount = pixelCount,
                    physicalSize = physicalSize,
                    values = values
                };
            });

            return new MaskStackData()
            {
                slices = slices
            };
        }
    }
}