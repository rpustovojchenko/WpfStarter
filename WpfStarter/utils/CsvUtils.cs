using System.Collections.ObjectModel;
using System.IO;
using WpfStarter.Data;
using WpfStarter.Models;


namespace WpfStarter.utils;

public static class CsvUtils
{
    private static readonly string _filePath =
        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "records.csv");



    public static void ImportCsvToDb()
    {
        var recordsInDb = DbManager.GetRecords();
        if (recordsInDb.Count > 0) return;

        var csvRecords = LoadDataFromCsv();
        foreach (var record in csvRecords)
            DbManager.AddRecord(record);
    }

    private static ObservableCollection<Record> LoadDataFromCsv()
    {
        if (!File.Exists(_filePath))
            return new();

        var lines = File.ReadAllLines(_filePath);
        var records = new ObservableCollection<Record>();

        foreach (var line in lines)
        {
            var parts = line.Split(';');
            records.Add(new()
            {
                Date = DateOnly.Parse(parts[0]),
                FirstName = parts[1],
                SecondName = parts[2],
                SurName = parts[3],
                City = parts[4],
                Country = parts[5]
            });
        }
        return records;
    }
}
