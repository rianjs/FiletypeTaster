using System.Diagnostics;
using FileTypeTaster;
using FileTypeTaster.Reader;
using FileTypeTaster.Taster;

namespace Runner;

public class Program
{
    public static async Task Main(string[] args)
    {
        var fsReader = new FilesystemOffsetReader();
        var testFiles = new List<string>
        {
            Path.Combine("/Users/rianjs/Downloads/hobbit ch 1.doc"),
            Path.Combine("/Users/rianjs/Downloads/Iced coffee.doc"),
            Path.Combine("/Users/rianjs/Downloads/Iced coffee.docx"),
            Path.Combine("/Users/rianjs/Downloads/2022-12-5-Signal FT - Pentest-Laika Sales Order_19947.docx"),
            Path.Combine("/Users/rianjs/Downloads/2023 Life.xls"),
            Path.Combine("/Users/rianjs/Downloads/Sample-Spreadsheet-5000-rows.xls"),
            Path.Combine("/Users/rianjs/Downloads/SampleXLSFile_38kb.xls"),
            Path.Combine("/Users/rianjs/Downloads/2023 Life.xlsx"),
            Path.Combine("/Users/rianjs/Downloads/numbersSampleXLSFile_38kb.xlsx"),
            Path.Combine("/Users/rianjs/Downloads/libreSampleXLSFile_38kb.xlsx"),
            Path.Combine("/Users/rianjs/Downloads/E2E business banking platform vision HL summary 9.14.22.pptx"),
            Path.Combine("/Users/rianjs/Downloads/2023 Life norm.xlsx"),
            Path.Combine("/Users/rianjs/Downloads/Signal Monit Partner Agreement Northeast Bank 20221215.pdf"),
            Path.Combine("/Users/rianjs/Downloads/2022-10-19-Monit - Active Renewal-Laika Renewal Order_165559.pdf"),
            Path.Combine("/Users/rianjs/Downloads/2022-12-01 - AppleCare - HRTJN5EU1WFV.pdf"),
            Path.Combine("/Users/rianjs/Downloads/Extraco Banks - Vendor Questionnaire.pdf"),
            Path.Combine("/Users/rianjs/Downloads/rstockbower-4Q2022.pdf"),
        };

        var pairings = new List<TastePairing>
        {
            new() { FileExtension = ".pdf", Taster = new Pdf(fsReader), Filetype = Filetype.Pdf },
            new() { FileExtension = ".xls", Taster = new LegacyExcel(fsReader), Filetype = Filetype.LegacyExcel },
            new() { FileExtension = ".xlsx", Taster = new Excel(fsReader), Filetype = Filetype.Excel },
            new() { FileExtension = ".docx", Taster = new Word(fsReader), Filetype = Filetype.Word },
            new() { FileExtension = ".pptx", Taster = new Powerpoint(fsReader), Filetype = Filetype.Powerpoint },
            new() { FileExtension = ".doc", Taster = new LegacyWord(fsReader), Filetype = Filetype.LegacyWord },
        };

        var extensionMap = pairings.ToDictionary(p => p.FileExtension, p => p, StringComparer.OrdinalIgnoreCase);
        var tasteMapper = new TasteMapper(extensionMap);
        var determiner = new TypeValidator(tasteMapper);

        foreach (var file in testFiles)
        {
            var filetype = await determiner.GetTypeAsync(file);
            Console.WriteLine($"FILE: File {file} matches the file signature for {filetype}");
            var f2 = determiner.GetType(file);
            Console.WriteLine($"SPAN: File {f2} matches the file signature for {filetype}");
            Console.WriteLine($"======================== Match? {filetype == f2}");
        }

        Console.WriteLine("================================================================================================================================================");

        Console.WriteLine("Beginning timed FILE (async) run");
        var timer = Stopwatch.StartNew();
        foreach (var file in testFiles)
        {
            var filetype = await determiner.GetTypeAsync(file);
            Console.WriteLine($"FILE: File {file} matches the file signature for {filetype}");
        }
        timer.Stop();
        Console.WriteLine($"FILE (async) run completed in {timer.ElapsedMilliseconds:N0}ms");

        Console.WriteLine("Beginning timed SPAN (sync) run");
        timer = Stopwatch.StartNew();
        foreach (var file in testFiles)
        {
            var filetype = determiner.GetType(file);
            Console.WriteLine($"SPAN: File {filetype} matches the file signature for {filetype}");
        }
        timer.Stop();
        Console.WriteLine($"SPAN (sync) run completed in {timer.ElapsedMilliseconds:N0}ms");
    }
}