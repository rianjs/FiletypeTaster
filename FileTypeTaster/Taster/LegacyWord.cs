using FileTypeTaster.Reader;

namespace FileTypeTaster.Taster;

public class LegacyWord :
    IFiletypeTaster
{
    private readonly FilesystemOffsetReader _reader;

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
    public Filetype TastesLike(string path)
    {
        const int offset = 512;
        var expectedSig = new byte[] { 0xEC, 0xA5, 0xC1, 0x00, };
        var checkSequence = _reader.ReadBytesAsync(path, offset, 4);
        return checkSequence.SequenceEqual(expectedSig)
            ? Filetype.LegacyWord
            : Filetype.Unknown;
    }
}