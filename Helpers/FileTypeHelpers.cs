using System.Linq;

namespace Renamer.Helpers
{
    public static class FileTypeHelpers
    {
        public static bool IsAllowedFileType(string fileType)
        {
            if (fileType.StartsWith("."))
            {
                fileType = fileType.Substring(1);
            }

            return Settings.VideoFileTypes.Contains(fileType) || Settings.ImageFileTypes.Contains(fileType);
        }

        public static bool IsVideoFileType(string fileType)
        {
            if (fileType.StartsWith("."))
            {
                fileType = fileType.Substring(1);
            }

            return Settings.VideoFileTypes.Contains(fileType);
        }
    }
}
