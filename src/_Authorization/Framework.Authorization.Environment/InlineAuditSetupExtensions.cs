using Framework.Authorization.Domain;
using Framework.Database.InlineAudit.DependencyInjection;

namespace Framework.Authorization.Environment;

public static class InlineAuditSetupExtensions
{
    public static IInlineAuditSetup AddAuthorizationInlineAudit(this IInlineAuditSetup rootSetup) =>
        rootSetup.AddDefault<AuditPersistentDomainObjectBase>(v => v.CreateDate, v => v.CreatedBy, v => v.ModifyDate, v => v.ModifiedBy);
}
