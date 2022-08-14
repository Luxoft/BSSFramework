using Framework.Persistent;

namespace Framework.DomainDriven
{
    public interface IDALFactory<in TPersistentDomainObjectBase, TIdent>
        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
    {
        IDAL<TDomainObject, TIdent> CreateDAL<TDomainObject>()
            where TDomainObject : class, TPersistentDomainObjectBase;
    }
}
