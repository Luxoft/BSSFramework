using System;
using System.Linq;
using System.Reflection;

using Framework.Core;
using Framework.Persistent;


namespace Framework.SecuritySystem.Rules.Builders;

public abstract class SecurityExpressionBuilderFactoryBase<TPersistentDomainObjectBase, TIdent> : ISecurityExpressionBuilderFactory<TPersistentDomainObjectBase, TIdent>

        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
{
    protected SecurityExpressionBuilderFactoryBase()
            : base()
    {
    }

    public ISecurityExpressionBuilder<TPersistentDomainObjectBase, TDomainObject, TIdent> CreateBuilder<TDomainObject>(SecurityPathBase<TPersistentDomainObjectBase, TDomainObject, TIdent> path)
            where TDomainObject : class, TPersistentDomainObjectBase
    {
        if (path == null) throw new ArgumentNullException(nameof(path));

        if (path is SecurityPathWrapper<TPersistentDomainObjectBase, TDomainObject, TIdent>)
        {
            return this.CreateBuilder(path as SecurityPathWrapper<TPersistentDomainObjectBase, TDomainObject, TIdent>);
        }

        if (path is SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent>.ConditionPath)
        {
            return this.CreateBuilder(path as SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent>.ConditionPath);
        }

        if (path is SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent>.AndSecurityPath)
        {
            return this.CreateBuilder(path as SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent>.AndSecurityPath);
        }

        if (path is SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent>.OrSecurityPath)
        {
            return this.CreateBuilder(path as SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent>.OrSecurityPath);
        }

        var pathType = path.GetType();

        if (pathType.IsGenericTypeImplementation(typeof(SecurityPath<,,>.NestedManySecurityPath<>)))
        {
            var func = new Func<SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent>.NestedManySecurityPath<TDomainObject>, ISecurityExpressionBuilder<TPersistentDomainObjectBase, TDomainObject, TIdent>>(this.CreateNestedBuilder);

            var args = pathType.GetGenericArguments().ElementsAt(1, 3).ToArray();

            var method = func.Method.GetGenericMethodDefinition().MakeGenericMethod(args);

            return method.Invoke(this, new object[] { path }) as ISecurityExpressionBuilder<TPersistentDomainObjectBase, TDomainObject, TIdent>;
        }

        if (pathType.BaseType.Maybe(baseType => baseType.IsGenericTypeImplementation(typeof(SecurityPath<,,>.FilterSecurityPath<>))))
        {
            var genericMethod = typeof(SecurityExpressionBuilderFactoryBase<TPersistentDomainObjectBase, TIdent>).GetMethod(nameof(this.CreateGenericBuilder), BindingFlags.Instance | BindingFlags.NonPublic);

            var args = pathType.GetGenericArguments().ElementsAt(1, 3).ToArray();

            var method = genericMethod.GetGenericMethodDefinition().MakeGenericMethod(args);

            return method.Invoke(this, new object[] { path }) as ISecurityExpressionBuilder<TPersistentDomainObjectBase, TDomainObject, TIdent>;
        }

        throw new ArgumentOutOfRangeException(nameof(path));
    }

    private ISecurityExpressionBuilder<TPersistentDomainObjectBase, TDomainObject, TIdent> CreateGenericBuilder<TDomainObject, TSecurityContext>(
            SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent>.FilterSecurityPath<TSecurityContext> path)
            where TSecurityContext : class, TPersistentDomainObjectBase, ISecurityContext
            where TDomainObject : class, TPersistentDomainObjectBase
    {
        if (path == null) throw new ArgumentNullException(nameof(path));

        if (path is SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent>.ManySecurityPath<TSecurityContext>)
        {
            return this.CreateBuilder(path as SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent>.ManySecurityPath<TSecurityContext>);
        }
        else if (path is SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent>.SingleSecurityPath<TSecurityContext>)
        {
            return this.CreateBuilder(path as SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent>.SingleSecurityPath<TSecurityContext>);
        }
        else if (path is SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent>.SecurityPathByIdents<TSecurityContext>)
        {
            return this.CreateBuilder(path as SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent>.SecurityPathByIdents<TSecurityContext>);
        }
        else
        {
            throw new ArgumentOutOfRangeException(nameof(path));
        }
    }

    private ISecurityExpressionBuilder<TPersistentDomainObjectBase, TDomainObject, TIdent> CreateNestedBuilder<TDomainObject, TNestedObject>(
            SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent>.NestedManySecurityPath<TNestedObject> path)
            where TNestedObject : class, TPersistentDomainObjectBase
            where TDomainObject : class, TPersistentDomainObjectBase
    {
        if (path == null) throw new ArgumentNullException(nameof(path));

        return this.CreateBuilder(path);
    }

    protected virtual ISecurityExpressionBuilder<TPersistentDomainObjectBase, TDomainObject, TIdent> CreateBuilder<TDomainObject>(SecurityPathWrapper<TPersistentDomainObjectBase, TDomainObject, TIdent> securityPath)
            where TDomainObject : class, TPersistentDomainObjectBase
    {
        if (securityPath == null) throw new ArgumentNullException(nameof(securityPath));

        return this.CreateBuilder(securityPath.SecurityPath);
    }

    protected abstract ISecurityExpressionBuilder<TPersistentDomainObjectBase, TDomainObject, TIdent> CreateBuilder<TDomainObject>(SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent>.ConditionPath securityPath)
            where TDomainObject : class, TPersistentDomainObjectBase;

    protected abstract ISecurityExpressionBuilder<TPersistentDomainObjectBase, TDomainObject, TIdent> CreateBuilder<TDomainObject, TSecurityContext>(SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent>.SecurityPathByIdents<TSecurityContext> securityPath)
            where TSecurityContext : class, TPersistentDomainObjectBase, ISecurityContext
            where TDomainObject : class, TPersistentDomainObjectBase;

    protected abstract ISecurityExpressionBuilder<TPersistentDomainObjectBase, TDomainObject, TIdent> CreateBuilder<TDomainObject, TSecurityContext>(SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent>.ManySecurityPath<TSecurityContext> securityPath)
            where TSecurityContext : class, TPersistentDomainObjectBase, ISecurityContext
            where TDomainObject : class, TPersistentDomainObjectBase;

    protected abstract ISecurityExpressionBuilder<TPersistentDomainObjectBase, TDomainObject, TIdent> CreateBuilder<TDomainObject, TSecurityContext>(SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent>.SingleSecurityPath<TSecurityContext> securityPath)
            where TSecurityContext : class, TPersistentDomainObjectBase, ISecurityContext
            where TDomainObject : class, TPersistentDomainObjectBase;

    protected abstract ISecurityExpressionBuilder<TPersistentDomainObjectBase, TDomainObject, TIdent> CreateBuilder<TDomainObject>(SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent>.OrSecurityPath securityPath)
            where TDomainObject : class, TPersistentDomainObjectBase;

    protected abstract ISecurityExpressionBuilder<TPersistentDomainObjectBase, TDomainObject, TIdent> CreateBuilder<TDomainObject>(SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent>.AndSecurityPath securityPath)
            where TDomainObject : class, TPersistentDomainObjectBase;

    protected abstract ISecurityExpressionBuilder<TPersistentDomainObjectBase, TDomainObject, TIdent> CreateBuilder<TDomainObject, TNestedObject>(SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent>.NestedManySecurityPath<TNestedObject> securityPath)
            where TNestedObject : class, TPersistentDomainObjectBase
            where TDomainObject : class, TPersistentDomainObjectBase;
}
