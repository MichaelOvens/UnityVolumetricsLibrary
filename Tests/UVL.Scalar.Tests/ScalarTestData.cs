using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace UVL.Scalar
{
    public static class ScalarTestData
    {
        public const float MAX_MS_PER_FRAME = 100;

        public static class ImageSequence
        {
            public static readonly string DIRECTORY = $"{TestData.DIRECTORY}/Foot Image Sequence";
            public static readonly string FILEPATH = $"{TestData.DIRECTORY}/Foot Image File/Foot DICOM Slice.jpg";
            public static readonly Vector3Int EXPECTED_VOXEL_COUNT = new Vector3Int(448, 448, 160);
            public static readonly Vector3 EXPECTED_PHYSICAL_SIZE = new Vector3(448, 448, 160);
        }
    }
}