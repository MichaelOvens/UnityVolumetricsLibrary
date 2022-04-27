using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace UVL.Dicom
{
    public static class DicomTestData
    {
        public const float MAX_MS_PER_FRAME = 100;

        public static class DicomSeries
        {
            public static readonly string DIRECTORY = $"{TestData.DIRECTORY}/Foot DICOM Volume";
            public static readonly string FILEPATH = $"{TestData.DIRECTORY}/Foot DICOM Slice/1.3.12.2.1107.5.2.41.69581.2020072113464472647863348.dcm";
            
            public static readonly Vector3Int EXPECTED_VOXEL_COUNT = new Vector3Int(448, 448, 160);
            public static readonly Vector3 EXPECTED_PHYSICAL_SIZE = new Vector3(280, 280, 160);
            
            public static readonly int EXPECTED_SERIES_LENGTH = 1;
            public static readonly int EXPECTED_STUDIES_COUNT = 1;
        }
    }
}