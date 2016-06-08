using System;
using System.IO;
using System.Linq;
using ExifLib;
using MediaInfoLib;
using Renamer.Helpers;
using TagLib;
using TagLib.Xmp;
using File = System.IO.File;

namespace Renamer
{
    public class Program
    {
        private static void Main(string[] args)
        {

            // Print column headers
            Console.WriteLine();
            OutputHelpers.WriteColumns(new[] { "EXISTING FILENAME", "NEW FILENAME", "DATE SOURCE" });
            Console.WriteLine();

            string[] filePaths = Directory.GetFiles(Directory.GetCurrentDirectory());

            foreach (var filePath in filePaths.Where(x => File.Exists(x)))
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
                DateTime xmpDateCreated = new DateTime();
                if (fileExtension.Equals(".png") || fileExtension.Equals(".gif"))
                {
                    TagLib.File file = TagLib.File.Create(fileName);

                    XmpTag xmp = file.GetTag(TagTypes.XMP) as XmpTag;
                    if (xmp != null)
                    {
                        var tree = xmp.NodeTree;
                        var node = tree.GetChild(XmpTag.PHOTOSHOP_NS, "DateCreated");
                        var dateCreated = node.Value;

                        DateTime.TryParse(dateCreated, out xmpDateCreated);
                    }
                }

                // Get video metadata with MediaInfoLib
                DateTime mediaInfoEncodedDate = new DateTime();
                DateTime mediaInfoMasteredDate = new DateTime();
                if (FileTypeHelpers.IsVideoFileType(fileExtension))
                {
                    MediaInfo mediaInfo = new MediaInfo();
                    mediaInfo.Open(fileName);

                    var encodedDateString = mediaInfo.Get(0, 0, "Encoded_Date");
                    mediaInfoEncodedDate = encodedDateString.ConvertToDateTime();

                    var masteredDateString = mediaInfo.Get(0, 0, "Mastered_Date");
                    mediaInfoMasteredDate = masteredDateString.ConvertToDateTime();

                    mediaInfo.Close();
                }

                // Get EXIF metadata with ExifReader
                DateTime exifDateDigitized = new DateTime();
                if (fileExtension.Equals(".jpg") || fileExtension.Equals(".jpeg"))
                {
                    using (ExifReader reader = new ExifReader(fileName))
                    {
                        reader.GetTagValue(ExifTags.DateTimeDigitized, out exifDateDigitized);
                    }
                }

                // Print existing filename
                //OutputHelpers.WriteColumns(new[] { "Existing filename:", fileName });

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
