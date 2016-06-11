using System;
using System.IO;
using TagLib;
using TagLib.Xmp;

namespace Renamer.Helpers
{
    public static class XMPHelper
    {
        public static DateTime GetXMPDateCreated(string fileName)
        {
            var xmpDateCreated = new DateTime();

            var fileExtension = Path.GetExtension(fileName).ToLower();
            //if (fileExtension.Equals(".png") || fileExtension.Equals(".gif"))
            //{
                TagLib.File file = TagLib.File.Create(fileName);

                XmpTag xmp = file.GetTag(TagTypes.XMP) as XmpTag;
                if (xmp != null)
                {
                    var tree = xmp.NodeTree;
                    var node = tree.GetChild(XmpTag.PHOTOSHOP_NS, "DateCreated");
                    var dateCreated = node.Value;

                    DateTime.TryParse(dateCreated, out xmpDateCreated);
                }
            //}

            return xmpDateCreated;
        }
    }
}
