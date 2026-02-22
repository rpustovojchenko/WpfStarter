using Microsoft.EntityFrameworkCore;
using WpfStarter.Models;
using WpfStarter.Utils;

namespace WpfStarter.Data;

public static class DbManager
{
    public static List<Record> GetRecords()
    {
        using var db = new AppDbContext();
        return db.Records.ToList();
    }

    public static void ClearAllRecords()
    {
        using var db = new AppDbContext();
        db.Database.ExecuteSqlRaw("DELETE FROM Records");
    }

    public static bool IsDbEmpty() =>
        GetRecords().Count() == 0;

    public static List<Record> QueryRecordsBySample(Record sample)
    {
        using var db = new AppDbContext();
        var query = db.Records.AsQueryable();

        if (sample.Date != null)
            query = query.Where(r => r.Date.HasValue && r.Date.Value.Date == sample.Date.Value.Date);

        if (!string.IsNullOrWhiteSpace(sample.FirstName))
            query = query.Where(r => r.FirstName.ToLower() == sample.FirstName.ToLower());

        if (!string.IsNullOrWhiteSpace(sample.LastName))
            query = query.Where(r => r.LastName.ToLower() == sample.LastName.ToLower());

        if (!string.IsNullOrWhiteSpace(sample.SurName))
            query = query.Where(r => r.SurName.ToLower() == sample.SurName.ToLower());

        if (!string.IsNullOrWhiteSpace(sample.City))
            query = query.Where(r => r.City.ToLower() == sample.City.ToLower());

        if (!string.IsNullOrWhiteSpace(sample.Country))
            query = query.Where(r => r.Country.ToLower() == sample.Country.ToLower());


        return query.ToList();
    }
}