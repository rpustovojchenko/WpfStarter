using Microsoft.EntityFrameworkCore;
using WpfStarter.Models;

namespace WpfStarter.Data;

public static class DbManager
{
    public static async Task<int> GetRecordsCountAsync()
    {
        using var db = new AppDbContext();
        return await db.Records.CountAsync();
    }

    public static async Task<List<Record>> GetRecordsPageAsync(int page, int pageSize)
    {
        using var db = new AppDbContext();
        return await db.Records
            .AsNoTracking()
            .OrderBy(r => r.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public static async Task<bool> IsDbEmptyAsync()
    {
        using var db = new AppDbContext();
        return !await db.Records.AsNoTracking().AnyAsync();
    }

    public static void ClearAllRecords()
    {
        using var db = new AppDbContext();
        db.Database.ExecuteSqlRaw("DELETE FROM Records");
    }

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