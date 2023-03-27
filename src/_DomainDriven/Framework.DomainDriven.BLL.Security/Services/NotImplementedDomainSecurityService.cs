using Framework.Core;
using Framework.SecuritySystem;

using JetBrains.Annotations;

namespace Framework.DomainDriven.BLL.Security;

public class NotImplementedDomainSecurityService<TBLLContext, TDomainObject> : IDomainSecurityService<TDomainObject>
        where TBLLContext : class
        where TDomainObject : class
{
    protected readonly IRootSecurityService<TBLLContext, TDomainObject> RootSecurityService;


    public NotImplementedDomainSecurityService(IRootSecurityService<TBLLContext, TDomainObject> rootSecurityService)
    {
        this.RootSecurityService = rootSecurityService ?? throw new ArgumentNullException(nameof(rootSecurityService));
    }


    public ISecurityProvider<TDomainObject> GetSecurityProvider(BLLSecurityMode securityMode)
    {
        if (securityMode == BLLSecurityMode.Disabled)
        {
            return this.RootSecurityService.GetDisabledSecurityProvider<TDomainObject>();
        }
        else
        {
            return this.RootSecurityService.GetNotImplementedSecurityProvider<TDomainObject>(securityMode);
        }
    }
}

public class NotImplementedDomainSecurityService<TBLLContext, TDomainObject, TSecurityOperationCode> : NotImplementedDomainSecurityService<TBLLContext, TDomainObject>, IDomainSecurityService<TDomainObject, TSecurityOperationCode>
        where TBLLContext : class
        where TDomainObject : class
        where TSecurityOperationCode : struct, Enum
{
    public NotImplementedDomainSecurityService(IRootSecurityService<TBLLContext, TDomainObject, TSecurityOperationCode> rootSecurityService)
            : base(rootSecurityService)
    {
    }


    public ISecurityProvider<TDomainObject> GetSecurityProvider([NotNull] SecurityOperation<TSecurityOperationCode> securityOperation)
    {
        if (securityOperation == null) throw new ArgumentNullException(nameof(securityOperation));

        return this.RootSecurityService.GetNotImplementedSecurityProvider<TDomainObject>(securityOperation);
    }

    public ISecurityProvider<TDomainObject> GetSecurityProvider(TSecurityOperationCode securityOperationCode)
    {
        if (securityOperationCode.IsDefault())
        {
            return this.RootSecurityService.GetDisabledSecurityProvider<TDomainObject>();
        }
        else
        {
            return this.RootSecurityService.GetNotImplementedSecurityProvider<TDomainObject>(securityOperationCode);
        }
    }
}
