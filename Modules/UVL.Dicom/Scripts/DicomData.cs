using UnityEngine;

using FellowOakDicom;

namespace UVL.Dicom
{
    using UVL.Scalar;

    /// <summary>
    /// A related collection of DICOM files.
    /// </summary>
    public class DicomSeries
    {
        public readonly int seriesNumber;
        public readonly DicomFile[] files;

        public DicomSeries(int seriesNumber, DicomFile[] files)
        {
            this.seriesNumber = seriesNumber;
            this.files = files;

            foreach (var file in files)
            {
                int localSeriesNumber = file.Dataset.GetSingleValue<int>(DicomTag.SeriesNumber);
                if (seriesNumber != localSeriesNumber)
                {
                    Debug.LogError($"WARNING: DicomFile {file.File.Name} series number ({localSeriesNumber})" +
                        $"does not match enclosing series number {seriesNumber}.");
                }
            }
        }
    }

    /// <summary>
    /// The record of an imaging procedure containing one or more series.
    /// </summary>
    public class DicomStudy
    {
        public readonly string studyName;
        public readonly DicomSeries[] series;

        public DicomStudy(string studyName, DicomSeries[] series)
        {
            this.studyName = studyName;
            this.series = series;

            foreach (var set in series)
            {
                foreach (var file in set.files)
                {
                    string localStudyName = file.Dataset.GetSingleValue<string>(DicomTag.StudyID);
                    if (studyName != localStudyName)
                    {
                        Debug.LogError($"WARNING: DicomFile {file.File.Name} study name ({localStudyName})" +
                            $"does not match enclosing study name {localStudyName}.");
                    }
                }
            }
        }
    }

    public static class DicomData
    {
        public static DicomStudy[] ToStudies(this DicomFile[] files)
            => Conversion.DicomFilesToDicomStudies.SortIntoStudies(files);
    }

    public static class DicomFileToScalarData
    {
        public static AsyncResult<ScalarSliceData> ToScalarSliceDataAsync(this DicomFile file)
            => Conversion.DicomFileToScalarSliceDataConversion.ConvertAsync(file);

        public static AsyncResult<ScalarStackData> ToScalarStackDataAsync(this DicomFile[] files)
            => Conversion.DicomFileToScalarStackDataConversion.ConvertAsync(files);

        public static AsyncResult<ScalarVolumeData> ToScalarVolumeDataAsync(this DicomFile[] files)
            => Conversion.DicomFileToScalarVolumeDataConversion.ConvertAsync(files);
    }
}