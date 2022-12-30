using System.IO.Compression;

namespace FileTypeTaster.Taster;

public class Powerpoint :
    OpenOfficeXml,
    IFiletypeTaster
{
    public Powerpoint(FilesystemOffsetReader reader) : base(reader) { }

    public async Task<Filetype> TastesLikeAsync(string path)
    {
        var baseResult = await base.TastesLikeAsync(path);
        if (baseResult is not Filetype.UnspecifiedOpenOfficeXml)
        {
            return Filetype.Unknown;
        }

        var ooArchive = ZipFile.OpenRead(path);
        var contents = ooArchive.ExtractContentTypes();
        return contents.Contains("PartName=\"/ppt/", StringComparison.OrdinalIgnoreCase)
            ? Filetype.Powerpoint
            : Filetype.Unknown;
    }
}

// Powerpoint [Content_Type].xml files look like this:
// <?xml version="1.0" encoding="UTF-8"?>
// <Types xmlns="http://schemas.openxmlformats.org/package/2006/content-types">
//    <Default Extension="jpeg" ContentType="image/jpeg" />
//    <Default Extension="png" ContentType="image/png" />
//    <Default Extension="rels" ContentType="application/vnd.openxmlformats-package.relationships+xml" />
//    <Default Extension="xml" ContentType="application/xml" />
//    <Override PartName="/ppt/presentation.xml" ContentType="application/vnd.openxmlformats-officedocument.presentationml.presentation.main+xml" />
//    <Override PartName="/ppt/slideMasters/slideMaster1.xml" ContentType="application/vnd.openxmlformats-officedocument.presentationml.slideMaster+xml" />
//    <Override PartName="/ppt/slides/slide1.xml" ContentType="application/vnd.openxmlformats-officedocument.presentationml.slide+xml" />
//    <Override PartName="/ppt/slides/slide2.xml" ContentType="application/vnd.openxmlformats-officedocument.presentationml.slide+xml" />
//    <Override PartName="/ppt/presProps.xml" ContentType="application/vnd.openxmlformats-officedocument.presentationml.presProps+xml" />
//    <Override PartName="/ppt/viewProps.xml" ContentType="application/vnd.openxmlformats-officedocument.presentationml.viewProps+xml" />
//    <Override PartName="/ppt/theme/theme1.xml" ContentType="application/vnd.openxmlformats-officedocument.theme+xml" />
//    <Override PartName="/ppt/tableStyles.xml" ContentType="application/vnd.openxmlformats-officedocument.presentationml.tableStyles+xml" />
//    <Override PartName="/ppt/slideLayouts/slideLayout1.xml" ContentType="application/vnd.openxmlformats-officedocument.presentationml.slideLayout+xml" />
//    <Override PartName="/ppt/slideLayouts/slideLayout2.xml" ContentType="application/vnd.openxmlformats-officedocument.presentationml.slideLayout+xml" />
//    <Override PartName="/ppt/slideLayouts/slideLayout3.xml" ContentType="application/vnd.openxmlformats-officedocument.presentationml.slideLayout+xml" />
//    <Override PartName="/ppt/slideLayouts/slideLayout4.xml" ContentType="application/vnd.openxmlformats-officedocument.presentationml.slideLayout+xml" />
//    <Override PartName="/ppt/slideLayouts/slideLayout5.xml" ContentType="application/vnd.openxmlformats-officedocument.presentationml.slideLayout+xml" />
//    <Override PartName="/ppt/slideLayouts/slideLayout6.xml" ContentType="application/vnd.openxmlformats-officedocument.presentationml.slideLayout+xml" />
//    <Override PartName="/ppt/slideLayouts/slideLayout7.xml" ContentType="application/vnd.openxmlformats-officedocument.presentationml.slideLayout+xml" />
//    <Override PartName="/ppt/slideLayouts/slideLayout8.xml" ContentType="application/vnd.openxmlformats-officedocument.presentationml.slideLayout+xml" />
//    <Override PartName="/ppt/slideLayouts/slideLayout9.xml" ContentType="application/vnd.openxmlformats-officedocument.presentationml.slideLayout+xml" />
//    <Override PartName="/ppt/slideLayouts/slideLayout10.xml" ContentType="application/vnd.openxmlformats-officedocument.presentationml.slideLayout+xml" />
//    <Override PartName="/ppt/slideLayouts/slideLayout11.xml" ContentType="application/vnd.openxmlformats-officedocument.presentationml.slideLayout+xml" />
//    <Override PartName="/ppt/changesInfos/changesInfo1.xml" ContentType="application/vnd.ms-powerpoint.changesinfo+xml" />
//    <Override PartName="/ppt/revisionInfo.xml" ContentType="application/vnd.ms-powerpoint.revisioninfo+xml" />
//    <Override PartName="/docProps/core.xml" ContentType="application/vnd.openxmlformats-package.core-properties+xml" />
//    <Override PartName="/docProps/app.xml" ContentType="application/vnd.openxmlformats-officedocument.extended-properties+xml" />
// </Types>