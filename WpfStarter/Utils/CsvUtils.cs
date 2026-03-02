using System.Globalization;
using System.IO;
using WpfStarter.Data;
using WpfStarter.Models;

namespace WpfStarter.Utils;

public static class CsvUtils
{
    public static async Task ImportCsvToDbAsync(
        string filePath,
        CancellationToken ct = default)
    {
        const int batchSize = 5000;
        var batch = new List<Record>(batchSize);

        int totalLines = File.ReadLines(filePath).Count();

        await Task.Run(async () =>
        {
            using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, useAsync: true);
            using var reader = new StreamReader(fs);

            string? line;

            while ((line = await reader.ReadLineAsync()) != null)
            {
                ct.ThrowIfCancellationRequested();

                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var parts = line.Split(';');
                if (parts.Length != 6)
                    continue;

                var record = new Record
                {
                    Date = DateTime.TryParseExact(
                        parts[0].Trim(),
                        "dd.MM.yyyy",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out var date)
                        ? date
                        : null,
                    FirstName = parts[1].Trim(),
                    LastName = parts[2].Trim(),
                    SurName = parts[3].Trim(),
                    City = parts[4].Trim(),
                    Country = parts[5].Trim()
                };

                batch.Add(record);

                if (batch.Count >= batchSize)
                {
                    await SaveBatchAsync(batch, ct);
                    batch.Clear();
                }
            }

            if (batch.Count > 0)
            {
                await SaveBatchAsync(batch, ct);
                batch.Clear();
            }
        }, ct);
    }

    private static async Task SaveBatchAsync(List<Record> batch, CancellationToken ct)
    {
        await using var db = new AppDbContext();

        db.ChangeTracker.AutoDetectChangesEnabled = false;
        db.ChangeTracker.QueryTrackingBehavior = Microsoft.EntityFrameworkCore.QueryTrackingBehavior.NoTracking;

        await db.Records.AddRangeAsync(batch, ct);
        await db.SaveChangesAsync(ct);
    }
}
