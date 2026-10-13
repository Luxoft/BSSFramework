using Framework.Configuration.Domain;
using Framework.Database.InlineAudit.DependencyInjection;

namespace Framework.Configuration.Environment;

public static class InlineAuditSetupExtensions
{
    public static IInlineAuditSetup AddConfigurationInlineAudit(this IInlineAuditSetup rootSetup) =>
        rootSetup.AddDefault<AuditPersistentDomainObjectBase>(v => v.CreateDate, v => v.CreatedBy, v => v.ModifyDate, v => v.ModifiedBy);
}
