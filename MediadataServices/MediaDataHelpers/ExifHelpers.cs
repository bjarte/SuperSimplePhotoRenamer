using System;
using ExifLib;

namespace MediadataServices.MediadataHelpers
{
    public static class ExifHelpers
    {
        public static DateTime GetExifDateDigitized(string fileName)
        {
            var date = new DateTime();

            try
            {
                using (var reader = new ExifReader(fileName))
                {
                    reader.GetTagValue(ExifTags.DateTimeDigitized, out date);
                }
            }
            catch (ExifLibException)
            {
                // File is not valid JPEG
            }

            return date;
        }

        public static DateTime GetExifDateTaken(string fileName)
        {
            var date = new DateTime();

            try
            {
                using (var reader = new ExifReader(fileName))
                {
                    reader.GetTagValue(ExifTags.DateTimeOriginal, out date);
                }
            }
            catch (ExifLibException)
            {
                // File is not valid JPEG
            }

            return date;
        }
    }
}
