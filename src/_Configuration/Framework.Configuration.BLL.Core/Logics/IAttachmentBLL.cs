using System;
using System.Collections.Generic;

using Framework.Configuration.Domain;
using Framework.Persistent;

using JetBrains.Annotations;

namespace Framework.Configuration.BLL
{
    public partial interface IAttachmentBLL
    {
        IList<Attachment> GetObjectsBy([NotNull] Type type, Guid domainObjectId);

        IList<Attachment> GetObjectsBy<TDomainObject>([NotNull] TDomainObject domainObject)
            where TDomainObject : IIdentityObject<Guid>;
    }
}
