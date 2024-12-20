using ByteBox.FileStore.Domain.Constants;
using ByteBox.FileStore.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ByteBox.FileStore.Infrastructure.Data.Interceptors;

public sealed class AuditableEntityInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        if (eventData.Context is not null)
        {
            UpdateAuditableEntries(eventData.Context);
        }

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        if (eventData.Context is not null)
        {
            UpdateAuditableEntries(eventData.Context);
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static void UpdateAuditableEntries(DbContext context)
    {
        var currentDateTime = DateTime.UtcNow;
        var currentUserId = Default.User.UserId;

        var entries = context.ChangeTracker.Entries<IAuditable>().ToList();

        foreach (var entry in entries )
        {
            if (entry.State == EntityState.Added)
            {
                HandleAddedAuditableEntries(entry, currentDateTime, currentUserId);
            }

            if (entry.State == EntityState.Modified)
            {
                HandleUpdatedAuditableEntries(entry, currentDateTime, currentUserId);
            }
        }
    }

    private static void HandleAddedAuditableEntries(EntityEntry entry, DateTime currentDateTime, Guid currentUserId)
    {
        entry.Property(nameof(IAuditable.CreatedAtUtc)).CurrentValue = currentDateTime;
        entry.Property(nameof(IAuditable.CreatedByUserId)).CurrentValue = currentUserId;
    }

    private static void HandleUpdatedAuditableEntries(EntityEntry entry, DateTime currentDateTime, Guid currentUserId)
    {
        entry.Property(nameof(IAuditable.UpdatedAtUtc)).CurrentValue = currentDateTime;
        entry.Property(nameof(IAuditable.UpdatedByUserId)).CurrentValue = currentUserId;
    }
}
