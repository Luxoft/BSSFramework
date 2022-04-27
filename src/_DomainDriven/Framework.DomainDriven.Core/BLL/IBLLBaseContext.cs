using Framework.DomainDriven.BLL.Tracking;
using Framework.Persistent;

namespace Framework.DomainDriven.BLL
{
    public interface IBLLBaseContextBase<in TPersistentDomainObjectBase, TIdent>

        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
    {
        IDALFactory<TPersistentDomainObjectBase, TIdent> DalFactory { get; }
    }


    public interface IBLLBaseContext<TPersistentDomainObjectBase, TDomainObjectBase, TIdent> : IBLLBaseContextBase<TPersistentDomainObjectBase, TIdent>, IBLLOperationEventContext<TDomainObjectBase>, IBLLSourceEventContext<TPersistentDomainObjectBase>, IODataBLLContext

        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>, TDomainObjectBase
        where TDomainObjectBase : class
    {
    }
}
