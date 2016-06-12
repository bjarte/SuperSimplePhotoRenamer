using System;
using TagLib;
using TagLib.Xmp;

namespace MediadataServices.MediadataHelpers
{
    public static class XmpHelpers
    {
        public static DateTime GetXmpDateCreated(string fileName)
        {
            var xmpDateCreated = new DateTime();

            try
            {
                var file = File.Create(fileName);

                var xmp = file.GetTag(TagTypes.XMP) as XmpTag;
                if (xmp != null)
                {
                    var tree = xmp.NodeTree;
                    var node = tree.GetChild(XmpTag.PHOTOSHOP_NS, "DateCreated");
                    if (node?.Value != null)
                    {
                        var dateCreated = node.Value;
                        DateTime.TryParse(dateCreated, out xmpDateCreated);
                    }
                }
            }
            catch (UnsupportedFormatException)
            {
                // Filetype not supported by TagLib
            }

            return xmpDateCreated;
        }
    }
}
