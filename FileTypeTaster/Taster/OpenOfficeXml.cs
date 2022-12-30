namespace FileTypeTaster.Taster;

public class OpenOfficeXml :
    IFiletypeTaster
{
    private readonly FilesystemOffsetReader _reader;

    public OpenOfficeXml(FilesystemOffsetReader reader)
    {
        _reader = reader;
    }

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    /// <remarks>50 4B 03 04 14 00 06 00	 	PK......
    /// DOCX, PPTX, XLSX	 	                Microsoft Office Open XML Format (OOXML) Document
    /// NOTE: There is no subheader for MS OOXML files as there is with DOC, PPT, and XLS files. To better understand the format of these files, rename any
    /// OOXML file to have a .ZIP extension and then unZIP the file; look at the resultant file named [Content_Types].xml to see the content types.
    /// In particular, look for the Override PartName= tag, where you will find word, ppt, or xl, respectively.
    /// Trailer: Look for 50 4B 05 06 (PK..) followed by 18 additional bytes at the end of the file.</remarks>
    /// <param name="path"></param>
    /// <returns></returns>
    public async Task<Filetype> TastesLikeAsync(string path)
    {
        const int headerSize = 8;
        var headers = new[]
        {
            new byte[] { 0x50, 0x4B, 0x03, 0x04, 0x14, 0x00, 0x06, 0x00, }, // Native Excel and Word files
            new byte[] { 0x50, 0x4B, 0x03, 0x04, 0x14, 0x00, 0x08, 0x08, }, // Google Docs and Libre Office export to both xlsx and docx files
            new byte[] { 0x50, 0x4B, 0x03, 0x04, 0x14, 0x00, 0x00, 0x00, }, // Numbers export to xlsx
        };

        var actualPrefix = await _reader.GetStartAsync(path, headerSize);
        if (!headers.Any(p => p.SequenceEqual(actualPrefix)))
        {
            return Filetype.Unknown;
        }

        var suffix = new byte[] { 0x50, 0x4B, 0x05, 0x06, };
        const int trailerBuffer = 18;
        var actualTrailer = await _reader.GetEndAsync(path, suffix.Length + trailerBuffer);
        return actualTrailer.StartsWith(suffix)
            ? Filetype.UnspecifiedOpenOfficeXml
            : Filetype.Unknown;
    }
}