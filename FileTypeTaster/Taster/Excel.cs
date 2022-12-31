using FileTypeTaster.Reader;

namespace FileTypeTaster.Taster;

public class Excel :
    OpenOfficeXml,
    IFiletypeTaster
{
    private const string _contentType = "xl";
    public Excel(IPointerOffsetReader reader) : base(reader) { }

    public Task<Filetype> TastesLikeAsync(string path)
        => base.TastesLikeAsync(path, _contentType);

    public Filetype TastesLike(ReadOnlySpan<byte> contents)
        => base.TastesLike(contents, _contentType);
}

// Excel [Content_Type].xml files look like this:
// <?xml version="1.0" encoding="UTF-8"?>
// <Types xmlns="http://schemas.openxmlformats.org/package/2006/content-types">
//    <Default Extension="xml" ContentType="application/xml" />
//    <Default Extension="rels" ContentType="application/vnd.openxmlformats-package.relationships+xml" />
//    <Default Extension="png" ContentType="image/png" />
//    <Default Extension="jpeg" ContentType="image/jpeg" />
//    <Override PartName="/_rels/.rels" ContentType="application/vnd.openxmlformats-package.relationships+xml" />
//    <Override PartName="/xl/workbook.xml" ContentType="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml" />
//    <Override PartName="/xl/styles.xml" ContentType="application/vnd.openxmlformats-officedocument.spreadsheetml.styles+xml" />
//    <Override PartName="/xl/_rels/workbook.xml.rels" ContentType="application/vnd.openxmlformats-package.relationships+xml" />
//    <Override PartName="/xl/worksheets/sheet1.xml" ContentType="application/vnd.openxmlformats-officedocument.spreadsheetml.worksheet+xml" />
//    <Override PartName="/xl/sharedStrings.xml" ContentType="application/vnd.openxmlformats-officedocument.spreadsheetml.sharedStrings+xml" />
//    <Override PartName="/docProps/core.xml" ContentType="application/vnd.openxmlformats-package.core-properties+xml" />
//    <Override PartName="/docProps/app.xml" ContentType="application/vnd.openxmlformats-officedocument.extended-properties+xml" />
// </Types>
