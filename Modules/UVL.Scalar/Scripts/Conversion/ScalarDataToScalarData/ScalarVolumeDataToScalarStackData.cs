using System.Threading.Tasks;
using UnityEngine;

namespace UVL.Scalar.Conversion
{
    public static class ScalarVolumeDataToScalarStackData
    {
        public static ScalarStackData Convert(ScalarVolumeData volume)
        {
            Vector2Int pixelCount = new Vector2Int(volume.voxelCount.x, volume.voxelCount.y);
            Vector2 physicalSize = new Vector2(volume.physicalSize.x, volume.physicalSize.y);

            ScalarSliceData[] slices = new ScalarSliceData[volume.voxelCount.z];
            int valuesPerSlice = pixelCount.x * pixelCount.y;

            Parallel.For(0, slices.Length, i =>
            {
                float[] values = new float[valuesPerSlice];
                for (int output = 0, input = i * valuesPerSlice; output < valuesPerSlice; output++, input++)
                    values[output] = volume.values[input];

                slices[i] = new ScalarSliceData()
                {
                    pixelCount = pixelCount,
                    physicalSize = physicalSize,
                    values = values
                };
            });

            return new ScalarStackData()
            {
                slices = slices
            };
        }
    }
}