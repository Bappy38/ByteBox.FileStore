using ByteBox.FileStore.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ByteBox.FileStore.Infrastructure.Data.Interceptors;

public sealed class SoftDeletableEntityInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        if (eventData.Context is not null)
        {
            HandleSoftDeletableEntries(eventData.Context);
        }
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        if (eventData.Context is not null)
        {
            HandleSoftDeletableEntries(eventData.Context);
        }
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static void HandleSoftDeletableEntries(DbContext context)
    {
        var entries = context.ChangeTracker.Entries<ISoftDeletable>().ToList();

        foreach (var entry in entries)
        {
            if (entry.State != EntityState.Deleted)
            {
                continue;
            }
            
            entry.Property(nameof(ISoftDeletable.IsDeleted)).CurrentValue = true;
            entry.State = EntityState.Modified;
        }
    }
}
