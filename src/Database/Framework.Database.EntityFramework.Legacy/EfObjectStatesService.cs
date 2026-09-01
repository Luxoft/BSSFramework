using Framework.Tracking;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Framework.Database.EntityFramework;

public class EfObjectStatesService(DbContext context) : IObjectStateService
{
    public IEnumerable<ObjectState> GetModifiedObjectStates(object? entity)
    {
        if (entity is null)
        {
            return [];
        }

        var entry = this.TryGetEntry(entity);

        if (entry is null)
        {
            return [];
        }

        var propertyStates = entry.Properties
                                  .Where(property => property.IsModified)
                                  .Select(property => new ObjectState(property.Metadata.Name, property.CurrentValue, property.OriginalValue, true));

        var navigationStates = entry.Navigations
                                    .OfType<CollectionEntry>()
                                    .Where(this.IsModifiedCollection)
                                    .Select(collection => new ObjectState(collection.Metadata.Name, collection.CurrentValue, null, true));

        return propertyStates.Concat(navigationStates);
    }

    public bool IsNew(object entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        var entry = this.TryGetEntry(entity);

        return entry is null || entry.State == EntityState.Added;
    }

    public bool IsRemoving(object entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return this.TryGetEntry(entity)?.State == EntityState.Deleted;
    }

    private bool IsModifiedCollection(CollectionEntry collection)
    {
        if (!collection.IsLoaded || collection.CurrentValue is null)
        {
            return false;
        }

        return collection.CurrentValue
                         .Cast<object>()
                         .Any(item => this.TryGetEntry(item) is { State: not EntityState.Unchanged } || this.GetModifiedObjectStates(item).Any());
    }

    private EntityEntry? TryGetEntry(object entity) => context.ChangeTracker.Entries().FirstOrDefault(entry => ReferenceEquals(entry.Entity, entity));
}
