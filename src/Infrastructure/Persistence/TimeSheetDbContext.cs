using Infrastructure.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence
{
    internal sealed class TimeSheetDbContext : DbContext
    {
        internal DbSet<ProjectEntity> Projects { get; set; }
        internal DbSet<TimeWorkedEntity> TimeWorked { get; set; }

        public TimeSheetDbContext(DbContextOptions options) : base(options)
        {
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach(var entry in ChangeTracker.Entries().Where(e => e.Entity is BaseEntity))
            {
                if(entry.State == EntityState.Deleted)
                {
                    entry.State = EntityState.Modified;
                    entry.CurrentValues[nameof(BaseEntity.IsActive)] = false;
                    entry.CurrentValues[nameof(BaseEntity.DateModified)] = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Added)
                {
                    entry.CurrentValues[nameof(BaseEntity.DateCreated)] = DateTime.UtcNow;
                    entry.CurrentValues[nameof(BaseEntity.IsActive)] = true;
                    entry.CurrentValues[nameof(BaseEntity.DateModified)] = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.CurrentValues[nameof(BaseEntity.DateModified)] = DateTime.UtcNow;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProjectEntity>().HasQueryFilter(p => p.IsActive);
            modelBuilder.Entity<TimeWorkedEntity>().HasQueryFilter(p => p.IsActive);

            modelBuilder.Entity<ProjectEntity>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<ProjectEntity>()
                .Property(p => p.Name)
                .HasMaxLength(64);

            modelBuilder.Entity<ProjectEntity>()
                .HasMany(p => p.TimeWorked)
                .WithOne(tw => tw.Project)
                .HasForeignKey(tw => tw.ProjectId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<TimeWorkedEntity>()
                .HasKey(tw => tw.Id);

            modelBuilder.Entity<TimeWorkedEntity>()
                .Property(tw => tw.Notes)
                .HasMaxLength(255);

            modelBuilder.Entity<TimeWorkedEntity>()
                .HasOne(tw => tw.Project)
                .WithMany(p => p.TimeWorked)
                .HasForeignKey(tw => tw.ProjectId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
