namespace FileTypeTaster.Taster;

public class Pdf :
    IFiletypeTaster
{
    private readonly FilesystemOffsetReader _reader;

    public Pdf(FilesystemOffsetReader reader)
    {
        _reader = reader;
    }

    private static readonly byte[] _preamble = { 0x25, 0x50, 0x44, 0x46 };  // %PDF

    private static readonly List<byte[]> _trailers = new()
    {
        new byte[]{0x0A, 0x25, 0x25, 0x45, 0x4F, 0x46}, // (.%%EOF)
        new byte[]{0x0A, 0x25, 0x25, 0x45, 0x4F, 0x46, 0x0A,}, // (.%%EOF.)
        new byte[]{0x0D, 0x0A, 0x25, 0x25, 0x45, 0x4F, 0x46, 0x0D, 0x0A,}, // (..%%EOF..)
        new byte[]{0x0D, 0x25, 0x25, 0x45, 0x4F, 0x46, 0x0D,} // (.%%EOF.)
    };

    public bool IsType(string path)
    {
        var front = _reader.GetFront(path, _preamble.Length);
        if (!front.SequenceEqual(_preamble))
        {
            return false;
        }

        const int longestBackmarker = 9;
        var eofChunk = _reader.GetBack(path, longestBackmarker);
        return _trailers.Any(possibility => eofChunk.EndsWith(possibility));
    }
}