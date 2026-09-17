using Framework.Database.InlineAudit;
using Framework.Database.InlineAudit.DependencyInjection;

namespace SampleSystem.ServiceEnvironment.DependencyInjection;

public static class InlineAuditSetupExtensions
{
    public static IInlineAuditSetup AddSampleSystemInlineAudit(this IInlineAuditSetup rootSetup) =>
        rootSetup
            .For<Framework.Authorization.Domain.AuditPersistentDomainObjectBase>(innerSetup =>
            {
                innerSetup.Add(v => v.CreateDate, InlineAuditAction.Create, AuditValueResolverHeader.Now)
                          .Add(v => v.CreatedBy, InlineAuditAction.Create, AuditValueResolverHeader.CurrentUser)
                          .Add(v => v.ModifyDate, InlineAuditAction.Modify, AuditValueResolverHeader.Now)
                          .Add(v => v.ModifiedBy, InlineAuditAction.Modify, AuditValueResolverHeader.CurrentUser);
            })
            .For<Framework.Configuration.Domain.AuditPersistentDomainObjectBase>(innerSetup =>
            {
                innerSetup.Add(v => v.CreateDate, InlineAuditAction.Create, AuditValueResolverHeader.Now)
                          .Add(v => v.CreatedBy, InlineAuditAction.Create, AuditValueResolverHeader.CurrentUser)
                          .Add(v => v.ModifyDate, InlineAuditAction.Modify, AuditValueResolverHeader.Now)
                          .Add(v => v.ModifiedBy, InlineAuditAction.Modify, AuditValueResolverHeader.CurrentUser);
            })
            .For<SampleSystem.Domain.AuditPersistentDomainObjectBase>(innerSetup =>
            {
                innerSetup.Add(v => v.CreateDate, InlineAuditAction.Create, AuditValueResolverHeader.Now)
                          .Add(v => v.CreatedBy, InlineAuditAction.Create, AuditValueResolverHeader.CurrentUser)
                          .Add(v => v.ModifyDate, InlineAuditAction.Modify, AuditValueResolverHeader.Now)
                          .Add(v => v.ModifiedBy, InlineAuditAction.Modify, AuditValueResolverHeader.CurrentUser);
            });
}
