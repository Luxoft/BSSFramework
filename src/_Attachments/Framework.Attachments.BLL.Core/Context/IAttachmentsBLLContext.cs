using System;
using System.Collections.Generic;

using Framework.Attachments.Domain;
using Framework.Authorization.BLL;
using Framework.DomainDriven.BLL.Security;

namespace Framework.Attachments.BLL
{
    public partial interface IAttachmentsBLLContext : ISecurityBLLContext<IAuthorizationBLLContext, PersistentDomainObjectBase, DomainObjectBase, Guid>
    {
        ITargetSystemService GetPersistentTargetSystemService(TargetSystem targetSystem);

        IEnumerable<ITargetSystemService> GetPersistentTargetSystemServices();

        DomainType GetDomainType(Type type, bool throwOnNotFound);
    }
}
