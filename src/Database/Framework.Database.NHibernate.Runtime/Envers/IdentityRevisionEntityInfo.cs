using NHibernate.Envers;

namespace Framework.Database.NHibernate.Envers;

public class IdentityRevisionEntityInfo<TRevisionEntity, TIdentity>(TRevisionEntity revisionEntity, TIdentity identity, RevisionType operation)
    : IIdentityRevisionEntityInfo<TRevisionEntity, TIdentity>
{
    public TIdentity Identity { get; } = identity;

    public TRevisionEntity RevisionEntity { get; } = revisionEntity;

    public RevisionType Operation { get; } = operation;
}
