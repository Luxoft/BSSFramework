using System;
using System.Collections.Generic;

using Framework.Attachments.Domain;

namespace Framework.Attachments.BLL
{
    public interface ITargetSystemService : IAttachmentSecurityProviderSource
    {
        TargetSystem TargetSystem
        {
            get;
        }

        bool HasAttachments { get; }
        bool IsAssignable(Type domainType);

        void TryRemoveAttachments(Array domainObjects);

        void TryDenormalizeHasAttachmentFlag(AttachmentContainer container, bool value);

        IEnumerable<Guid> GetNotExistsObjects(DomainType domainType, IEnumerable<Guid> idents);
    }

    public interface ITargetSystemService<in TPersistentDomainObjectBase> : ITargetSystemService
        where TPersistentDomainObjectBase : class
    {
        void TryRemoveAttachments<TDomainObject>(IEnumerable<TDomainObject> domainObjects)
            where TDomainObject : class, TPersistentDomainObjectBase;
    }
}
