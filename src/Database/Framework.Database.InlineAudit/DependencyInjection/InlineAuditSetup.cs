using System.Linq.Expressions;

using Anch.Core;
using Anch.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Database.InlineAudit.DependencyInjection;

public class InlineAuditSetup<TDomainObject> : IInlineAuditSetup<TDomainObject>, IServiceInitializer
{
    private readonly List<InlineAuditBinding> bindings = [];

    public IInlineAuditSetup<TDomainObject> Add<TProperty, TAuditValueResolver>(
        Expression<Func<TDomainObject, TProperty?>> path,
        InlineAuditAction inlineAuditAction)
        where TProperty : notnull
        where TAuditValueResolver : IAuditValueResolver<TProperty>
    {
        this.bindings.Add(new InlineAuditBinding<TDomainObject, TProperty, TAuditValueResolver>(path.ToPropertyAccessors()) { Action = inlineAuditAction });

        return this;
    }

    public IInlineAuditSetup<TDomainObject> Add<TProperty>(Expression<Func<TDomainObject, TProperty?>> path, InlineAuditAction inlineAuditAction, AuditValueResolverHeader<TProperty> auditValueResolverHeader)
        where TProperty : notnull =>
        new Func<Expression<Func<TDomainObject, TProperty>>, InlineAuditAction, IInlineAuditSetup<TDomainObject>>(this.Add<TProperty, IAuditValueResolver<TProperty>>)
            .CreateGenericMethod(typeof(TProperty), auditValueResolverHeader.ResolverType)
            .Invoke<IInlineAuditSetup<TDomainObject>>(path, inlineAuditAction);

    public void Initialize(IServiceCollection services)
    {
        foreach (var binding in this.bindings)
        {
            services.AddSingleton(binding);
        }
    }
}

public class InlineAuditSetup : IInlineAuditSetup, IServiceInitializer
{
    private readonly List<IServiceInitializer> domainInitializers = [];

    public IInlineAuditSetup For<TDomainObject>(Action<IInlineAuditSetup<TDomainObject>> domainSetup)
    {
        var domainInitializer = new InlineAuditSetup<TDomainObject>();

        domainSetup(domainInitializer);

        this.domainInitializers.Add(domainInitializer);

        return this;
    }

    public void Initialize(IServiceCollection services)
    {
        foreach (var domainInitializer in this.domainInitializers)
        {
            domainInitializer.Initialize(services);
        }
    }
}
