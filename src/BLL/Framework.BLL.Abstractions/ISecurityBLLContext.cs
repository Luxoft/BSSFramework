using Framework.Application.Domain;

namespace Framework.BLL;

public interface ISecurityBLLContext<in TPersistentDomainObjectBase, TIdent> :

    IDefaultBLLContext<TPersistentDomainObjectBase, TIdent>

    where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>;
