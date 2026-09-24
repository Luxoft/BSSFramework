using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Framework.Database.EntityFramework.SqlExceptionProcessors;

internal class ExceptionProcessingContext
{
    private readonly ILookup<string, IEntityType> entityTypesByTableName;

    public ExceptionProcessingContext(IModel model)
    {
        this.entityTypesByTableName = model.GetEntityTypes()
                                            .Where(entityType => entityType.GetTableName() is not null)
                                            .ToLookup(entityType => entityType.GetTableName()!, StringComparer.InvariantCultureIgnoreCase);
    }

    public IEntityType? GetEntityType(string rawTableName)
    {
        var tableName = rawTableName.Trim('[', ']').Split('.').Last().Trim('[', ']');

        return this.entityTypesByTableName[tableName].FirstOrDefault();
    }

    public static object GetEntityId(EntityEntry? entry)
    {
        if (entry is null)
        {
            return null!;
        }

        var primaryKey = entry.Metadata.FindPrimaryKey();

        if (primaryKey is not null && primaryKey.Properties.Count == 1)
        {
            return entry.Property(primaryKey.Properties[0].Name).CurrentValue!;
        }

        return entry.Entity;
    }
}
