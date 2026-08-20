namespace DataFetcher
{
    public class FetcherConfig
    {
        public string OutputDirectory { get; set; } = "";

        public List<SpreadsheetConfig> Spreadsheets { get; set; } = new();
    }

    public class SpreadsheetConfig
    {
        public string Name { get; set; } = "";

        public string SpreadsheetId { get; set; } = "";
    }
}