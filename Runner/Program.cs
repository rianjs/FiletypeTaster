using System.Diagnostics;
using FileTypeTaster;
using FileTypeTaster.Reader;

namespace Runner;

public class Program
{
    public static async Task Main(string[] args)
    {
        var fsReader = new FilesystemOffsetReader();
        var testFiles = new List<string>
        {
            Path.Combine("/Users/rianjs/Downloads/mlb_players.csv"),
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

        var tasteFactory = new TasterFactory(new FilesystemOffsetReader());

        foreach (var file in testFiles)
        {
            var taster = tasteFactory.GetTaster(Path.GetExtension(file));
            var filetype = await taster.TastesLikeAsync(file);
            Console.WriteLine($"FILE: File {file} matches the file signature for {filetype}");
            var f2 = taster.TastesLike(File.ReadAllBytes(file));
            Console.WriteLine($"SPAN: File {f2} matches the file signature for {filetype}");
            Console.WriteLine($"======================== Match? {filetype == f2}");
        }

        Console.WriteLine("================================================================================================================================================");

        Console.WriteLine("Beginning timed FILE (async) run");
        var timer = Stopwatch.StartNew();
        foreach (var file in testFiles)
        {
            var taster = tasteFactory.GetTaster(Path.GetExtension(file));
            var filetype = await taster.TastesLikeAsync(file);
            Console.WriteLine($"FILE: File {file} matches the file signature for {filetype}");
        }
        timer.Stop();
        Console.WriteLine($"FILE (async) run completed in {timer.ElapsedMilliseconds:N0}ms");

        Console.WriteLine("Beginning timed SPAN (sync) run");
        timer = Stopwatch.StartNew();
        foreach (var file in testFiles)
        {
            var taster = tasteFactory.GetTaster(Path.GetExtension(file));
            var filetype = taster.TastesLike(File.ReadAllBytes(file));
            Console.WriteLine($"SPAN: File {filetype} matches the file signature for {filetype}");
        }
        timer.Stop();
        Console.WriteLine($"SPAN (sync) run completed in {timer.ElapsedMilliseconds:N0}ms");
    }
}