using System;

using Framework.Authorization.BLL;
using Framework.Core;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;
using Framework.DomainDriven.BLL.Tracking;
using Framework.Security.Cryptography;
using Framework.Attachments.BLL;

using AttachmentsSampleSystem.Domain;

namespace AttachmentsSampleSystem.BLL
{
    public partial interface IAttachmentsSampleSystemBLLContext :

        ISecurityBLLContext<IAuthorizationBLLContext, PersistentDomainObjectBase, DomainObjectBase, Guid>,

        IAttachmentsBLLContextContainer,

        ITrackingServiceContainer<PersistentDomainObjectBase>,

        ICryptServiceContainer<CryptSystem>,

        IImpersonateObject<IAttachmentsSampleSystemBLLContext>,

        ITypeResolverContainer<string>,

        Framework.DomainDriven.BLL.Configuration.IConfigurationBLLContextContainer<Framework.Configuration.BLL.IConfigurationBLLContext>,

        IDefaultHierarchicalBLLContext<PersistentDomainObjectBase, Guid>,

        IDateTimeServiceContainer
    {
    }
}
