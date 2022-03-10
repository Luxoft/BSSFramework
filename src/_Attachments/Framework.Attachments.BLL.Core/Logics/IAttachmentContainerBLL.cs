using System;
using System.Collections.Generic;

using Framework.Attachments.Domain;
using Framework.Persistent;

using JetBrains.Annotations;

namespace Framework.Attachments.BLL
{
    public partial interface IAttachmentContainerBLL
    {
        IList<AttachmentContainer> GetNotSynchronizated();

        AttachmentContainer GetObjectBy([NotNull] Type type, Guid domainObjectId);

        AttachmentContainer GetObjectBy<TDomainObject>([NotNull] TDomainObject domainObject)
            where TDomainObject : class, IIdentityObject<Guid>;
    }
}
