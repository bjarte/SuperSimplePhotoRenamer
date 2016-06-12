using System.Linq;

namespace MediadataServices.Helpers
{
    public static class FiletypeHelpers
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
