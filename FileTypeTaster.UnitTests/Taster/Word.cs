using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FileTypeTaster.Reader;
using FileTypeTaster.Taster;
using NUnit.Framework;

namespace FileTypeTaster.UnitTests.Taster;

public class Word
{
    private const string _ext = ".docx";
    private const Filetype _expectedType = Filetype.Word;
    private static readonly string[] _testFiles = Directory.GetFiles("TestFiles");
    private static readonly TasterFactory _tasterFactory = new(new FilesystemOffsetReader());
    private static readonly IFiletypeTaster _taster = _tasterFactory.GetTaster(_ext);

    [Test]
    public async Task PathTastingSucceeds()
    {
        foreach (var file in _testFiles.Where(f => f.EndsWith(_ext, StringComparison.OrdinalIgnoreCase)))
        {
            var result = await _taster.TastesLikeAsync(file);
            Assert.AreEqual(_expectedType, result);
        }
    }

    [Test]
    public void BlobTastingSucceeds()
    {
        foreach (var file in _testFiles.Where(f => f.EndsWith(_ext, StringComparison.OrdinalIgnoreCase)))
        {
            var result = _taster.TastesLike(File.ReadAllBytes(file));
            Assert.AreEqual(_expectedType, result);
        }
    }

    [Test]
    public async Task IrrelevantPathsFail()
    {
        foreach (var file in _testFiles.Where(f => !f.EndsWith(_ext, StringComparison.OrdinalIgnoreCase)))
        {
            var result = await _taster.TastesLikeAsync(file);
            Assert.AreEqual(Filetype.Unknown, result);
        }
    }

    [Test]
    public void IrrelevantBlobsFail()
    {
        foreach (var file in _testFiles.Where(f => !f.EndsWith(_ext, StringComparison.OrdinalIgnoreCase)))
        {
            var result = _taster.TastesLike(File.ReadAllBytes(file));
            Assert.AreEqual(Filetype.Unknown, result);
        }
    }
}