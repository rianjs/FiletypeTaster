namespace FileTypeTaster;

public class FilesystemOffsetReader : IOffsetReader
{
    public Task<byte[]> ReadBytesAsync(string path, long offset, int count)
    {
        var buffer = new byte[count];
        using (var fs = new FileStream(path, FileMode.Open))
        using (var reader = new BinaryReader(fs))
        {
            reader.BaseStream.Seek(offset, SeekOrigin.Begin);
            reader.Read(buffer, 0, count);
        }

        return Task.FromResult(buffer);
    }

    public Task<byte[]> GetStartAsync(string path, int count)
        => ReadBytesAsync(path, 0, count);

    public Task<byte[]> GetEndAsync(string path, int offsetFromRear)
    {
        var fileLength = new FileInfo(path).Length;
        var fromRear = fileLength - offsetFromRear;
        return ReadBytesAsync(path, fromRear, offsetFromRear);
    }
}