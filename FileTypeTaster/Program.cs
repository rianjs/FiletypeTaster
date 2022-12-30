using System.Diagnostics;
using FileTypeTaster;
using FileTypeTaster.Taster;

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
            var filetype = determiner.GetTypeAsync(file);
            Console.WriteLine($"File {file} matches the file signature for {filetype}");
        }

        Console.WriteLine("Beginning timed run");
        var timer = Stopwatch.StartNew();
        foreach (var file in testFiles)
        {
            var filetype = determiner.GetTypeAsync(file);
            Console.WriteLine($"File {file} matches the file signature for {filetype}");
        }
        timer.Stop();
        Console.WriteLine($"Checked all files in {timer.ElapsedMilliseconds:N0}ms");
    }
}