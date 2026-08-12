using NHibernate.Event;

namespace Framework.Database.NHibernate;

internal static class EntityActionHeaderExtensions
{
    internal static IDALObject ToDALObjects<T>(this T source, long applyIndex)
            where T : AbstractPostDatabaseOperationEvent =>
        new DALObject(source.Entity, source.Persister.EntityMetamodel.Type, applyIndex);
}
