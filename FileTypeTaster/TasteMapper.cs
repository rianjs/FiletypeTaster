using FileTypeTaster.Taster;

namespace FileTypeTaster;

public record TastePairing
{
    public string FileExtension { get; init; }
    public IFiletypeTaster Taster { get; init; }
    public Filetype Filetype { get; init; }
}

public class TasteMapper
{
    private readonly Dictionary<string, TastePairing> _tastePairings;

    public TasteMapper(Dictionary<string, TastePairing> tastePairings)
    {
        _tastePairings = tastePairings;
    }

    public IFiletypeTaster GetTaster(string fileExtension)
        => GetTastePairing(fileExtension).Taster;

    public Filetype GetExpectedFiletype(string fileExtension)
        => GetTastePairing(fileExtension).Filetype;

    public bool IsSupportedExtension(string fileExtension)
    {
        try
        {
            var _ = GetTastePairing(fileExtension);
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }

    public TastePairing GetTastePairing(string fileExtension)
    {
        if (string.IsNullOrWhiteSpace(fileExtension)) throw new ArgumentNullException(nameof(fileExtension));
        return _tastePairings.TryGetValue(fileExtension, out var expected)
            ? expected
            : throw new ArgumentException($"{fileExtension} is not a supported file extension");
    }
}