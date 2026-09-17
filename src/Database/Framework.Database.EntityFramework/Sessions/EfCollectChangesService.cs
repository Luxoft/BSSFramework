using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Framework.Database.EntityFramework.Sessions;

internal class EfCollectChangesService
{
    private long counter;

    public DALChanges CollectChanges(DbContext context)
    {
        context.ChangeTracker.DetectChanges();

        var createdItems = new List<IDALObject>();
        var updatedItems = new List<IDALObject>();
        var removedItems = new List<IDALObject>();

        foreach (var entry in context.ChangeTracker.Entries())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    createdItems.Add(this.ToDALObject(entry));
                    break;

                case EntityState.Modified:
                    updatedItems.Add(this.ToDALObject(entry));
                    break;

                case EntityState.Deleted:
                    removedItems.Add(this.ToDALObject(entry));
                    break;
            }
        }

        return new DALChanges(createdItems, updatedItems, removedItems);
    }

    private IDALObject ToDALObject(EntityEntry entry) => new DALObject(entry.Entity, entry.Metadata.ClrType, this.counter++);
}
