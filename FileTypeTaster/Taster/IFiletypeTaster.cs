namespace FileTypeTaster.Taster;

public interface IFiletypeTaster
{
    Task<Filetype> TastesLikeAsync(string path);
    Filetype TastesLike(ReadOnlySpan<byte> contents);
}