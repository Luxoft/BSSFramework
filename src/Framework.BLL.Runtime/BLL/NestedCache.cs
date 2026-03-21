using CommonFramework.DictionaryCache;

using Framework.Application.Domain;

namespace Framework.BLL.BLL;

public class NestedCache<TBLLContext, TPersistentDomainObjectBase, TIdent, TDomainObject> : DictionaryCache<TDomainObject, TDomainObject>
        where TBLLContext : IDefaultBLLContext<TPersistentDomainObjectBase, TIdent>
        where TDomainObject : class, TPersistentDomainObjectBase
        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
{
    public NestedCache(TBLLContext context)
            : base(obj => context.Logics.Default.Create<TDomainObject>().GetNested(obj))
    {

    }
}
