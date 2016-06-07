using System;
using System.IO;
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
                Console.WriteLine(filePath);

                if (filePath.ToLower().EndsWith("jpg") || filePath.ToLower().EndsWith("jpeg"))
                {
                    using (ExifReader reader = new ExifReader(filePath))
                    {
                        // Extract the tag data using the ExifTags enumeration
                        DateTime datePictureTaken;
                        if (reader.GetTagValue(ExifTags.DateTimeDigitized,
                            out datePictureTaken))
                        {
                            Console.WriteLine(" - EXIF date: " +
                                              datePictureTaken.ToString("yyyy-MM-dd HH:mm:ss"));
                        }
                    }
                }
            }
        }
    }
}
