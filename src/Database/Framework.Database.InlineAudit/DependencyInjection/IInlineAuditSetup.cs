using System.Linq.Expressions;

namespace Framework.Database.InlineAudit.DependencyInjection;

public interface IInlineAuditSetup
{
    IInlineAuditSetup<TDomainObject> For<TDomainObject>(Action<IInlineAuditSetup<TDomainObject>> domainSetup);
}


public interface IInlineAuditSetup<TDomainObject>
{
    IInlineAuditSetup Add<TProperty, TAuditValueResolver>(Expression<Func<TDomainObject, TProperty?>> path, InlineAuditType inlineAuditType)
        where TAuditValueResolver : IAuditValueResolver<TProperty>;

    IInlineAuditSetup Add<TProperty>(
        Expression<Func<TDomainObject, TProperty?>> path,
        InlineAuditType inlineAuditType,
        AuditValueResolverHeader<TProperty> auditValueResolverHeader);
}
