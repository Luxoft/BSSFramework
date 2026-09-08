using System.Linq.Expressions;

namespace Framework.Database.InlineAudit.DependencyInjection;

public interface IInlineAuditSetup
{
    IInlineAuditSetup For<TDomainObject>(Action<IInlineAuditSetup<TDomainObject>> domainSetup);
}


public interface IInlineAuditSetup<TDomainObject>
{
    IInlineAuditSetup<TDomainObject> Add<TProperty, TAuditValueResolver>(Expression<Func<TDomainObject, TProperty?>> path, InlineAuditAction inlineAuditAction)
        where TAuditValueResolver : IAuditValueResolver<TProperty>
        where TProperty : notnull;

    IInlineAuditSetup<TDomainObject> Add<TProperty>(
        Expression<Func<TDomainObject, TProperty?>> path,
        InlineAuditAction inlineAuditAction,
        AuditValueResolverHeader<TProperty> auditValueResolverHeader)
        where TProperty : notnull;
}
