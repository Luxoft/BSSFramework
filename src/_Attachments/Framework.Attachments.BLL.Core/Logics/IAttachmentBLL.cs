using System;
using System.Collections.Generic;

using Framework.Attachments.Domain;
using Framework.Persistent;

using JetBrains.Annotations;

namespace Framework.Attachments.BLL
{
    public partial interface IAttachmentBLL
    {
        IList<Attachment> GetObjectsBy([NotNull] Type type, Guid domainObjectId);

        IList<Attachment> GetObjectsBy<TDomainObject>([NotNull] TDomainObject domainObject)
            where TDomainObject : IIdentityObject<Guid>;
    }
}
