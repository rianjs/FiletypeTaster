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
        var expected = _tasteMap.GetTastePairing(Path.GetExtension(path));
        var isAccurate = await expected.Taster.IsTypeAsync(path);
        if (isAccurate)
        {
            return expected.Filetype;
        }

        // Do something
        Console.WriteLine($"Expected type was {expected.Filetype} because the extension was {fileExtension} for path {path}");
        return Filetype.Unknown;
    }
}