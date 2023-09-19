using Framework.Persistent;

namespace Framework.DomainDriven.BLL;

public interface IDefaultHierarchicalBLLContext<in TPersistentDomainObjectBase, TIdent> : IDefaultBLLContext<TPersistentDomainObjectBase, TIdent>

    where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
{
}
