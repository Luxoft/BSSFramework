using Framework.DomainDriven.BLL;

using JetBrains.Annotations;

using NHibernate.Event;

namespace Framework.DomainDriven.NHibernate;

internal static class EntityActionHeaderExtensions
{
    internal static IDALObject ToDALObjects<T>([NotNull] this T source, long applyIndex)
            where T : AbstractPostDatabaseOperationEvent
    {
        return new DALObject(source.Entity, source.Persister.EntityMetamodel.Type, applyIndex);
    }
}
