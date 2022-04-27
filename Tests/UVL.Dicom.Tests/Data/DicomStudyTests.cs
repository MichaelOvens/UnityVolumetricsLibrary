using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

using FellowOakDicom;

namespace UVL.Dicom.Data
{
    using UVL.Dicom.IO;

    public class DicomStudyTests
    {
        public static readonly string DIRECTORY = DicomTestData.DicomSeries.DIRECTORY;
        public static readonly int EXPECTED_STUDIES_LENGTH = DicomTestData.DicomSeries.EXPECTED_STUDIES_COUNT;

        [UnityTest]
        public IEnumerator SortsDicomFilesIntoSeries()
        {
            AsyncResult<DicomFile[]> result = DicomDirectoryInput.ReadAsync(DIRECTORY);

            while (result.inProgress)
                yield return null;

            DicomStudy[] studies = result.Result.ToStudies();

            Assert.That(studies.Length == EXPECTED_STUDIES_LENGTH);
        }
    }
}