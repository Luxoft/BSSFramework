using NHibernate.Envers;

namespace Framework.Database.NHibernate.Envers;

public class IdentityRevisionEntityInfo<TRevisionEntity, TIdentity>(TRevisionEntity revisionEntity, TIdentity identity, RevisionType operation)
    : IIdentityRevisionEntityInfo<TRevisionEntity, TIdentity>
{
    public TIdentity Identity { get; private set; } = identity;

    public TRevisionEntity RevisionEntity { get; private set; } = revisionEntity;

    public RevisionType Operation { get; private set; } = operation;
}
