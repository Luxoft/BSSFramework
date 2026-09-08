using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Framework.Database.EntityFramework.EnversAudit;

public class EfAuditReader(DbContext dbContext) : IEfAuditReader
{
    private readonly IAuditEntityFactory auditEntityFactory = dbContext.GetService<IAuditEntityFactory>();


    public TEntity Find<TEntity>(object id, long revision)
        where TEntity : class
    {
        var metadata = this.GetMetadataOrThrow(typeof(TEntity));

        var revisionIdProp = metadata.AuditEntityType.GetProperty(auditEntityFactory.RevisionIdPropertyName)!;

        var row = this.QueryAuditRowsForKey(metadata, id)
                      .FirstOrDefault(r => (long)revisionIdProp.GetValue(r)! == revision)
                  ?? throw new InvalidOperationException($"No audit data found for \"{typeof(TEntity).Name}\" id \"{id}\" at revision {revision}.");

        return this.ReconstructEntity<TEntity>(metadata, row);
    }

    public IReadOnlyList<TEntity> FindObjects<TEntity>(IEnumerable<object> ids, long revision)
        where TEntity : class =>
        ids.Select(id => this.Find<TEntity>(id, revision)).ToList();

    public IReadOnlyList<long> GetRevisions(Type entityType, object id)
    {
        var metadata = this.GetMetadataOrThrow(entityType);
        var revisionIdProp = metadata.AuditEntityType.GetProperty(auditEntityFactory.RevisionIdPropertyName)!;

        return this.QueryAuditRowsForKey(metadata, id)
                   .Select(row => (long)revisionIdProp.GetValue(row)!)
                   .OrderBy(revisionId => revisionId)
                   .ToList();
    }

    public IReadOnlyList<long> GetRevisions(Type entityType, object id, long maxRevision) =>
        this.GetRevisions(entityType, id).Where(revisionId => revisionId < maxRevision).ToList();

    public long? GetPreviousRevision(Type entityType, object id, long maxRevision) =>
        this.GetRevisions(entityType, id, maxRevision).OrderByDescending(revisionId => revisionId).Cast<long?>().FirstOrDefault();

    public long GetCurrentRevision() => dbContext.Set<AuditRevisionEntity>().OrderByDescending(revision => revision.Id).Select(revision => revision.Id).FirstOrDefault();

    public long GetMaxRevision() => this.GetCurrentRevision();

    public IReadOnlyList<Tuple<TEntity, long>> GetDomainObjectRevisions<TEntity>(object id, int takeCount)
        where TEntity : class
    {
        var metadata = this.GetMetadataOrThrow(typeof(TEntity));
        var revisionIdProp = metadata.AuditEntityType.GetProperty(auditEntityFactory.RevisionIdPropertyName)!;

        var rows = this.QueryAuditRowsForKey(metadata, id)
                       .OrderByDescending(row => (long)revisionIdProp.GetValue(row)!)
                       .Take(takeCount)
                       .ToList();

        return rows.Select(row => Tuple.Create(this.ReconstructEntity<TEntity>(metadata, row), (long)revisionIdProp.GetValue(row)!)).ToList();
    }

    public IReadOnlyList<AuditRevisionInfo> GetObjectRevisions(Type entityType, object id, DateTime? fromDate = null, DateTime? toDate = null)
    {
        var metadata = this.GetMetadataOrThrow(entityType);

        var revisionIdProp = metadata.AuditEntityType.GetProperty(auditEntityFactory.RevisionIdPropertyName)!;
        var revisionTypeProp = metadata.AuditEntityType.GetProperty(auditEntityFactory.RevisionTypePropertyName)!;

        var rows = this.QueryAuditRowsForKey(metadata, id)
                       .Select(row => (
                           RevisionId: (long)revisionIdProp.GetValue(row)!,
                           RevisionType: (AuditRevisionType)revisionTypeProp.GetValue(row)!))
                       .ToList();

        var revisionEntities = this.GetRevisionEntities(rows.Select(row => row.RevisionId));

        return rows.Select(row => (row.RevisionType, Revision: revisionEntities[row.RevisionId]))
                   .Where(row => this.IsInPeriod(row.Revision.RevisionDate, fromDate, toDate))
                   .Select(row => new AuditRevisionInfo(row.Revision.Id, row.Revision.Author, row.Revision.RevisionDate, row.RevisionType))
                   .OrderBy(revisionInfo => revisionInfo.RevisionNumber)
                   .ToList();
    }

