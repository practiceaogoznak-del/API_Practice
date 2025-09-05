using Microsoft.EntityFrameworkCore;
using WebApplication1.models;

namespace WebApplication1.Data
{
    public class ZadelkaContext : DbContext
    {
        public ZadelkaContext(DbContextOptions<ZadelkaContext> options) : base(options)
        {
        }

        public DbSet<ZadelkaRecords> ZadelkaRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ZadelkaRecords>(entity =>
            {
                entity.ToTable("zadelka", "public"); // Схема и таблица


                entity.HasKey(e => new { e.series1, e.series2, e.number });

                entity.Property(e => e.series1).HasColumnName("series1");
                entity.Property(e => e.series2).HasColumnName("series2");
                entity.Property(e => e.number).HasColumnName("number");
                entity.Property(e => e.checkedtabnom).HasColumnName("checked_tabnom");
                entity.Property(e => e.checkeddatetime).HasColumnName("checked_datetime");
            });
        }
    }
}