using NHibernate.Event;

namespace Framework.DomainDriven.NHibernate;

internal static class EntityActionHeaderExtensions
{
    internal static IDALObject ToDALObjects<T>(this T source, long applyIndex)
            where T : AbstractPostDatabaseOperationEvent
    {
        return new DALObject(source.Entity, source.Persister.EntityMetamodel.Type, applyIndex);
    }
}
