using System.Text.Json;
using DataFetcher;
using DataFetcher.JsonExporter;
using DataFetcher.Services;

Console.WriteLine("================================");
Console.WriteLine("     Google Sheet DataFetcher");
Console.WriteLine("================================");
Console.WriteLine();

const string configFile = "appsettings.json";
const string credentialsFile = "credentials.json";

if (!File.Exists(configFile))
{
    Console.WriteLine($"Missing: {configFile}");
    return;
}

if (!File.Exists(credentialsFile))
{
    Console.WriteLine($"Missing: {credentialsFile}");
    return;
}

string configJson = await File.ReadAllTextAsync(configFile);

// If error on this line, it because of the path in appsettings.json is wrong.
FetcherConfig? config = JsonSerializer.Deserialize<FetcherConfig>(configJson,
        new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

if (config == null)
{
    Console.WriteLine("Invalid configuration.");
    return;
}

var googleSheets =
    new GoogleSheetsService(credentialsFile);

var exporter =
    new JsonExporter();

foreach (SpreadsheetConfig spreadsheet in config.Spreadsheets)
{
    Console.WriteLine();
    Console.WriteLine(
        $"Spreadsheet: {spreadsheet.Name}");

    Console.WriteLine(
        $"ID: {spreadsheet.SpreadsheetId}");

    try
    {
        // Get every tab inside this spreadsheet
        List<string> sheetNames =
            await googleSheets.GetAllSheetNamesAsync(
                spreadsheet.SpreadsheetId);

        Console.WriteLine(
            $"Found {sheetNames.Count} tabs.");

        foreach (string sheetName in sheetNames)
        {
            Console.WriteLine(
                $"  Fetching: {sheetName}");

            var data =
                await googleSheets.FetchSheetAsync(
                    spreadsheet.SpreadsheetId,
                    sheetName);

            string outputPath =
                Path.Combine(
                    config.OutputDirectory,
                    $"{sheetName}.json");

            await exporter.ExportAsync(
                data,
                outputPath);

            Console.WriteLine(
                $"  ✓ {sheetName}.json");
        }
    }
    catch (Exception exception)
    {
        Console.WriteLine(
            $"Failed: {spreadsheet.Name}");

        Console.WriteLine(
            exception.Message);
    }
}

Console.WriteLine();
Console.WriteLine("================================");
Console.WriteLine("          Complete!");
Console.WriteLine("================================");