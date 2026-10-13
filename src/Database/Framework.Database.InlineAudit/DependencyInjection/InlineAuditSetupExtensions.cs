using System.Linq.Expressions;

namespace Framework.Database.InlineAudit.DependencyInjection;

public static class InlineAuditSetupExtensions
{
    public static IInlineAuditSetup AddDefault<TDomainObject>(
        this IInlineAuditSetup rootSetup,
        Expression<Func<TDomainObject, DateTime?>> createDatePath,
        Expression<Func<TDomainObject, string?>> createdByPath,
        Expression<Func<TDomainObject, DateTime?>> modifyDatePath,
        Expression<Func<TDomainObject, string?>> modifiedByPath) =>
        rootSetup.For<TDomainObject>(innerSetup =>
        {
            innerSetup.Add(createDatePath, InlineAuditAction.Create, AuditValueResolverHeader.Now)
                      .Add(createdByPath, InlineAuditAction.Create, AuditValueResolverHeader.CurrentUser)
                      .Add(modifyDatePath, InlineAuditAction.Modify, AuditValueResolverHeader.Now)
                      .Add(modifiedByPath, InlineAuditAction.Modify, AuditValueResolverHeader.CurrentUser);
        });
}
