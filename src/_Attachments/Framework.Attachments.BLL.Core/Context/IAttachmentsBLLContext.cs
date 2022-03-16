using System;
using System.Collections.Generic;

using Framework.Attachments.Domain;
using Framework.Authorization.BLL;
using Framework.Core;
using Framework.DomainDriven.BLL.Configuration;
using Framework.DomainDriven.BLL.Security;

namespace Framework.Attachments.BLL
{
    public partial interface IAttachmentsBLLContext :

            ISecurityBLLContext<IAuthorizationBLLContext, PersistentDomainObjectBase, DomainObjectBase, Guid>,
            IConfigurationBLLContextContainer<Framework.Configuration.BLL.IConfigurationBLLContext>,
            ITypeResolverContainer<string>
    {
        ITargetSystemService GetPersistentTargetSystemService(TargetSystem targetSystem);

        IEnumerable<ITargetSystemService> GetPersistentTargetSystemServices();

        DomainType GetDomainType(Type type, bool throwOnNotFound);
    }
}
