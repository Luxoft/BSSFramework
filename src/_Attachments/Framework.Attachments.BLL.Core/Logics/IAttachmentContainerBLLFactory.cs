﻿using Framework.Attachments.Domain;

using Framework.SecuritySystem;

namespace Framework.Attachments.BLL
{
    public partial interface IAttachmentContainerBLLFactory
    {
        IAttachmentContainerBLL Create(DomainType domainType, BLLSecurityMode securityMode);
    }
}
