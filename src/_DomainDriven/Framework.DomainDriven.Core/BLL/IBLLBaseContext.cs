using Framework.Persistent;

namespace Framework.DomainDriven.BLL;

public interface IBLLBaseContextBase<in TPersistentDomainObjectBase, TIdent> : IServiceProviderContainer

        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
{
}


public interface IBLLBaseContext<in TPersistentDomainObjectBase, TDomainObjectBase, TIdent> : IBLLBaseContextBase<TPersistentDomainObjectBase, TIdent>, IBLLOperationEventContext<TPersistentDomainObjectBase>, IODataBLLContext

        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>, TDomainObjectBase
        where TDomainObjectBase : class
{
}
