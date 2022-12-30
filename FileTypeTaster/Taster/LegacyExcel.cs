using FileTypeTaster.Reader;

namespace FileTypeTaster.Taster;

public class LegacyExcel :
    IFiletypeTaster
{
    private readonly FilesystemOffsetReader _reader;
    private const int _offset = 512;
    private const int _longestArray = 8;

    public LegacyExcel(FilesystemOffsetReader reader)
    {
        _reader = reader;
    }

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    /// <remarks>
    /// All possibilities have a 512 (0x200) byte offset. Then one of the following possibilities:
    /// 09 08 10 00 00 06 05 00
    /// FD FF FF FF 20 00 00 00
    /// FD FF FF FF nn 00
    /// FD FF FF FF nn 02
    /// nn values are not noted to be constrained in any way, so we can just ignore their values
    /// </remarks>
    /// <param name="path"></param>
    /// <returns></returns>
    public async Task<Filetype> TastesLikeAsync(string path)
    {
        var checkSequence = await _reader.ReadBytesAsync(path, _offset, _longestArray);
        return GetFiletype(checkSequence);
    }

    public Filetype TastesLike(ReadOnlySpan<byte> contents)
    {
        var slice = contents.ReadBytes(_offset, _longestArray);
        return GetFiletype(slice);
    }

    private Filetype GetFiletype(ReadOnlySpan<byte> subsequence)
    {
        var simpleSigs = new[]
        {
            new byte[] { 0x09, 0x08, 0x10, 0x00, 0x00, 0x06, 0x05, 0x00, },
            new byte[] { 0xFD, 0xFF, 0xFF, 0xFF, 0x20, 0x00, 0x00, 0x00, },
        };

        foreach (var sig in simpleSigs)
        {
            if (subsequence.SequenceEqual(sig))
            {
                return Filetype.LegacyExcel;
            }
        }

        var expectedPrefix = new byte[] { 0xFD, 0xFF, 0xFF, 0xFF, };
        if (!subsequence.StartsWith(expectedPrefix))
        {
            return Filetype.Unknown;
        }

        const int wildcardCount = 1;
        var actualSuffix = subsequence[expectedPrefix.Length + wildcardCount];
        var result = actualSuffix is 0x00 or 0x02 or 0x07;    // Empirically, 0x07 is also a valid value based on testing
        return result
            ? Filetype.LegacyExcel
            : Filetype.Unknown;
    }
}