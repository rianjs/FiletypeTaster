namespace FileTypeTaster.Taster;

public interface IFiletypeTaster
{
    Task<bool> IsTypeAsync(string path);
}