    public IReadOnlyList<AuditPropertyRevisionInfo<TProperty>> GetPropertyRevisions<TProperty>(Type entityType, object id, string propertyName, DateTime? fromDate = null, DateTime? toDate = null)
    {
        var metadata = this.GetMetadataOrThrow(entityType);

        var property = metadata.Properties.FirstOrDefault(p => !p.IsModOnly && string.Equals(p.ModName, propertyName, StringComparison.OrdinalIgnoreCase))
                        ?? throw new InvalidOperationException($"Property \"{propertyName}\" not found in audit metadata for \"{entityType.Name}\".");

        var revisionIdProp = metadata.AuditEntityType.GetProperty(auditEntityFactory.RevisionIdPropertyName)!;
        var revisionTypeProp = metadata.AuditEntityType.GetProperty(auditEntityFactory.RevisionTypePropertyName)!;
        var valueProp = metadata.AuditEntityType.GetProperty(property.Name)!;
        var modFlagProp = property.IsKey ? null : metadata.AuditEntityType.GetProperty($"{property.ModName}_MOD");

        var changedRows = this.QueryAuditRowsForKey(metadata, id)
                              .Select(row => (
                                  RevisionId: (long)revisionIdProp.GetValue(row)!,
                                  RevisionType: (AuditRevisionType)revisionTypeProp.GetValue(row)!,
                                  RawValue: valueProp.GetValue(row),
                                  IsChanged: modFlagProp is null || (bool)modFlagProp.GetValue(row)!))
                              .Where(row => row.RevisionType == AuditRevisionType.Added || row.IsChanged)
                              .ToList();

        var revisionEntities = this.GetRevisionEntities(changedRows.Select(row => row.RevisionId));

        return changedRows.Select(row => (row.RevisionType, row.RawValue, Revision: revisionEntities[row.RevisionId]))
                          .Where(row => this.IsInPeriod(row.Revision.RevisionDate, fromDate, toDate))
                          .Select(row => new AuditPropertyRevisionInfo<TProperty>(
                                      this.ResolvePropertyValue<TProperty>(row.RawValue)!,
                                      new AuditRevisionInfo(row.Revision.Id, row.Revision.Author, row.Revision.RevisionDate, row.RevisionType)))
                          .OrderBy(propertyRevision => propertyRevision.Revision.RevisionNumber)
                          .ToList();
    }

    /// <summary>
    /// Envers-style historical predicate matching against the flattened audit shadow entities is not implemented;
    /// this evaluates the predicate against the current (live) entity set instead.
    /// </summary>
    public IReadOnlyList<TIdent> GetIdentiesWithHistory<TEntity, TIdent>(Expression<Func<TEntity, bool>> predicate)
        where TEntity : class
    {
        var idProperty = typeof(TEntity).GetProperty("Id")
                          ?? throw new InvalidOperationException($"Entity \"{typeof(TEntity).Name}\" has no \"Id\" property.");

        var parameter = predicate.Parameters[0];
        var idSelector = Expression.Lambda<Func<TEntity, TIdent>>(Expression.Property(parameter, idProperty), parameter);

        return dbContext.Set<TEntity>().Where(predicate).Select(idSelector).ToList();
    }

    private Dictionary<long, AuditRevisionEntity> GetRevisionEntities(IEnumerable<long> revisionIds)
    {
        var revisionIdSet = revisionIds.ToHashSet();

        return dbContext.Set<AuditRevisionEntity>().Where(revision => revisionIdSet.Contains(revision.Id)).ToDictionary(revision => revision.Id);
    }

    private bool IsInPeriod(DateTime date, DateTime? fromDate, DateTime? toDate) =>
        (fromDate is null || date >= fromDate) && (toDate is null || date <= toDate);

    private TEntity ReconstructEntity<TEntity>(AuditEntityMetadata metadata, object auditRow)
        where TEntity : class
    {
        var instance = (TEntity)Activator.CreateInstance(typeof(TEntity), nonPublic: true)!;

        foreach (var property in metadata.Properties.Where(p => !p.IsModOnly))
        {
            var rawValue = metadata.AuditEntityType.GetProperty(property.Name)!.GetValue(auditRow);

            if (property.NestedPropertyName is null)
            {
                this.SetScalarOrReferenceValue(instance, property.ModName, rawValue);
            }
            else
            {
                this.SetNestedValue(instance, property, rawValue);
            }
        }

        return instance;
    }

