namespace FileTypeTaster;

public class TypeDeterminer
{
    private readonly TasteMapper _tasteMap;

    public TypeDeterminer(TasteMapper tasteMap)
    {
        _tasteMap = tasteMap;
    }

    public Filetype GetType(string path)
    {
        if (string.IsNullOrWhiteSpace(path)) throw new ArgumentNullException(nameof(path));

        var fileExtension = Path.GetExtension(path);
        var expected = _tasteMap.GetTastePairing(Path.GetExtension(path));
        var isAccurate = expected.Taster.IsType(path);
        if (isAccurate)
        {
            return expected.Filetype;
        }

        // Do something
        Console.WriteLine($"Expected type was {expected.Filetype} because the extension was {fileExtension} for path {path}");
        return Filetype.Unknown;
    }
}

public record FiletypeReport
{
    /// <summary> Based on the file extension </summary>
    public Filetype Expected { get; init; }
    public Filetype Actual { get; init; }
}

public enum Filetype
{
    Unknown,
    Pdf,
    LegacyExcel,
    Excel,
    Csv,
    Word,
}