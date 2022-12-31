using FileTypeTaster.Reader;
using Microsoft.VisualBasic.FileIO;

namespace FileTypeTaster.Taster;

public class Csv :
    IFiletypeTaster
{
    private readonly IPointerOffsetReader _reader;
    private const int _minLineCount = 3;
    private const int _minColumnCount = 2;

    public Csv(IPointerOffsetReader reader)
    {
        _reader = reader;
    }

    public async Task<Filetype> TastesLikeAsync(string path)
    {
        var contents = await _reader.GetAllBytesAsync(path);
        return TastesLike(contents);
    }

    public Filetype TastesLike(ReadOnlySpan<byte> contents)
    {
        var lineCount = 0;
        using (var ms = new MemoryStream(contents.ToArray()))
        using (var parser = new TextFieldParser(ms))
        {
            parser.TextFieldType = FieldType.Delimited;
            parser.SetDelimiters(",");
            while (!parser.EndOfData && lineCount < _minLineCount)
            {
                var fields = parser.ReadFields();
                if (fields?.Length is null || fields.Length < _minColumnCount)
                {
                    continue;
                }

                // This parser will do weird stuff with binary files, so we have to check to make sure the contents are all numbers, letters, and symbols
                foreach (var field in fields)
                {
                    foreach (char c in field)
                    {
                        if (!char.IsLetterOrDigit(c) && !char.IsSymbol(c) && !char.IsWhiteSpace(c) && !char.IsPunctuation(c))
                        {
                            return Filetype.Unknown;
                        }
                    }
                }

                lineCount++;
            }
        }

        return lineCount < _minLineCount
            ? Filetype.Unknown
            : Filetype.Csv;
    }
}