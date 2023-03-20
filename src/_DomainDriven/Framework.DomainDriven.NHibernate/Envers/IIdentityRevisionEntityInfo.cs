namespace NHibernate.Envers.Patch;

public interface IIdentityRevisionEntityInfo<out TRevisionEntity, out TIdentity>
{
    TIdentity Identity { get; }

    TRevisionEntity RevisionEntity { get; }

    RevisionType Operation { get; }
}
