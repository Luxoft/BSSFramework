using CommonFramework.DictionaryCache;
using Framework.Application.Domain;

namespace Framework.BLL;

public class NestedCache<TBLLContext, TPersistentDomainObjectBase, TIdent, TDomainObject>(TBLLContext context)
    : DictionaryCache<TDomainObject, TDomainObject>(obj => context.Logics.Default.Create<TDomainObject>().GetNested(obj))
    where TBLLContext : IDefaultBLLContext<TPersistentDomainObjectBase, TIdent>
    where TDomainObject : class, TPersistentDomainObjectBase
    where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>;
