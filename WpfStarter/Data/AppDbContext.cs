using Microsoft.EntityFrameworkCore;
using System.Configuration;
using WpfStarter.Models;

namespace WpfStarter.Data;

public class AppDbContext : DbContext
{
    public DbSet<Record> Records { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["RecordsConnection"]!.ConnectionString);
}
