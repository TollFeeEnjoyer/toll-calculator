using Microsoft.EntityFrameworkCore;
using TollCalculator.Domain.TaxRule.Impl;

namespace TollCalculator.Data.DatabaseContext;

public class AppDbContext : DbContext
{
    public DbSet<TollFeeTaxRule> TaxRules { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<TollFeeTaxRule>()
            .HasOne(t => t.MultiPassageRule)
            .WithOne()
            .HasForeignKey<TollFeeTaxRule>(t => t.MultiPassageRuleId);

        modelBuilder.Entity<TollFeeTaxRule>()
            .HasMany(e => e.TaxExemptVehicles)
            .WithMany(e => e.TollFeeTaxRule);

        modelBuilder.Entity<TollFeeTaxRule>()
            .HasMany(e => e.TaxRates)
            .WithMany(e => e.TollFeeTaxRule);

        modelBuilder.Entity<TollFeeTaxRule>()
            .HasMany(e => e.TollFreeDates)
            .WithMany(e => e.TollFeeTaxRule);
    }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
}