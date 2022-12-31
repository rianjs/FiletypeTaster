# Usage

Tasters will return `Filetype.Unknown` if you mismatch taster with filetype. So an Excel spreadsheet with a CSV taster will return `Filetype.Unknown`.

## Supported filetypes
* .xlsx - Excel
* .pdf - PDF
* .xls - Legacy Excel
* .docx - Word
* .doc - Legacy Word
* .pptx  - Powerpoint
* .csv - CSV

Implementing other filetypes in the `Taster` namespace would be fairly straightforward to do by looking at GCK's file definitions.

## Local use
Doing stuff in a context where you have a local disk that the program knows about:

```csharp
var fsOffsetReader = new FilesystemOffsetReader();
var tasters = new TasterFactory(fsOffsetReader);
var someFile = Path.Combine("/Users/username/Documents/some-spreadsheet.xlsx");
var taster = tasters.GetTaster(Path.GetExtension(someFile));
var result = await taster.TastesLikeAsync(someFile);    // Filetype.Excel
```

## In-memory usage
This is more in line with how you'd use this in the event that someone `POST`s a file to a web application, or similar. It's a little more awkward to demonstrate, because this is a console app. But the "I have the data in memory already" API looks like this:

```csharp
var nullReader = new EmptyPointerOffsetReader();
var tasters = new TasterFactory(nullReader);

// This would have come from the POST body, along with any other metadata like the
// filename from the machine that POSTed it
// Normally you'd have the file contents in memory already, but this is a console app, so...
var filename = "some-excel-file.xlsx";
var fileContents = File.ReadAllBytes(filename); // Yes, you can just give it a byte array, too


// If someone is POSTing something to your endpoint, you should know what its file extension is
// We'll assume it's an Excel file again
var taster = tasters.GetTaster(Path.GetExtension(filename));
var result = taster.TastesLike(fileContents);   // Filetype.Excel
```

* `EmptyPointerOffsetReader` has methods that always return empty byte arrays
* Instantiating a `TasterFactory` with no constructor args will use an `EmptyPointerOffsetReader` automagically

## Implementing a cloud object store offset reader
If you're using S3, you can implement an `IPointerOffsetReader`, and give that to the `TasterFactory`. S3 supports reading byte ranges from objects, which should cut down considerably on IO latency and data transfer. Each taster knows which byte ranges it needs from the file, and will only request those ranges necessary for each taster to do its thing.

So go ahead and create an S3 offset reader in your consuming application!

# Performance
For files under 1MB, you should see < 10ms performance for both usage types. I haven't tested with larger files. Cloud object storage will obviously be slower due to network latency.

# Design notes
This was designed mostly for cloud usage. My use case is "validate uploaded files", where these quick checks are the first stage of a more comprehensive "is this malware?" interrogation / processing pipeline.

I intentionally didn't implement a `TastesLike()` method that can take any file and run the whole collection of tasters:
* I wanted this as narrowly-scoped as possible for security reasons
* Files have similar signatures in many cases, which leads to indeterminate results
* From a security perspective, it's best to have expectations come from the outside world, rather than using "it could be anything" as one's starting point
* I tested the file types pretty extensively

# About and Credit
This library is a fairly straightforward implementation of several of [GCK's forensic signature definitions](https://www.garykessler.net/library/file_sigs.html#acks). I extended the collection based on empirical testing: exporting from common apps like Apple's office suite, and Google Docs/Workspace, and examining the binary contents to find little differences between the programs that technically "own" the spec, and those that merely implement it.

You will find those notes alongside GCK's original investigative work.
