using System.IO.Compression;
using FileTypeTaster.Reader;

namespace FileTypeTaster.Taster;

public abstract class OpenOfficeXml
{
    private readonly FilesystemOffsetReader _reader;
    private const int _headerSize = 8;
    private const int _trailerBuffer = 18;
    private static readonly byte[] _expectedSuffix = { 0x50, 0x4B, 0x05, 0x06, };
    private static readonly Dictionary<string, Filetype> _contentTypeToFiletype = new(StringComparer.OrdinalIgnoreCase)
    {
        { "ppt", Filetype.Powerpoint },
        { "word", Filetype.Word },
        { "xl", Filetype.Excel },
    };

    public OpenOfficeXml(FilesystemOffsetReader reader)
    {
        _reader = reader;
    }

    /// <summary>
    /// </summary>
    /// <remarks>50 4B 03 04 14 00 06 00	 	PK......
    /// DOCX, PPTX, XLSX	 	                Microsoft Office Open XML Format (OOXML) Document
    /// NOTE: There is no subheader for MS OOXML files as there is with DOC, PPT, and XLS files. To better understand the format of these files, rename any
    /// OOXML file to have a .ZIP extension and then unZIP the file; look at the resultant file named [Content_Types].xml to see the content types.
    /// In particular, look for the Override PartName= tag, where you will find word, ppt, or xl, respectively.
    /// Trailer: Look for 50 4B 05 06 (PK..) followed by 18 additional bytes at the end of the file.</remarks>
    /// <param name="path"></param>
    /// <param name="expectedContentType"></param>
    /// <returns></returns>
    protected async Task<Filetype> TastesLikeAsync(string path, string expectedContentType)
    {
        if (!_contentTypeToFiletype.TryGetValue(expectedContentType, out var expectedFiletype))
        {
            throw new ArgumentException($"{expectedContentType} is not a recognized type");
        }

        var actualPrefix = await _reader.GetStartAsync(path, _headerSize);
        if (!HasRequiredHeader(actualPrefix))
        {
            return Filetype.Unknown;
        }

        var actualTrailer = await _reader.GetEndAsync(path, _expectedSuffix.Length + _trailerBuffer);
        if (!actualTrailer.StartsWith(_expectedSuffix))
        {
            return Filetype.Unknown;
        }

        return ContainsContentType(path, expectedContentType)
            ? expectedFiletype
            : Filetype.Unknown;
    }

    /// <summary>
    /// </summary>
    /// <remarks>50 4B 03 04 14 00 06 00	 	PK......
    /// DOCX, PPTX, XLSX	 	                Microsoft Office Open XML Format (OOXML) Document
    /// NOTE: There is no subheader for MS OOXML files as there is with DOC, PPT, and XLS files. To better understand the format of these files, rename any
    /// OOXML file to have a .ZIP extension and then unZIP the file; look at the resultant file named [Content_Types].xml to see the content types.
    /// In particular, look for the Override PartName= tag, where you will find word, ppt, or xl, respectively.
    /// Trailer: Look for 50 4B 05 06 (PK..) followed by 18 additional bytes at the end of the file.</remarks>
    /// <param name="contents"></param>
    /// <param name="expectedContentType"></param>
    /// <returns></returns>
    protected Filetype TastesLike(ReadOnlySpan<byte> contents, string expectedContentType)
    {
        if (!_contentTypeToFiletype.TryGetValue(expectedContentType, out var expectedFiletype))
        {
            throw new ArgumentException($"{expectedContentType} is not a recognized type");
        }

        var actualPrefix = contents.GetStart(_headerSize);
        if (!HasRequiredHeader(actualPrefix))
        {
            return Filetype.Unknown;
        }

        var actualTrailer = contents.GetEnd(_expectedSuffix.Length + _trailerBuffer);
        if (!actualTrailer.StartsWith(_expectedSuffix))
        {
            return Filetype.Unknown;
        }

        return ContainsContentType(contents, expectedContentType)
            ? expectedFiletype
            : Filetype.Unknown;
    }

    private bool HasRequiredHeader(ReadOnlySpan<byte> prefix)
    {
        var knownHeaders = new[]
        {
            new byte[] { 0x50, 0x4B, 0x03, 0x04, 0x14, 0x00, 0x06, 0x00, }, // Native Excel and Word files
            new byte[] { 0x50, 0x4B, 0x03, 0x04, 0x14, 0x00, 0x08, 0x08, }, // Google Docs and Libre Office export to both xlsx and docx files
            new byte[] { 0x50, 0x4B, 0x03, 0x04, 0x14, 0x00, 0x00, 0x00, }, // Numbers export to xlsx
        };

        foreach (var possibility in knownHeaders)
        {
            if (new ReadOnlySpan<byte>(possibility).SequenceEqual(prefix))
            {
                return true;
            }
        }

        return false;
    }

    protected bool ContainsContentType(ReadOnlySpan<byte> contents, string contentType)
    {
        using (var ms = new MemoryStream(contents.ToArray()))
        using (var ooA = new ZipArchive(ms, ZipArchiveMode.Read))
        {
            var contentTypes = ooA.ExtractContentTypes();
            var fullContentType = $"PartName=\"/{contentType}/";
            return contentTypes.Contains(fullContentType, StringComparison.OrdinalIgnoreCase);
        }
    }

    protected bool ContainsContentType(string path, string contentType)
    {
        using (var zipArchive = ZipFile.OpenRead(path))
        {
            var contentTypes = zipArchive.ExtractContentTypes();
            var fullContentType = $"PartName=\"/{contentType}/";
            return contentTypes.Contains(fullContentType, StringComparison.OrdinalIgnoreCase);
        }
    }
}