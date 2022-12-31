namespace FileTypeTaster.Reader;

/// <summary>
/// In some cases, you don't need or want a pointer-based offset reader, because you'll always have the file contents in memory, and you'll only ever call
/// GetType with the file contents. In that case, just use this.
/// </summary>
public class EmptyPointerOffsetReader :
    IPointerOffsetReader
{
    public Task<byte[]> ReadBytesAsync(string path, long offset, int count)
        => Task.FromResult(Array.Empty<byte>());

    public Task<byte[]> GetStartAsync(string path, int count)
        => Task.FromResult(Array.Empty<byte>());

    public Task<byte[]> GetEndAsync(string path, int offsetFromRear)
        => Task.FromResult(Array.Empty<byte>());

    public Task<byte[]> GetAllBytesAsync(string path)
        => Task.FromResult(Array.Empty<byte>());
}