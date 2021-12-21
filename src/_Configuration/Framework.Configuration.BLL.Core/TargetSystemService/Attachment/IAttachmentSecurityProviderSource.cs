using System;
using System.Linq.Expressions;

using Framework.Configuration.Domain;
using Framework.SecuritySystem;

namespace Framework.Configuration.BLL
{
    public interface IAttachmentSecurityProviderSource
    {
        ISecurityProvider<TDomainObject> GetAttachmentSecurityProvider<TDomainObject>(Expression<Func<TDomainObject, AttachmentContainer>> containerPath, DomainType mainDomainType, BLLSecurityMode securityMode)
            where TDomainObject : PersistentDomainObjectBase;
    }

    public static class AttachmentSecurityProviderSourceExtensions
    {
        public static ISecurityProvider<Attachment> GetAttachmentSecurityProvider(this IAttachmentSecurityProviderSource source, DomainType mainDomainType, BLLSecurityMode securityMode)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (mainDomainType == null) throw new ArgumentNullException(nameof(mainDomainType));

            return source.GetAttachmentSecurityProvider<Attachment>(attachment => attachment.Container, mainDomainType, securityMode);
        }
    }
}
