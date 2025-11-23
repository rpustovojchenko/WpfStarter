using System;
using System.Collections.Generic;
using System.Text;
using WpfStarter.Models;

namespace WpfStarter.Data;

public static class DbManager
{
    public static void AddRecord(Record record)
    {
        using var db = new AppDbContext();
        db.Records.Add(record);
        db.SaveChanges();
    }

    public static List<Record> GetRecords()
    {
        using var db = new AppDbContext();
        return db.Records.ToList();
    }

    public static List<Record> QueryRecordsBySample(Record sample)
    {
        using var db = new AppDbContext();
        var query = db.Records.AsQueryable();

        if (sample.Date != null)
            query = query.Where(r => r.Date == sample.Date);

        if (!string.IsNullOrWhiteSpace(sample.FirstName))
            query = query.Where(r => r.FirstName == sample.FirstName);

        if (!string.IsNullOrWhiteSpace(sample.SecondName))
            query = query.Where(r => r.SecondName == sample.SecondName);

        if (!string.IsNullOrWhiteSpace(sample.SurName))
            query = query.Where(r => r.SurName == sample.SurName);

        if (!string.IsNullOrWhiteSpace(sample.City))
            query = query.Where(r => r.City == sample.City);

        if (!string.IsNullOrWhiteSpace(sample.Country))
            query = query.Where(r => r.Country == sample.Country);

        return query.ToList();
    }
}
