using System.Reflection;

using Framework.Core;
using Framework.Persistent;

namespace Framework.SecuritySystem.Rules.Builders;

public abstract class SecurityExpressionBuilderFactoryBase<TIdent> : ISecurityExpressionBuilderFactory
{
    public ISecurityExpressionBuilder<TDomainObject> CreateBuilder<TDomainObject>(SecurityPath<TDomainObject> path)
    {
        var func =
            new Func<SecurityPath<IIdentityObject<TIdent>>.NestedManySecurityPath<IIdentityObject<TIdent>>, ISecurityExpressionBuilder<IIdentityObject<TIdent>>>(
                this.CreateBuilderInternal);

        return func.CreateGenericMethod(typeof(TDomainObject)).Invoke<ISecurityExpressionBuilder<TDomainObject>>(this, path);
    }

    private ISecurityExpressionBuilder<TDomainObject> CreateBuilderInternal<TDomainObject>(SecurityPath<TDomainObject> path)
        where TDomainObject : class, IIdentityObject<TIdent>
    {
        if (path == null) throw new ArgumentNullException(nameof(path));

        var pathType = path.GetType();

        if (path is SecurityPath<TDomainObject>.ConditionPath conditionPath)
        {
            return this.CreateBuilder(conditionPath);
        }
        else if (path is SecurityPath<TDomainObject>.AndSecurityPath andSecurityPath)
        {
            return this.CreateBuilder(andSecurityPath);
        }
        else if (path is SecurityPath<TDomainObject>.OrSecurityPath securityPath)
        {
            return this.CreateBuilder(securityPath);
        }
        else if (pathType.IsGenericTypeImplementation(typeof(SecurityPath<>.NestedManySecurityPath<>)))
        {
            var func =
                new Func<SecurityPath<TDomainObject>.NestedManySecurityPath<TDomainObject>, ISecurityExpressionBuilder<TDomainObject>>(
                    this.CreateNestedBuilder);

            var args = pathType.GetGenericArguments().ToArray();

            var method = func.Method.GetGenericMethodDefinition().MakeGenericMethod(args);

            return method.Invoke(this, [path]) as ISecurityExpressionBuilder<TDomainObject>;
        }
        else if (pathType.BaseType.Maybe(baseType => baseType.IsGenericTypeImplementation(typeof(SecurityPath<>))))
        {
            var genericMethod = typeof(SecurityExpressionBuilderFactoryBase<TIdent>).GetMethod(
                nameof(this.CreateGenericBuilder),
                BindingFlags.Instance | BindingFlags.NonPublic)!;

            var args = pathType.GetGenericArguments().ToArray();

            var method = genericMethod.GetGenericMethodDefinition().MakeGenericMethod(args);

            return method.Invoke(this, [path]) as ISecurityExpressionBuilder<TDomainObject>;
        }
        else
        {
            throw new ArgumentOutOfRangeException(nameof(path));
        }
    }

    private ISecurityExpressionBuilder<TDomainObject> CreateGenericBuilder<TDomainObject, TSecurityContext>(
        SecurityPath<TDomainObject> path)
        where TSecurityContext : class, IIdentityObject<TIdent>, ISecurityContext
        where TDomainObject : class, IIdentityObject<TIdent>
    {
        return path switch
        {
            null => throw new ArgumentNullException(nameof(path)),
            SecurityPath<TDomainObject>.ManySecurityPath<TSecurityContext> manySecurityPath => this.CreateBuilder(manySecurityPath),
            SecurityPath<TDomainObject>.SingleSecurityPath<TSecurityContext> securityPath => this.CreateBuilder(securityPath),
            _ => throw new ArgumentOutOfRangeException(nameof(path))
        };
    }

    private ISecurityExpressionBuilder<TDomainObject> CreateNestedBuilder<TDomainObject, TNestedObject>(
        SecurityPath<TDomainObject>.NestedManySecurityPath<TNestedObject> path)
        where TNestedObject : class, IIdentityObject<TIdent>
        where TDomainObject : class, IIdentityObject<TIdent>
    {
        if (path == null) throw new ArgumentNullException(nameof(path));

        return this.CreateBuilder(path);
    }

    protected abstract ISecurityExpressionBuilder<TDomainObject> CreateBuilder<TDomainObject>(
        SecurityPath<TDomainObject>.ConditionPath securityPath)
        where TDomainObject : class, IIdentityObject<TIdent>;

    protected abstract ISecurityExpressionBuilder<TDomainObject> CreateBuilder<TDomainObject, TSecurityContext>(
        SecurityPath<TDomainObject>.ManySecurityPath<TSecurityContext> securityPath)
        where TSecurityContext : class, ISecurityContext, IIdentityObject<TIdent>
        where TDomainObject : class, IIdentityObject<TIdent>;

    protected abstract ISecurityExpressionBuilder<TDomainObject> CreateBuilder<TDomainObject, TSecurityContext>(
        SecurityPath<TDomainObject>.SingleSecurityPath<TSecurityContext> securityPath)
        where TSecurityContext : class, IIdentityObject<TIdent>, ISecurityContext
        where TDomainObject : class, IIdentityObject<TIdent>;

    protected abstract ISecurityExpressionBuilder<TDomainObject> CreateBuilder<TDomainObject>(
        SecurityPath<TDomainObject>.OrSecurityPath securityPath)
        where TDomainObject : class, IIdentityObject<TIdent>;

    protected abstract ISecurityExpressionBuilder<TDomainObject> CreateBuilder<TDomainObject>(
        SecurityPath<TDomainObject>.AndSecurityPath securityPath)
        where TDomainObject : class, IIdentityObject<TIdent>;

    protected abstract ISecurityExpressionBuilder<TDomainObject> CreateBuilder<TDomainObject, TNestedObject>(
        SecurityPath<TDomainObject>.NestedManySecurityPath<TNestedObject> securityPath)
        where TNestedObject : class, IIdentityObject<TIdent>
        where TDomainObject : class, IIdentityObject<TIdent>;
}
