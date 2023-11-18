using ArcanumLogic.EntityFramework.Map;
using DataAccess.Abstractions.Interfaces;
using DataAccess.SqlLite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcanumLogic.EntityFramework;

public class MyContext : SqlLiteDataContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=Arcanum.db;", b => b.MigrationsAssembly(typeof(MyContext).Assembly.GetName().Name));
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AccountMap());
        modelBuilder.ApplyConfiguration(new TransferMap());
        modelBuilder.ApplyConfiguration(new BidMap());
        modelBuilder.ApplyConfiguration(new ImagineMap());
        modelBuilder.ApplyConfiguration(new ResearchMap());
        modelBuilder.ApplyConfiguration(new TransferMap());
        modelBuilder.ApplyConfiguration(new TreeMap());
        modelBuilder.ApplyConfiguration(new FabricaMap());
    }
}
