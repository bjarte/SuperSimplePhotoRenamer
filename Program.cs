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

                // Parse file name as date

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

                // Get metadata for videos with MediaInfoLib
                DateTime mediaInfoEncodedDate = new DateTime();
                DateTime mediaInfoMasteredDate = new DateTime();
                if (FileTypeHelpers.IsVideoFileType(fileExtension))
                {
                    MediaInfo mediaInfo = new MediaInfo();
                    mediaInfo.Open(fileName);

                    // Created date for Android MP4 files and iPhone MOV files (UTC time)
                    // Format: UTC 2016-06-07 07:07:30
                    var encodedDateString = mediaInfo.Get(0, 0, "Encoded_Date");
                    mediaInfoEncodedDate = encodedDateString.ConvertToDateTime();

                    var masteredDateString = mediaInfo.Get(0, 0, "Mastered_Date");
                    mediaInfoMasteredDate = masteredDateString.ConvertToDateTime();

                    mediaInfo.Close();
                }

                // Get date from EXIF with ExifReader
                DateTime exifDateDigitized = new DateTime();
                if (fileExtension.Equals(".jpg") || fileExtension.Equals(".jpeg"))
                {
                    using (ExifReader reader = new ExifReader(fileName))
                    {
                        reader.GetTagValue(ExifTags.DateTimeDigitized, out exifDateDigitized);
                    }
                }

                // Print all results
                Console.WriteLine();

                // Print filename and date from filename
                var fileNameDate = fileName.ConvertToDateTime();
                if (fileNameDate != new DateTime())
                {
                    OutputHelpers.WriteColumns(new[]
                    { fileName, "Date from filename:", fileNameDate.ToString(Settings.DateFormat) });
                }
                else
                {
                    OutputHelpers.WriteColumns(new[] { fileName, "Date from filename:", "---" });
                }

                // Print date found in XMP DateCreated property
                if (xmpDateCreated != new DateTime())
                {
                    OutputHelpers.WriteColumns(new[] { "", "XMP DateCreated:", xmpDateCreated.ToString(Settings.DateFormat) });
                }

                // Print date found in MediaInfo Encoded_Date property
                if (mediaInfoEncodedDate != new DateTime())
                {
                    OutputHelpers.WriteColumns(new[] { "", "MediaInfo Encoded_Date:", mediaInfoEncodedDate.ToString(Settings.DateFormat) });
                }

                // Print date found in MediaInfo Mastered_Date property
                if (mediaInfoMasteredDate != new DateTime())
                {
                    OutputHelpers.WriteColumns(new[] { "", "MediaInfo Mastered_Date:", mediaInfoMasteredDate.ToString(Settings.DateFormat) });
                }

                // Print date found in EXIF Date Digitized property
                if (exifDateDigitized != new DateTime())
                {
                    OutputHelpers.WriteColumns(new[] { "", "EXIF DateDigitized:", exifDateDigitized.ToString(Settings.DateFormat) });
                }

                // Print date found in File modified property
                //var fileModified = File.GetLastWriteTime(fileName);
                //OutputHelpers.WriteColumns(new[] { "", "File modified:", fileModified.ToString(Settings.DateFormat) });

                // Print date found in File created property
                //var fileCreated = File.GetCreationTime(fileName);
                //OutputHelpers.WriteColumns(new[] { "", "File created:", fileCreated.ToString(Settings.DateFormat) });
            }
        }
    }
}
