using FileTypeTaster.Reader;

namespace FileTypeTaster.Taster;

public class LegacyWord :
    IFiletypeTaster
{
    private readonly IPointerOffsetReader _reader;
    private const int _length = 4;
    private const int _offset = 512;

    public LegacyWord(FilesystemOffsetReader reader)
    {
        _reader = reader;
    }

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    /// <remarks>
    /// All olf MS Office formats before Open Office XML had a 512 (0x200) byte offset. Then a program-specific sequence.
    /// EC A5 C1 00
    /// nn values are not noted to be constrained in any way, so we can just ignore their values
    /// </remarks>
    /// <param name="path"></param>
    /// <returns></returns>
    public async Task<Filetype> TastesLikeAsync(string path)
    {
        var checkSequence = await _reader.ReadBytesAsync(path, _offset, _length);
        return GetFiletype(checkSequence);
    }

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    /// <remarks>
    /// All olf MS Office formats before Open Office XML had a 512 (0x200) byte offset. Then a program-specific sequence.
    /// EC A5 C1 00
    /// nn values are not noted to be constrained in any way, so we can just ignore their values
    /// </remarks>
    /// <param name="contents"></param>
    /// <returns></returns>
    public Filetype TastesLike(ReadOnlySpan<byte> contents)
    {
        var checkSequence = contents.ReadBytes(_offset, _length);
        return GetFiletype(checkSequence);
    }

    private Filetype GetFiletype(ReadOnlySpan<byte> subsequence)
    {
        var expectedSig = new byte[] { 0xEC, 0xA5, 0xC1, 0x00, };
        return subsequence.SequenceEqual(expectedSig)
            ? Filetype.LegacyWord
            : Filetype.Unknown;
    }
}