using System.IO.Compression;
using System.Text;

namespace FileTypeTaster;

public static class OpenOfficeExtensions
{
    public static string ExtractContentTypes(this ZipArchive ooArchive)
    {
        var contentTypeDesc = ooArchive.Entries.Single(e => e.Name.Equals("[Content_Types].xml"));

        byte[] uncompressedContents;
        using (var source = contentTypeDesc.Open())
        using (var dest = new MemoryStream())
        {
            source.CopyTo(dest);
            uncompressedContents = dest.ToArray();
        }

        return Encoding.UTF8.GetString(uncompressedContents);
    }
}