    private void SetScalarOrReferenceValue(object instance, string domainPropertyName, object? rawValue)
    {
        var domainProperty = instance.GetType().GetProperty(domainPropertyName);

        if (domainProperty is null || !domainProperty.CanWrite)
        {
            return;
        }

        if (rawValue is null)
        {
            domainProperty.SetValue(instance, null);
            return;
        }

        if (domainProperty.PropertyType.IsInstanceOfType(rawValue))
        {
            domainProperty.SetValue(instance, rawValue);
            return;
        }

        if (!IsSimpleType(domainProperty.PropertyType))
        {
            domainProperty.SetValue(instance, dbContext.Find(domainProperty.PropertyType, rawValue));
            return;
        }

        domainProperty.SetValue(instance, ConvertScalarValue(domainProperty.PropertyType, rawValue));
    }

    private void SetNestedValue(object instance, AuditPropertyMetadata property, object? rawValue)
    {
        var containerProperty = instance.GetType().GetProperty(property.ModName);

        if (containerProperty is null)
        {
            return;
        }

        var containerValue = containerProperty.GetValue(instance) ?? Activator.CreateInstance(containerProperty.PropertyType, nonPublic: true)!;

        var nestedProperty = containerProperty.PropertyType.GetProperty(property.NestedPropertyName!)!;
        nestedProperty.SetValue(containerValue, ConvertScalarValue(nestedProperty.PropertyType, rawValue));

        containerProperty.SetValue(instance, containerValue);
    }

    private TProperty? ResolvePropertyValue<TProperty>(object? rawValue)
    {
        if (rawValue is null)
        {
            return default;
        }

        if (rawValue is TProperty typedValue)
        {
            return typedValue;
        }

        if (!IsSimpleType(typeof(TProperty)))
        {
            return (TProperty?)dbContext.Find(typeof(TProperty), rawValue);
        }

        return (TProperty)ConvertScalarValue(typeof(TProperty), rawValue)!;
    }

    private static object? ConvertScalarValue(Type targetType, object? rawValue)
    {
        if (rawValue is null)
        {
            return null;
        }

        if (targetType.IsInstanceOfType(rawValue))
        {
            return rawValue;
        }

        var underlyingType = Nullable.GetUnderlyingType(targetType) ?? targetType;

        return underlyingType.IsEnum ? Enum.ToObject(underlyingType, rawValue) : Convert.ChangeType(rawValue, underlyingType);
    }

    private static bool IsSimpleType(Type type)
    {
        var underlyingType = Nullable.GetUnderlyingType(type) ?? type;

        return underlyingType.IsPrimitive
               || underlyingType.IsEnum
               || underlyingType == typeof(string)
               || underlyingType == typeof(decimal)
               || underlyingType == typeof(DateTime)
               || underlyingType == typeof(DateTimeOffset)
               || underlyingType == typeof(Guid)
               || underlyingType == typeof(TimeSpan);
    }

    private AuditEntityMetadata GetMetadataOrThrow(Type entityType) =>
        auditEntityFactory.TryGet(entityType, out var metadata)
            ? metadata
            : throw new InvalidOperationException($"Entity \"{entityType.Name}\" is not audited.");

    private List<object> QueryAuditRowsForKey(AuditEntityMetadata metadata, object id)
    {
        var keyProperty = metadata.Properties.First(p => p.IsKey);

        var setMethod = typeof(DbContext)
                        .GetMethods()
                        .First(m => m.Name == nameof(DbContext.Set) && m.IsGenericMethodDefinition && m.GetParameters().Length == 0)
                        .MakeGenericMethod(metadata.AuditEntityType);

        var queryable = (IQueryable)setMethod.Invoke(dbContext, null)!;

        var parameter = Expression.Parameter(metadata.AuditEntityType, "x");
        var keyAccess = Expression.Property(parameter, keyProperty.Name);
        var lambda = Expression.Lambda(Expression.Equal(keyAccess, Expression.Constant(id, keyAccess.Type)), parameter);

        var whereMethod = typeof(Queryable)
                          .GetMethods()
                          .First(m => m.Name == nameof(Queryable.Where)
                                      && m.GetParameters().Length == 2
                                      && m.GetParameters()[1].ParameterType.GetGenericArguments()[0].GetGenericArguments().Length == 2)
                          .MakeGenericMethod(metadata.AuditEntityType);

        var filtered = (IQueryable)whereMethod.Invoke(null, [queryable, lambda])!;

        return filtered.Cast<object>().ToList();
    }
}
