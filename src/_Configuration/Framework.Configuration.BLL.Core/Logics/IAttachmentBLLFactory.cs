using Framework.Configuration.Domain;

using Framework.SecuritySystem;

namespace Framework.Configuration.BLL
{
    public partial interface IAttachmentBLLFactory
    {
        IAttachmentBLL Create(DomainType domainType, BLLSecurityMode securityMode);
    }
}
