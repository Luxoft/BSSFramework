using NHibernate.Envers;

namespace Framework.Database.NHibernate.Envers;

public interface IIdentityRevisionEntityInfo<out TRevisionEntity, out TIdentity>
{
    TIdentity Identity { get; }

    TRevisionEntity RevisionEntity { get; }

    RevisionType Operation { get; }
}
