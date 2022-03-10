using Framework.Configuration.Domain;

using Framework.SecuritySystem;

namespace Framework.Attachments.BLL
{
    public partial interface IAttachmentBLLFactory
    {
        IAttachmentBLL Create(DomainType domainType, BLLSecurityMode securityMode);
    }
}
