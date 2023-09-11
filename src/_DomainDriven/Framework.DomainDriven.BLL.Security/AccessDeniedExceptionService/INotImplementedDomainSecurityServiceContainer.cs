using Framework.DomainDriven.Repository.NotImplementedDomainSecurityService;

namespace Framework.DomainDriven.BLL.Security;

public interface INotImplementedDomainSecurityServiceSource
{
    INotImplementedDomainSecurityService<TDomainObject> GetNotImplementedDomainSecurityService<TDomainObject>();
}
