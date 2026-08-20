using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using DataFetcher.Services;

namespace DataFetcher.JsonExporter
{
    public class JsonExporter
    {
        public async Task ExportAsync<T>(
        T data,
        string outputPath)
        {
            string? directory =
                Path.GetDirectoryName(outputPath);

            if (!string.IsNullOrEmpty(directory))
            {
                Directory.CreateDirectory(directory);
            }

            string json =
                JsonSerializer.Serialize(
                    data,
                    new JsonSerializerOptions
                    {
                        WriteIndented = true
                    });

            await File.WriteAllTextAsync(
                outputPath,
                json);

            Console.WriteLine(
                $"Generated: {outputPath}");
        }
    }
}
