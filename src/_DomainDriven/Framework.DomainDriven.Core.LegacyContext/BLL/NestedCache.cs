using CommonFramework.DictionaryCache;

using Framework.Persistent;

namespace Framework.DomainDriven.BLL;

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
