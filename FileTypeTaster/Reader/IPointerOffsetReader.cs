namespace FileTypeTaster.Reader;

/// <summary>
/// An interface for reading pointer-based byte ranges from files. This could be a filesystem via the FilesystemOffsetReader, or could be implemented in your
/// application via a cloud object store like S3 via an S3OffsetReader, or similar.
///
/// For uses where the contents of the file or object is already in memory, which may occur in a web service where someone has uploaded a document, you may wish
/// to use the PinnedMemoryOffsetReader which works with bytes that are in application memory.
/// </summary>
public interface IPointerOffsetReader
{
    Task<byte[]> ReadBytesAsync(string path, long offset, int count);
    Task<byte[]> GetStartAsync(string path, int count);
    Task<byte[]> GetEndAsync(string path, int offsetFromRear);
    Task<byte[]> GetAllBytesAsync(string path);

}