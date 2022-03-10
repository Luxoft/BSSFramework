using Framework.Configuration.Domain;

using Framework.SecuritySystem;

namespace Framework.Attachments.BLL
{
    public partial interface IAttachmentContainerBLLFactory
    {
        IAttachmentContainerBLL Create(DomainType domainType, BLLSecurityMode securityMode);
    }
}
