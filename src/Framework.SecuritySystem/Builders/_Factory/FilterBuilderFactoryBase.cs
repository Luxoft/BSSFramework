using Framework.Core;

namespace Framework.SecuritySystem.Builders._Factory;

public abstract class FilterBuilderFactoryBase<TDomainObject, TBuilder>
{
    public virtual TBuilder CreateBuilder(
        SecurityPath<TDomainObject> baseSecurityPath,
        IReadOnlyList<SecurityContextRestriction> securityContextRestrictions)
    {
        var securityPathType = baseSecurityPath.GetType();

        if (baseSecurityPath is SecurityPath<TDomainObject>.ConditionPath conditionPath)
        {
            return this.CreateBuilder(conditionPath);
        }
        else if (baseSecurityPath is SecurityPath<TDomainObject>.AndSecurityPath andSecurityPath)
        {
            return this.CreateBuilder(andSecurityPath, securityContextRestrictions);
        }
        else if (baseSecurityPath is SecurityPath<TDomainObject>.OrSecurityPath orSecurityPath)
        {
            return this.CreateBuilder(orSecurityPath, securityContextRestrictions);
        }
        else if (securityPathType.IsGenericTypeImplementation(typeof(SecurityPath<>.NestedManySecurityPath<>)))
        {
            return new Func<SecurityPath<TDomainObject>.NestedManySecurityPath<TDomainObject>, IReadOnlyList<SecurityContextRestriction>, TBuilder>(this.CreateBuilder)
                   .CreateGenericMethod(securityPathType.GetGenericArguments().Skip(1).ToArray())
                   .Invoke<TBuilder>(this, baseSecurityPath, securityContextRestrictions);
        }
        else if (securityPathType.BaseType.Maybe(baseType => baseType.IsGenericTypeImplementation(typeof(SecurityPath<>))))
        {
            return new Func<SecurityPath<TDomainObject>, IReadOnlyList<SecurityContextRestriction>, TBuilder>(
                       this.CreateSecurityContextBuilder<ISecurityContext>)
                   .CreateGenericMethod(securityPathType.GetGenericArguments().Skip(1).ToArray())
                   .Invoke<TBuilder>(this, baseSecurityPath, securityContextRestrictions);
        }
        else
        {
            throw new ArgumentOutOfRangeException(nameof(baseSecurityPath));
        }
    }

    protected abstract TBuilder CreateBuilder(SecurityPath<TDomainObject>.ConditionPath securityPath);

    protected abstract TBuilder CreateBuilder<TSecurityContext>(
        SecurityPath<TDomainObject>.SingleSecurityPath<TSecurityContext> securityPath,
        SecurityContextRestriction<TSecurityContext>? securityContextRestriction)
        where TSecurityContext : class, ISecurityContext;

    protected abstract TBuilder CreateBuilder<TSecurityContext>(
        SecurityPath<TDomainObject>.ManySecurityPath<TSecurityContext> securityPath,
        SecurityContextRestriction<TSecurityContext>? securityContextRestriction)
        where TSecurityContext : class, ISecurityContext;

    protected abstract TBuilder CreateBuilder(
        SecurityPath<TDomainObject>.OrSecurityPath securityPath,
        IReadOnlyList<SecurityContextRestriction> securityContextRestrictions);

    protected abstract TBuilder CreateBuilder(
        SecurityPath<TDomainObject>.AndSecurityPath securityPath,
        IReadOnlyList<SecurityContextRestriction> securityContextRestrictions);

    protected abstract TBuilder CreateBuilder<TNestedObject>(
        SecurityPath<TDomainObject>.NestedManySecurityPath<TNestedObject> securityPath,
        IReadOnlyList<SecurityContextRestriction> securityContextRestrictions);

    private TBuilder CreateSecurityContextBuilder<TSecurityContext>(
        SecurityPath<TDomainObject> securityPath,
        IReadOnlyList<SecurityContextRestriction> securityContextRestrictions)
        where TSecurityContext : class, ISecurityContext
    {
        var restrictionFilterInfo =
            (SecurityContextRestriction<TSecurityContext>?)securityContextRestrictions.SingleOrDefault(
                v => v.SecurityContextType == typeof(TSecurityContext));

        return securityPath switch
        {
            SecurityPath<TDomainObject>.SingleSecurityPath<TSecurityContext> singleSecurityPath => this.CreateBuilder(
                singleSecurityPath,
                restrictionFilterInfo),

            SecurityPath<TDomainObject>.ManySecurityPath<TSecurityContext> manySecurityPath => this.CreateBuilder(
                manySecurityPath,
                restrictionFilterInfo),

            _ => throw new ArgumentOutOfRangeException(nameof(securityPath))
        };
    }
}
