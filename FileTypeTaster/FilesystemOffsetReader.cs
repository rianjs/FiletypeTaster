namespace FileTypeTaster;

public class FilesystemOffsetReader
{
    public byte[] ReadBytes(string path, long offset, int count)
    {
        var buffer = new byte[count];
        using (var fs = new FileStream(path, FileMode.Open))
        using (var reader = new BinaryReader(fs))
        {
            reader.BaseStream.Seek(offset, SeekOrigin.Begin);
            reader.Read(buffer, 0, count);
        }

        return buffer;
    }

    public byte[] GetFront(string path, int count)
        => ReadBytes(path, 0, count);

    public byte[] GetBack(string path, int offsetFromRear)
    {
        var fileLength = new FileInfo(path).Length;
        var fromRear = fileLength - offsetFromRear;
        return ReadBytes(path, fromRear, offsetFromRear);
    }
}