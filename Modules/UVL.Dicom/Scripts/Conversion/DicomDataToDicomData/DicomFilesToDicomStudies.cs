using System;
using System.Collections.Generic;
using System.Linq;

using Dicom;

namespace UVL.Dicom.Conversion
{
    public static class DicomFilesToDicomStudies
    {
        public static DicomStudy[] SortIntoStudies (IEnumerable<DicomFile> files)
        {
            var studyFileMap = SplitFilesByTag(files, DicomTag.StudyID)
                .ToArray();

            DicomStudy[] studies = new DicomStudy[studyFileMap.Length];
            for (int i = 0; i < studyFileMap.Length; i++)
            {
                studies[i] = new DicomStudy(
                    studyFileMap[i].Key, 
                    SortFilesIntoSeries(studyFileMap[i].Value)
                    );
            }

            Array.Sort(studies, (DicomStudy a, DicomStudy b) =>
            {
                return a.studyName.CompareTo(b.studyName);
            });

            return studies;
        }

        private static DicomSeries[] SortFilesIntoSeries(IEnumerable<DicomFile> files)
        {
            var seriesFileMap = SplitFilesByTag(files, DicomTag.SeriesNumber)
                .ToArray();

            DicomSeries[] series = new DicomSeries[seriesFileMap.Length];
            for (int i = 0; i < series.Length; i++)
            {
                series[i] = new DicomSeries(
                    int.Parse(seriesFileMap[i].Key), 
                    seriesFileMap[i].Value.ToArray()
                    );
            }

            Array.Sort(series, (DicomSeries a, DicomSeries b) =>
            {
                return a.seriesNumber.CompareTo(b.seriesNumber);
            });

            return series;
        }

        public static Dictionary<string, List<DicomFile>> SplitFilesByTag(IEnumerable<DicomFile> dicomFiles, DicomTag tag)
        {
            var idFileMap = new Dictionary<string, List<DicomFile>>();

            foreach (var file in dicomFiles)
            {
                string id;

                try { id = file.Dataset.GetSingleValue<string>(tag); }
                catch { id = "N/A"; }

                if (!idFileMap.ContainsKey(id))
                    idFileMap.Add(id, new List<DicomFile>());

                idFileMap[id].Add(file);
            }

            return idFileMap;
        }
    }
}