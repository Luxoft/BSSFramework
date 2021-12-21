using Framework.Configuration.Domain;

using Framework.SecuritySystem;

namespace Framework.Configuration.BLL
{
    public partial interface IAttachmentContainerBLLFactory
    {
        IAttachmentContainerBLL Create(DomainType domainType, BLLSecurityMode securityMode);
    }
}
