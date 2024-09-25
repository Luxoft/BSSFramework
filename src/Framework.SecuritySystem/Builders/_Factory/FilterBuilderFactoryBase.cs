using Framework.Core;

namespace Framework.SecuritySystem.Builders._Factory;

public abstract class FilterBuilderFactoryBase<TDomainObject, TBuilder>
{
    public virtual TBuilder CreateBuilder(SecurityPath<TDomainObject> baseSecurityPath)
    {
        var securityPathType = baseSecurityPath.GetType();

        if (baseSecurityPath is SecurityPath<TDomainObject>.ConditionPath conditionPath)
        {
            return this.CreateBuilder(conditionPath);
        }
        else if (baseSecurityPath is SecurityPath<TDomainObject>.AndSecurityPath andSecurityPath)
        {
            return this.CreateBuilder(andSecurityPath);
        }
        else if (baseSecurityPath is SecurityPath<TDomainObject>.OrSecurityPath orSecurityPath)
        {
            return this.CreateBuilder(orSecurityPath);
        }
        else if (securityPathType.IsGenericTypeImplementation(typeof(SecurityPath<>.NestedManySecurityPath<>)))
        {
            return new Func<SecurityPath<TDomainObject>.NestedManySecurityPath<TDomainObject>, TBuilder>(this.CreateBuilder)
                   .CreateGenericMethod(securityPathType.GetGenericArguments().Skip(1).ToArray())
                   .Invoke<TBuilder>(this, baseSecurityPath);
        }
        else if (securityPathType.BaseType.Maybe(baseType => baseType.IsGenericTypeImplementation(typeof(SecurityPath<>))))
        {
            return new Func<SecurityPath<TDomainObject>, TBuilder>(this.CreateSecurityContextBuilder<ISecurityContext>)
                   .CreateGenericMethod(securityPathType.GetGenericArguments().Skip(1).ToArray())
                   .Invoke<TBuilder>(this, baseSecurityPath);
        }
        else
        {
            throw new ArgumentOutOfRangeException(nameof(baseSecurityPath));
        }
    }

    protected abstract TBuilder CreateBuilder(SecurityPath<TDomainObject>.ConditionPath securityPath);

    protected abstract TBuilder CreateBuilder<TSecurityContext>(
        SecurityPath<TDomainObject>.SingleSecurityPath<TSecurityContext> securityPath)
        where TSecurityContext : class, ISecurityContext;

    protected abstract TBuilder CreateBuilder<TSecurityContext>(SecurityPath<TDomainObject>.ManySecurityPath<TSecurityContext> securityPath)
        where TSecurityContext : class, ISecurityContext;

    protected abstract TBuilder CreateBuilder(SecurityPath<TDomainObject>.OrSecurityPath securityPath);

    protected abstract TBuilder CreateBuilder(SecurityPath<TDomainObject>.AndSecurityPath securityPath);

    protected abstract TBuilder CreateBuilder<TNestedObject>(
        SecurityPath<TDomainObject>.NestedManySecurityPath<TNestedObject> securityPath);

    private TBuilder CreateSecurityContextBuilder<TSecurityContext>(SecurityPath<TDomainObject> securityPath)
        where TSecurityContext : class, ISecurityContext =>
        securityPath switch
        {
            SecurityPath<TDomainObject>.SingleSecurityPath<TSecurityContext> singleSecurityPath => this.CreateBuilder(singleSecurityPath),
            SecurityPath<TDomainObject>.ManySecurityPath<TSecurityContext> manySecurityPath => this.CreateBuilder(manySecurityPath),
            _ => throw new ArgumentOutOfRangeException(nameof(securityPath))
        };
}
