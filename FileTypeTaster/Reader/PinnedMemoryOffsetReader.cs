namespace FileTypeTaster.Reader;

public static class PinnedMemoryOffsetReader
{
    public static ReadOnlySpan<byte> ReadBytes(this ReadOnlySpan<byte> contents, int offset, int count)
        => contents.Slice(offset, count);

    public static ReadOnlySpan<byte> GetStart(this ReadOnlySpan<byte> contents, int count)
        => contents.Slice(0, count);

    public static ReadOnlySpan<byte> GetEnd(this ReadOnlySpan<byte> contents, int offsetFromRear)
    {
        var start = contents.Length - offsetFromRear;
        return contents.Slice(start, offsetFromRear);
    }
}