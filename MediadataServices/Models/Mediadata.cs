using System;

namespace MediadataServices.Models
{
    public class Mediadata
    {
        public string Path { get; set; }

        public string FileName { get; set; }
        public string Extension { get; set; }

        public DateTime XmpDateCreated { get; set; }
        public DateTime MediaInfoEncodedDate { get; set; }
        public DateTime MediaInfoMasteredDate { get; set; }
        public DateTime ExifDateDigitized { get; set; }
        public DateTime ExifDateOriginal { get; set; }

        public DateSource SourceForFilenameDate { get; set; }
    }

    public enum DateSource
    {
        Original,
        FromFilename,
        XmpDateCreated,
        MediaInfoEncodedDate,
        MediaInfoMasteredDate,
        ExifDateDigitized,
        ExifDateOriginal
    }
}


