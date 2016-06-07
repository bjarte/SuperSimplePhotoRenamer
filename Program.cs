using System;
using System.IO;
using System.Linq;
using ExifLib;

namespace Renamer
{
    class Program
    {
        static void Main(string[] args)
        {
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

                Console.WriteLine();
                Console.WriteLine(fileName);

                var fileModified = File.GetLastWriteTimeUtc(fileName);
                Console.WriteLine("            - File modified:   " + fileModified.ToString("yyyy-MM-dd HH:mm:ss"));

                var fileCreated = File.GetCreationTimeUtc(fileName);
                Console.WriteLine("            - File created:    " + fileCreated.ToString("yyyy-MM-dd HH:mm:ss"));

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
                        Console.WriteLine("            - Filename date:   " + fileNameDate.ToString("yyyy-MM-dd HH:mm:ss"));
                    }
                }

                // Get date from EXIF
                if (filePath.ToLower().EndsWith("jpg") || filePath.ToLower().EndsWith("jpeg"))
                {
                    using (ExifReader reader = new ExifReader(fileName))
                    {
                        // Extract the tag data using the ExifTags enumeration
                        DateTime datePictureTaken;
                        if (reader.GetTagValue(ExifTags.DateTimeDigitized,
                            out datePictureTaken))
                        {
                            Console.WriteLine("            - EXIF date:       " +
                                              datePictureTaken.ToString("yyyy-MM-dd HH:mm:ss"));
                        }
                    }
                }
            }
        }
    }
}
