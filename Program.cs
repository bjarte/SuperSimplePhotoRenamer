using System;
using System.IO;
using System.Linq;
using Renamer.Helpers;
using Renamer.MediaDataHelpers;
using File = System.IO.File;

namespace Renamer
{
    public class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Arguments available:");
            OutputHelpers.WriteColumns(new[] { "/v", "Verbose output (more information about each file" });

            // User defined settings
            var settingVerbose = false;

            foreach (var arg in args)
            {
                if (arg.ToLower().Contains("/v"))
                {
                    settingVerbose = true;
                }
            }

            // Print column headers
            Console.WriteLine();
            OutputHelpers.WriteColumns(new[] { "EXISTING FILENAME", "NEW FILENAME", "DATE SOURCE" });
            Console.WriteLine();

            var filePaths = Directory.GetFiles(Directory.GetCurrentDirectory());

            foreach (var filePath in filePaths.Where(File.Exists))
            {
                var fileName = Path.GetFileName(filePath);
                if (string.IsNullOrEmpty(fileName))
                {
                    continue;
                }

                var fileExtension = Path.GetExtension(fileName).ToLower();
                if (!FileTypeHelpers.IsAllowedFileType(fileExtension))
                {
                    continue;
                }

                // Get XMP metadata with taglib
                var xmpDateCreated = XmpHelpers.GetXmpDateCreated(fileName);

                // Get video metadata with MediaInfoLib
                var mediaInfoEncodedDate = MediaInfoHelpers.GetMediaInfoEncodedDate(fileName);
                var mediaInfoMasteredDate = MediaInfoHelpers.GetMediaInfoMasteredDate(fileName);

                // Get EXIF metadata with ExifReader
                var exifDateDigitized = ExifHelpers.GetExifDateDigitized(fileName);
                var exifDateOriginal = ExifHelpers.GetExifDateTaken(fileName);


                // Define new filename
                var newFileName = fileName;
                var nameSource = "Original filename";

                // Print date found in existing filename
                var fileNameDate = fileName.ConvertToDateTime();
                if (fileNameDate != new DateTime())
                {
                    newFileName = fileNameDate.ToString(Settings.DateFormat) + fileExtension;
                    nameSource = "Date from filename";
                    //OutputHelpers.WriteColumns(new[] { "Date from filename:", newFileName });
                }

                // Print date found in XMP DateCreated property
                if (xmpDateCreated != new DateTime())
                {
                    newFileName = xmpDateCreated.ToString(Settings.DateFormat) + fileExtension;
                    nameSource = "XMP DateCreated";
                    //OutputHelpers.WriteColumns(new[] { "XMP DateCreated:", newFileName });
                }

                // Print date found in MediaInfo Encoded_Date property
                if (mediaInfoEncodedDate != new DateTime())
                {
                    newFileName = mediaInfoEncodedDate.ToString(Settings.DateFormat) + fileExtension;
                    nameSource = "Encoded_Date";
                    //OutputHelpers.WriteColumns(new[] { "MediaInfo Encoded_Date:", newFileName });
                }

                // Print date found in MediaInfo Mastered_Date property
                if (mediaInfoMasteredDate != new DateTime())
                {
                    newFileName = mediaInfoMasteredDate.ToString(Settings.DateFormat) + fileExtension;
                    nameSource = "Mastered_Date";
                    //OutputHelpers.WriteColumns(new[] { "MediaInfo Mastered_Date:", newFileName });
                }

                // Print date found in EXIF Date Digitized property
                if (exifDateDigitized != new DateTime())
                {
                    newFileName = exifDateDigitized.ToString(Settings.DateFormat) + fileExtension;
                    nameSource = "EXIF DateDigitized";
                    //OutputHelpers.WriteColumns(new[] { "EXIF DateDigitized:", newFileName });
                }

                // Print date found in EXIF Date Original property
                if (exifDateOriginal != new DateTime())
                {
                    newFileName = exifDateOriginal.ToString(Settings.DateFormat) + fileExtension;
                    nameSource = "EXIF DateOriginal";
                    //OutputHelpers.WriteColumns(new[] { "EXIF DateOriginal:", newFileName });
                }

                OutputHelpers.WriteColumns(new[] { fileName, newFileName, nameSource });

                // Print date found in File modified property
                //var fileModified = File.GetLastWriteTime(fileName);
                //OutputHelpers.WriteColumns(new[] { "File modified:", fileModified.ToString(Settings.DateFormat) });

                // Print date found in File created property
                //var fileCreated = File.GetCreationTime(fileName);
                //OutputHelpers.WriteColumns(new[] { "File created:", fileCreated.ToString(Settings.DateFormat) });
            }

            //Console.WriteLine();
            //Console.WriteLine("Press any key to exit application...");
            //Console.Read();
        }
    }
}
