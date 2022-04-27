using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

using Dicom;

namespace UVL.Dicom.IO
{
    public class DicomInputTests
    {
        private readonly string DIRECTORY = DicomTestData.DicomSeries.DIRECTORY;
        private readonly string FILEPATH = DicomTestData.DicomSeries.FILEPATH;

        [UnityTest]
        public IEnumerator ReadFileAsDicomFile()
        {
            AsyncResult<DicomFile> result = DicomFileInput.ReadAsync(FILEPATH);

            while (result.inProgress)
                yield return null;

            Assert.That(result.Result != null);
        }

        [UnityTest]
        public IEnumerator ReadDirectoryAsDicomFileArray()
        {
            AsyncResult<DicomFile[]> result = DicomDirectoryInput.ReadAsync(DIRECTORY);

            while (result.inProgress)
                yield return null;

            Assert.That(result.Result != null);
        }
    }
}