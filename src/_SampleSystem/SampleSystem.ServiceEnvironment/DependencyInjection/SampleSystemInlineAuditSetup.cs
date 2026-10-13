using Framework.Authorization.Environment;
using Framework.Configuration.Environment;

using Framework.Database.InlineAudit;
using Framework.Database.InlineAudit.DependencyInjection;

using SampleSystem.Domain.Employee;

namespace SampleSystem.ServiceEnvironment.DependencyInjection;

public static class InlineAuditSetupExtensions
{
    public static IInlineAuditSetup AddSampleSystemInlineAudit(this IInlineAuditSetup rootSetup) =>
        rootSetup
            .AddAuthorizationInlineAudit()
            .AddConfigurationInlineAudit()
            .AddDefault<SampleSystem.Domain.AuditPersistentDomainObjectBase>(v => v.CreateDate, v => v.CreatedBy, v => v.ModifyDate, v => v.ModifiedBy)
            .For<SampleSystem.Domain.ExtendedInlineAuditObj>(innerSetup =>
            {
                innerSetup.Add<Employee?, CurrentUserAuditValueResolver<Employee?>>(v => v.CreatedByEmployee, InlineAuditAction.Create)
                          .Add<Employee?, CurrentUserAuditValueResolver<Employee?>>(v => v.ModifiedByEmployee, InlineAuditAction.Modify);
            });
}
