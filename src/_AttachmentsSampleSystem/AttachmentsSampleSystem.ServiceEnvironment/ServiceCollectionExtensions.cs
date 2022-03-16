using System;

using Framework.Core;
using Framework.DomainDriven.ServiceModel.IAD;
using Framework.SecuritySystem;
using Framework.Attachments.BLL;

using Microsoft.Extensions.DependencyInjection;

namespace AttachmentsSampleSystem.ServiceEnvironment
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterAttachmentsBLL(this IServiceCollection services)
        {
            return services

                   .AddScopedTransientByWFContainer(c => c.Attachments)
                   .AddScopedTransientByWFContainer<ISecurityOperationResolver<Framework.Attachments.Domain.PersistentDomainObjectBase, Framework.Attachments.AttachmentsSecurityOperationCode>>(c => c.Attachments)
                   .AddScopedTransientByWFContainer<IDisabledSecurityProviderContainer<Framework.Attachments.Domain.PersistentDomainObjectBase>>(c => c.Attachments.SecurityService)
                   .AddScopedTransientByWFContainer<IAttachmentsSecurityPathContainer>(c => c.Attachments.SecurityService)
                   .AddScopedTransientByWFContainer(c => c.Attachments.GetQueryableSource())
                   .AddScopedTransientByWFContainer(c => c.Attachments.SecurityExpressionBuilderFactory)

                   .AddScoped<IAccessDeniedExceptionService<Framework.Attachments.Domain.PersistentDomainObjectBase>, AccessDeniedExceptionService<Framework.Attachments.Domain.PersistentDomainObjectBase, Guid>>()
                   .Self(AttachmentsSecurityServiceBase.Register)
                   .Self(AttachmentsBLLFactoryContainer.RegisterBLLFactory);
        }

        public static IServiceCollection AddScopedTransientByWFContainer<T>(this IServiceCollection services, Func<IAttachmentsBLLContextContainer, T> func)
                where T : class
        {
            return services.AddScopedTransientByContainerBase(container => func((IAttachmentsBLLContextContainer)container));
        }
    }
}
