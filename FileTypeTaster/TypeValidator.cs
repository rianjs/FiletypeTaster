namespace FileTypeTaster;

public class TypeValidator
{
    private readonly TasteMapper _tasteMap;

    public TypeValidator(TasteMapper tasteMap)
    {
        _tasteMap = tasteMap;
    }

    public async Task<Filetype> GetTypeAsync(string path)
    {
        if (string.IsNullOrWhiteSpace(path)) throw new ArgumentNullException(nameof(path));

        var fileExtension = Path.GetExtension(path);
        var expected = _tasteMap.GetTastePairing(fileExtension);
        var filetype = await expected.Taster.TastesLikeAsync(path);
        var isAccurate = filetype == expected.Filetype;
        return isAccurate
            ? expected.Filetype
            : Filetype.Unknown;
    }

    public Filetype GetType(string path)
    {
        if (string.IsNullOrWhiteSpace(path)) throw new ArgumentNullException(nameof(path));

        var fileExtension = Path.GetExtension(path);
        var expected = _tasteMap.GetTastePairing(fileExtension);
        var contents = File.ReadAllBytes(path);
        var filetype = expected.Taster.TastesLike(contents);
        var isAccurate = filetype == expected.Filetype;
        return isAccurate
            ? expected.Filetype
            : Filetype.Unknown;
    }
}