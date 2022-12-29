using System.IO.Compression;

namespace FileTypeTaster.Taster;

public class Word :
    OpenOfficeXml
{
    public Word(FilesystemOffsetReader reader) : base(reader) { }

    public override bool IsType(string path)
    {
        if (!base.IsType(path))
        {
            return false;
        }

        var ooArchive = ZipFile.OpenRead(path);
        var contents = ooArchive.ExtractContentTypes();
        return contents.Contains("PartName=\"/word/", StringComparison.OrdinalIgnoreCase);
    }
}

// Word [Content_Type].xml files look like this.
// <?xml version="1.0" encoding="UTF-8"?>
// <Types xmlns="http://schemas.openxmlformats.org/package/2006/content-types">
//    <Default Extension="png" ContentType="image/png" />
//    <Default Extension="rels" ContentType="application/vnd.openxmlformats-package.relationships+xml" />
//    <Default Extension="xml" ContentType="application/xml" />
//    <Override PartName="/word/document.xml" ContentType="application/vnd.openxmlformats-officedocument.wordprocessingml.document.main+xml" />
//    <Override PartName="/customXml/itemProps1.xml" ContentType="application/vnd.openxmlformats-officedocument.customXmlProperties+xml" />
//    <Override PartName="/word/numbering.xml" ContentType="application/vnd.openxmlformats-officedocument.wordprocessingml.numbering+xml" />
//    <Override PartName="/word/styles.xml" ContentType="application/vnd.openxmlformats-officedocument.wordprocessingml.styles+xml" />
//    <Override PartName="/word/settings.xml" ContentType="application/vnd.openxmlformats-officedocument.wordprocessingml.settings+xml" />
//    <Override PartName="/word/webSettings.xml" ContentType="application/vnd.openxmlformats-officedocument.wordprocessingml.webSettings+xml" />
//    <Override PartName="/word/footnotes.xml" ContentType="application/vnd.openxmlformats-officedocument.wordprocessingml.footnotes+xml" />
//    <Override PartName="/word/endnotes.xml" ContentType="application/vnd.openxmlformats-officedocument.wordprocessingml.endnotes+xml" />
//    <Override PartName="/word/header1.xml" ContentType="application/vnd.openxmlformats-officedocument.wordprocessingml.header+xml" />
//    <Override PartName="/word/header2.xml" ContentType="application/vnd.openxmlformats-officedocument.wordprocessingml.header+xml" />
//    <Override PartName="/word/footer1.xml" ContentType="application/vnd.openxmlformats-officedocument.wordprocessingml.footer+xml" />
//    <Override PartName="/word/footer2.xml" ContentType="application/vnd.openxmlformats-officedocument.wordprocessingml.footer+xml" />
//    <Override PartName="/word/header3.xml" ContentType="application/vnd.openxmlformats-officedocument.wordprocessingml.header+xml" />
//    <Override PartName="/word/footer3.xml" ContentType="application/vnd.openxmlformats-officedocument.wordprocessingml.footer+xml" />
//    <Override PartName="/word/header4.xml" ContentType="application/vnd.openxmlformats-officedocument.wordprocessingml.header+xml" />
//    <Override PartName="/word/footer4.xml" ContentType="application/vnd.openxmlformats-officedocument.wordprocessingml.footer+xml" />
//    <Override PartName="/word/fontTable.xml" ContentType="application/vnd.openxmlformats-officedocument.wordprocessingml.fontTable+xml" />
//    <Override PartName="/word/theme/theme1.xml" ContentType="application/vnd.openxmlformats-officedocument.theme+xml" />
//    <Override PartName="/docProps/core.xml" ContentType="application/vnd.openxmlformats-package.core-properties+xml" />
//    <Override PartName="/docProps/app.xml" ContentType="application/vnd.openxmlformats-officedocument.extended-properties+xml" />
// </Types>