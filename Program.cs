using System;
using System.Globalization;
using System.IO;
using System.Linq;
using ExifLib;
using MediaInfoLib;
using TagLib;
using TagLib.Xmp;
using File = System.IO.File;

namespace Renamer
{
    public class Program
    {
        private static void Main(string[] args)
        {
            const string dateFormat = "yyyy-MM-dd HH-mm-ss";
            string[] allowedFileTypes =
            {
                ".jpg", ".jpeg", ".png", ".gif", ".mp4", ".avi", ".mov", ".mpg", ".mpeg"
            };

            string[] filePaths = Directory.GetFiles(Directory.GetCurrentDirectory());

            foreach (var filePath in filePaths)
            {
                if (!File.Exists(filePath))
                {
                    continue;
                }

                var fileName = Path.GetFileName(filePath);
                if (string.IsNullOrEmpty(fileName))
                {
                    continue;
                }

                var fileExtension = Path.GetExtension(fileName).ToLower();
                if (!allowedFileTypes.Contains(fileExtension))
                {
                    continue;
                }

                Console.WriteLine();
                Console.WriteLine(fileName);

                // Parse file name as date
                var fileNameDigits = new string(fileName.ToCharArray().Where(x => char.IsDigit(x)).ToArray());
                if (fileNameDigits.Length >= 14)
                {
                    var year = int.Parse(fileNameDigits.Substring(0, 4));
                    var month = int.Parse(fileNameDigits.Substring(4, 2));
                    var day = int.Parse(fileNameDigits.Substring(6, 2));

                    var hour = int.Parse(fileNameDigits.Substring(8, 2));
                    var minute = int.Parse(fileNameDigits.Substring(10, 2));
                    var second = int.Parse(fileNameDigits.Substring(12, 2));

                    DateTime fileNameDate;
                    DateTime.TryParse($"{year}-{month}-{day} {hour}:{minute}:{second}", out fileNameDate);
                    if (fileNameDate != new DateTime())
                    {
                        Console.WriteLine("               - Filename date:         " + fileNameDate.ToString(dateFormat) +
                                          fileExtension);
                    }
                    else
                    {
                        Console.WriteLine("               - Filename date:         -");
                    }
                }
                else
                {
                    Console.WriteLine("               - Filename date:         -");
                }

                //var fileModified = File.GetLastWriteTimeUtc(fileName);
                //Console.WriteLine("            - File modified:      " + fileModified.ToString(dateFormat));

                //var fileCreated = File.GetCreationTimeUtc(fileName);
                //Console.WriteLine("            - File created:       " + fileCreated.ToString(dateFormat));

                // Get XMP metadata with taglib
                if (fileExtension.Equals(".png") || fileExtension.Equals(".gif"))
                {
                    TagLib.File file = TagLib.File.Create(fileName);

                    XmpTag xmp = file.GetTag(TagTypes.XMP) as XmpTag;
                    if (xmp != null)
                    {
                        var tree = xmp.NodeTree;
                        var node = tree.GetChild(XmpTag.PHOTOSHOP_NS, "DateCreated");
                        var dateCreated = node.Value;

                        DateTime xmpDateCreated;
                        DateTime.TryParse(dateCreated, out xmpDateCreated);
                        if (xmpDateCreated != new DateTime())
                        {
                            Console.WriteLine("               - XMP DateCreated:       " + xmpDateCreated.ToString(dateFormat) + fileExtension);
                        }
                    }
                }

                // Get metadata for videos with MediaInfoLib
                if (fileExtension.Equals(".mp4") || fileExtension.Equals(".mov"))
                {
                    DateTime createdDate;

                    // Created date for Android MP4 files and iPhone MOV files (UTC time)
                    // Format: UTC 2016-06-07 07:07:30
                    MediaInfo mediaInfo = new MediaInfo();
                    mediaInfo.Open(fileName);
                    var createdDateString = mediaInfo.Get(0, 0, "Encoded_Date");
                    mediaInfo.Close();

                    if (createdDateString.StartsWith("UTC"))
                    {
                        DateTime.TryParse(createdDateString.Substring(4)
                            , CultureInfo.CurrentCulture
                            , DateTimeStyles.AssumeUniversal
                            , out createdDate);
                    }
                    else
                    {
                        DateTime.TryParse(createdDateString, out createdDate);
                    }

                    if (createdDate != new DateTime())
                    {
                        Console.WriteLine("               - Video created date:    " + createdDate.ToString(dateFormat) + fileExtension);
                    }
                }

                // Get date from EXIF with ExifReader
                if (fileExtension.Equals(".jpg") || fileExtension.Equals(".jpeg")
                    )
                {
                    using (ExifReader reader = new ExifReader(fileName))
                    {
                        // Extract the tag data using the ExifTags enumeration
                        DateTime datePictureTaken;
                        if (reader.GetTagValue(ExifTags.DateTimeDigitized,
                            out datePictureTaken))
                        {
                            Console.WriteLine("               - EXIF date:             " +
                                              datePictureTaken.ToString(dateFormat) + fileExtension);
                        }
                    }
                }
            }
        }
    }
}
