using Framework.Core;
using Framework.Persistent;

namespace Framework.DomainDriven.BLL;

public interface IBLLBaseContext<in TPersistentDomainObjectBase, TIdent> : IBLLOperationEventContext<TPersistentDomainObjectBase>,
                                                                           IODataBLLContext,
                                                                           IServiceProviderContainer

    where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
{
}
