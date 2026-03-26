using Framework.Application.Domain;

namespace Framework.BLL;

public interface IDefaultBLLFactory<in TPersistentDomainObjectBase, TIdent>
    where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
{
    IDefaultDomainBLLBase<TPersistentDomainObjectBase, TDomainObject, TIdent> Create<TDomainObject>()
        where TDomainObject : class, TPersistentDomainObjectBase;
}
