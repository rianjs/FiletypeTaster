using FileTypeTaster.Reader;
using FileTypeTaster.Taster;

namespace FileTypeTaster;

public record TastePairing
{
    public string FileExtension { get; init; } = null!;
    public IFiletypeTaster Taster { get; init; } = null!;
    public Filetype Filetype { get; init; }
}

public class TasterFactory
{
    private readonly Dictionary<string, TastePairing> _tastePairings;

    public TasterFactory(IPointerOffsetReader offsetReader)
    {
        ArgumentNullException.ThrowIfNull(offsetReader);
        _tastePairings = new List<TastePairing>
        {
            new() { FileExtension = ".pdf", Taster = new Pdf(offsetReader), Filetype = Filetype.Pdf },
            new() { FileExtension = ".xls", Taster = new LegacyExcel(offsetReader), Filetype = Filetype.LegacyExcel },
            new() { FileExtension = ".xlsx", Taster = new Excel(offsetReader), Filetype = Filetype.Excel },
            new() { FileExtension = ".docx", Taster = new Word(offsetReader), Filetype = Filetype.Word },
            new() { FileExtension = ".pptx", Taster = new Powerpoint(offsetReader), Filetype = Filetype.Powerpoint },
            new() { FileExtension = ".doc", Taster = new LegacyWord(offsetReader), Filetype = Filetype.LegacyWord },
            new() { FileExtension = ".csv", Taster = new Csv(offsetReader), Filetype = Filetype.Csv },
        }
        .ToDictionary(p => p.FileExtension, StringComparer.OrdinalIgnoreCase);
    }

    public TasterFactory()
        : this(new EmptyPointerOffsetReader()) { }

    public IFiletypeTaster GetTaster(string fileExtension)
        => GetTastePairing(fileExtension).Taster;

    public Filetype GetExpectedFiletype(string fileExtension)
        => GetTastePairing(fileExtension).Filetype;

    public bool IsSupportedExtension(string fileExtension)
        => _tastePairings.ContainsKey(fileExtension);

    public TastePairing GetTastePairing(string fileExtension)
        => _tastePairings.TryGetValue(fileExtension, out var expected)
            ? expected
            : throw new ArgumentException($"{fileExtension} is not a supported file extension");
}