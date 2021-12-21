using Framework.Persistent;

namespace Framework.DomainDriven.BLL
{
    public interface IDALFactory
    {
    }

    public interface IDALFactory<in TPersistentDomainObjectBase, TIdent> : IDALFactory
        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
    {
        IDAL<TDomainObject, TIdent> CreateDAL<TDomainObject>()
            where TDomainObject : class, TPersistentDomainObjectBase;
    }
}
