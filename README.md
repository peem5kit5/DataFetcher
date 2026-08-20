# Google Sheets JSON DataFetcher

A simple C# tool that reads data from Google Sheets and exports it as JSON files.

> [!IMPORTANT]
> ## Before Using This Tool
>
> You **must complete the following setup before running the DataFetcher**:
>
> 1. Create a Google Cloud Service Account
> 2. Enable the Google Sheets API
> 3. Generate `credentials.json`
> 4. Share your Google Sheet with the Service Account
> 5. Configure `appsettings.json`
>
> The DataFetcher will **not work without `credentials.json` that you can generated from google cloud api key and a correctly configured `appsettings.json` with desired path, sheet id and tab**.

---

# How It Works

```text
Google Sheets
      │
      ▼
DataFetcher.exe
      │
      ▼
JSON Files
