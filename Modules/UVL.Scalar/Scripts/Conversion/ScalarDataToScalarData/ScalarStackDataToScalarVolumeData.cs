using System.Threading.Tasks;
using UnityEngine;

namespace UVL.Scalar.Conversion
{
    public static class ScalarStackDataToScalarVolumeData
    {
        public static ScalarVolumeData Convert(ScalarStackData stack)
        {
            foreach (var slice in stack.slices)
                if (slice.pixelCount != stack.slices[0].pixelCount)
                    throw new System.DataMisalignedException("Stack contains nonuniform slice dimensions");

            Vector3Int voxelCount = new Vector3Int(stack.slices[0].pixelCount.x, stack.slices[0].pixelCount.y, stack.slices.Length);
            Vector3 physicalSize = new Vector3(stack.slices[0].physicalSize.x, stack.slices[0].physicalSize.x, stack.slices.Length);

            float[] values = new float[voxelCount.x * voxelCount.y * voxelCount.z];
            int valuesPerSlice = voxelCount.x * voxelCount.y;

            Parallel.For(0, stack.slices.Length, i =>
            {
                for (int input = 0, output = i * valuesPerSlice; input < valuesPerSlice; input++, output++)
                    values[output] = stack.slices[i].values[input];
            });

            return new ScalarVolumeData()
            {
                voxelCount = voxelCount,
                physicalSize = physicalSize,
                values = values
            };
        }
    }
}