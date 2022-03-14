using System;
using System.Collections.Generic;

using Framework.Attachments.Domain;
using Framework.Persistent;

using JetBrains.Annotations;

namespace Framework.Attachments.BLL
{
    public partial interface IAttachmentBLL : Framework.DomainDriven.BLL.Security.IDefaultSecurityDomainBLLBase<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Domain.PersistentDomainObjectBase, Framework.Attachments.Domain.Attachment, System.Guid>
    {
        IList<Attachment> GetObjectsBy([NotNull] Type type, Guid domainObjectId);

        IList<Attachment> GetObjectsBy<TDomainObject>([NotNull] TDomainObject domainObject)
            where TDomainObject : IIdentityObject<Guid>;
    }
}
