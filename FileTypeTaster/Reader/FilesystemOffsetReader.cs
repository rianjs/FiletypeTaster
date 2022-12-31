namespace FileTypeTaster.Reader;

public class FilesystemOffsetReader :
    IPointerOffsetReader
{
    public Task<byte[]> ReadBytesAsync(string path, long offset, int count)
    {
        using (var fs = new FileStream(path, FileMode.Open))
        using (var reader = new BinaryReader(fs))
        {
            reader.BaseStream.Seek(offset, SeekOrigin.Begin);
            return Task.FromResult(reader.ReadBytes(count));
        }
    }

    public Task<byte[]> GetStartAsync(string path, int count)
        => ReadBytesAsync(path, 0, count);

    public Task<byte[]> GetEndAsync(string path, int offsetFromRear)
    {
        var fileLength = new FileInfo(path).Length;
        var fromRear = fileLength - offsetFromRear;
        return ReadBytesAsync(path, fromRear, offsetFromRear);
    }

    public Task<byte[]> GetAllBytesAsync(string path)
        => File.ReadAllBytesAsync(path);
}