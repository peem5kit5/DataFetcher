using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;

namespace DataFetcher.Services
{
    public class GoogleSheetsService
    {
        private readonly SheetsService service;

        public GoogleSheetsService(string credentialsPath)
        {
            GoogleCredential credential = GoogleCredential.FromFile(credentialsPath).CreateScoped(SheetsService.Scope.SpreadsheetsReadonly);

            service = new SheetsService(
                new BaseClientService.Initializer
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "MagicOnion DataFetcher"
                });
        }

        public async Task<List<string>> GetAllSheetNamesAsync(string spreadsheetId)
        {
            var request = service.Spreadsheets.Get(spreadsheetId);

            Spreadsheet spreadsheet = await request.ExecuteAsync();

            return spreadsheet.Sheets
                .Where(sheet => sheet.Properties?.Title != null)
                .Select(sheet => sheet.Properties!.Title!)
                .ToList();
        }

        public async Task<List<Dictionary<string, string>>> FetchSheetAsync(string spreadsheetId,string sheetName)
        {
            string range = $"{sheetName}!A:ZZ";

            var request =
                service.Spreadsheets.Values.Get(
                    spreadsheetId,
                    range);

            ValueRange response =
                await request.ExecuteAsync();

            if (response.Values == null ||
                response.Values.Count == 0)
            {
                return new List<Dictionary<string, string>>();
            }

            // First row contains column names.
            List<string> headers =
                response.Values[0]
                    .Select(value => value?.ToString() ?? "")
                    .ToList();

            var result =
                new List<Dictionary<string, string>>();

            // Start at row 1 because row 0 is the header.
            for (int rowIndex = 1;
                 rowIndex < response.Values.Count;
                 rowIndex++)
            {
                var row =
                    response.Values[rowIndex];

                var data =
                    new Dictionary<string, string>();

                for (int columnIndex = 0;
                     columnIndex < headers.Count;
                     columnIndex++)
                {
                    string header =
                        headers[columnIndex];

                    if (string.IsNullOrWhiteSpace(header))
                        continue;

                    string value =
                        columnIndex < row.Count
                            ? row[columnIndex]?.ToString() ?? ""
                            : "";

                    data[header] = value;
                }

                result.Add(data);
            }

            return result;
        }
    }
}
