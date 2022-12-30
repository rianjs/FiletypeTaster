namespace FileTypeTaster;

public interface IOffsetReader
{
    Task<byte[]> ReadBytesAsync(string path, long offset, int count);
    Task<byte[]> GetStartAsync(string path, int count);
    Task<byte[]> GetEndAsync(string path, int offsetFromRear);
}