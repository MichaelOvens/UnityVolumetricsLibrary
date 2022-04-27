using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using FellowOakDicom;

namespace UVL.Dicom.IO
{
    public static class DicomDirectoryInput
    {
        public static AsyncResult<DicomFile[]> ReadAsync(string directory)
        {
            var result = new AsyncResult<DicomFile[]>();
            AsyncFactory.Instance.StartCoroutine(ReadAsync(result, directory));
            return result;
        }

        private static IEnumerator ReadAsync(AsyncResult<DicomFile[]> result, string directory)
        {
            result.Start();

            var filePaths = GetDicomFilePathsInDirectory(directory);

            DicomFile[] dicomFiles = new DicomFile[filePaths.Count()];

            // Start this parallel operation in a task so it doesn't lock the main thread
            Task task = Task.Run(() =>
            {
                Parallel.For(0, dicomFiles.Length, i =>
                {
                    dicomFiles[i] = DicomFile.Open(filePaths.ElementAt(i));
                });
            });

            while (!task.IsCompleted)
                yield return null;

            TryOrderBy<int>(ref dicomFiles, DicomTag.InstanceNumber); // Recommended default ordering

            result.Complete(dicomFiles);
        }

        private static IEnumerable<string> GetDicomFilePathsInDirectory(string directory)
        {
            // Gets all files in the directory that end with one of the specified DICOM file types
            string[] allFiles = Directory.GetFiles(directory);

            var dicomFiles = new List<string>();
            foreach (var file in allFiles)
            {
                foreach (var extension in DicomFileExtensions)
                {
                    if (file.EndsWith(extension))
                    {
                        dicomFiles.Add(file);
                        break;
                    }
                }
            }

            if (dicomFiles.Count() == 0)
                throw new System.IndexOutOfRangeException("Directory contains no files with a recognised DICOM extension (.dcm, .dicom, .dicm).");

            return dicomFiles;
        }

        /// <remarks>
        /// Recommended tag to sort by is InstanceNumber.
        /// </remarks>
        private static bool TryOrderBy<T>(ref DicomFile[] dicomFiles, DicomTag tag) where T : IComparable
        {
            // Check that all files contain the tag
            foreach (var file in dicomFiles)
                if (!file.Dataset.Contains(tag))
                    return false;

            Array.Sort(dicomFiles, (DicomFile a, DicomFile b) =>
            {
                return a.Dataset.GetValue<T>(tag, 0)
                .CompareTo(b.Dataset.GetValue<T>(tag, 0));
            });

            return true;
        }

        private static readonly string[] DicomFileExtensions = new string[]
        {
            "dicom",
            "dicm",
            "dcm",
        };
    }
}