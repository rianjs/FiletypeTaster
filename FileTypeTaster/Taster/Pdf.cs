using FileTypeTaster.Reader;

namespace FileTypeTaster.Taster;

public class Pdf :
    IFiletypeTaster
{
    private readonly IPointerOffsetReader _reader;

    public Pdf(IPointerOffsetReader reader)
    {
        _reader = reader;
    }

    private static readonly byte[] _preamble = { 0x25, 0x50, 0x44, 0x46 };  // %PDF
    private const int _longestBackmarker = 9;

    public async Task<Filetype> TastesLikeAsync(string path)
    {
        var front = await _reader.GetStartAsync(path, _preamble.Length);
        if (!front.SequenceEqual(_preamble))
        {
            return Filetype.Unknown;
        }

        var eofChunk = await _reader.GetEndAsync(path, _longestBackmarker);
        return HasRequiredSuffix(eofChunk)
            ? Filetype.Pdf
            : Filetype.Unknown;
    }

    public Filetype TastesLike(ReadOnlySpan<byte> contents)
    {
        var front = contents.GetStart(_preamble.Length);
        if (!front.SequenceEqual(_preamble))
        {
            return Filetype.Unknown;
        }

        var eofChunk = contents.GetEnd(_longestBackmarker);
        return HasRequiredSuffix(eofChunk)
            ? Filetype.Pdf
            : Filetype.Unknown;
    }

    private bool HasRequiredSuffix(ReadOnlySpan<byte> eofChunk)
    {
        var suffixes = new[]
        {
            new byte[]{0x0A, 0x25, 0x25, 0x45, 0x4F, 0x46}, // (.%%EOF)
            new byte[]{0x0A, 0x25, 0x25, 0x45, 0x4F, 0x46, 0x0A,}, // (.%%EOF.)
            new byte[]{0x0D, 0x0A, 0x25, 0x25, 0x45, 0x4F, 0x46, 0x0D, 0x0A,}, // (..%%EOF..)
            new byte[]{0x0D, 0x25, 0x25, 0x45, 0x4F, 0x46, 0x0D,} // (.%%EOF.)
        };

        foreach (var possibility in suffixes)
        {
            if (eofChunk.EndsWith(possibility))
            {
                return true;
            }
        }

        return false;
    }
}