using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SBSC.Wallet.BusinessCore.AuditLogs;
using SBSC.Wallet.CoreObject.Enumerables;

namespace SBSC.Wallet.BusinessCore.DbModels;

public partial class WalletContext: IdentityDbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            IConfiguration Configuration = builder.Build();
            optionsBuilder.UseSqlServer(Configuration.GetConnectionString("WalletConnection"));
        }
    }
    public virtual int SaveChanges(string? userId = null, bool track = true)
    {
        if (track)
            OnBeforeSaveChanges(userId);
        var result = base.SaveChangesAsync().Result;
        return result;
    }

    public virtual async Task<int> SaveChangesAsync(string? userId = null, bool track = true)
    {
        if (track)
            OnBeforeSaveChanges(userId);
        var result = await base.SaveChangesAsync();
        return result;
    }

    private void OnBeforeSaveChanges(string? userId)
    {
        ChangeTracker.DetectChanges();
        var auditEntries = new List<AuditHelper>();
        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.Entity is Audit || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                continue;
            if (entry.State == EntityState.Added)
                continue;
            var auditEntry = new AuditHelper(entry);
            auditEntry.TableName = entry.Entity.GetType().Name;
            auditEntry.UserId = userId;
            auditEntries.Add(auditEntry);
            foreach (var property in entry.Properties)
            {
                if (property == null)
                    continue;
                string propertyName = property.Metadata.Name;
                if (property.Metadata.IsPrimaryKey())
                {
                    auditEntry.KeyValues[propertyName] = property.CurrentValue;
                    continue;
                }
                switch (entry.State)
                {
                    case EntityState.Added:
                        auditEntry.AuditType = AuditType.Create;
                        auditEntry.NewValues[propertyName] = property.CurrentValue;
                        break;

                    case EntityState.Deleted:
                        auditEntry.AuditType = AuditType.Delete;
                        auditEntry.OldValues[propertyName] = property.OriginalValue;
                        break;

                    case EntityState.Modified:
                        if (property.IsModified)
                        {
                            auditEntry.ChangedColumns.Add(propertyName);
                            auditEntry.AuditType = AuditType.Update;
                            auditEntry.OldValues[propertyName] = property.OriginalValue;
                            auditEntry.NewValues[propertyName] = property.CurrentValue;
                        }
                        break;
                }
            }
        }
        foreach (var auditEntry in auditEntries)
        {
            Audits.Add(auditEntry.ToAudit());
        }
    }
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<Wallet>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<Admin>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<Currency>().HasQueryFilter(x => !x.IsDeleted);
    }

}
