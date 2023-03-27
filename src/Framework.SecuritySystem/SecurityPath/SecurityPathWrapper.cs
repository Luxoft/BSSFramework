using System.Linq.Expressions;

using Framework.Persistent;


namespace Framework.SecuritySystem;

public abstract class SecurityPathWrapper<TPersistentDomainObjectBase, TDomainObject, TIdent> : SecurityPathBase<TPersistentDomainObjectBase, TDomainObject, TIdent>
        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
        where TDomainObject : class, TPersistentDomainObjectBase
{
    protected SecurityPathWrapper(SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent> securityPath)
    {
        if (securityPath == null) throw new ArgumentNullException(nameof(securityPath));

        this.SecurityPath = securityPath;
    }


    public static implicit operator SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent>(SecurityPathWrapper<TPersistentDomainObjectBase, TDomainObject, TIdent> securityPath)
    {
        return securityPath.SecurityPath;
    }


    protected internal sealed override IEnumerable<Type> GetInternalUsedTypes()
    {
        return this.SecurityPath.GetInternalUsedTypes();
    }

    #region Create

    public SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent> SecurityPath { get; }


    public SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent> Or<TSecurityContext>(Expression<Func<TDomainObject, TSecurityContext>> securityPath)
            where TSecurityContext : class, TPersistentDomainObjectBase, ISecurityContext
    {
        return this.Or(Create(securityPath));
    }

    public SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent> Or<TSecurityContext>(Expression<Func<TDomainObject, IEnumerable<TSecurityContext>>> securityPath)
            where TSecurityContext : class, TPersistentDomainObjectBase, ISecurityContext
    {
        return this.Or(Create(securityPath));
    }

    public SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent> Or(SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent> other)
    {
        if (other == null) throw new ArgumentNullException(nameof(other));

        return new SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent>.OrSecurityPath(this.SecurityPath, other);
    }

    public SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent> Or<TSecurityContext>(Func<IEnumerable<TIdent>, Expression<Func<TDomainObject, bool>>> otherSecurityFilter)
            where TSecurityContext : class, TPersistentDomainObjectBase, ISecurityContext
    {
        if (otherSecurityFilter == null) throw new ArgumentNullException(nameof(otherSecurityFilter));

        return new SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent>.OrSecurityPath(this.SecurityPath, Create<TSecurityContext>(otherSecurityFilter));
    }


    public SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent> And<TSecurityContext>(Expression<Func<TDomainObject, TSecurityContext>> securityPath)
            where TSecurityContext : class, TPersistentDomainObjectBase, ISecurityContext
    {
        return this.And(Create(securityPath));
    }

    public SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent> And<TSecurityContext>(Expression<Func<TDomainObject, IEnumerable<TSecurityContext>>> securityPath)
            where TSecurityContext : class, TPersistentDomainObjectBase, ISecurityContext
    {
        return this.And(Create(securityPath));
    }

    public SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent> And(SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent> other)
    {
        if (other == null) throw new ArgumentNullException(nameof(other));

        return new SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent>.AndSecurityPath(this.SecurityPath, other);
    }

    public SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent> And<TSecurityContext>(Func<IEnumerable<TIdent>, Expression<Func<TDomainObject, bool>>> otherSecurityFilter)
            where TSecurityContext : class, TPersistentDomainObjectBase, ISecurityContext
    {
        if (otherSecurityFilter == null) throw new ArgumentNullException(nameof(otherSecurityFilter));

        return new SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent>.AndSecurityPath(this.SecurityPath, Create<TSecurityContext>(otherSecurityFilter));
    }

    public SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent> And(Expression<Func<TDomainObject, bool>> securityFilter)
    {
        if (securityFilter == null) throw new ArgumentNullException(nameof(securityFilter));

        var condPath = new SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent>.ConditionPath(securityFilter);

        return new SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent>.AndSecurityPath(this.SecurityPath, condPath);
    }

    public SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent> Or(Expression<Func<TDomainObject, bool>> securityFilter)
    {
        if (securityFilter == null) throw new ArgumentNullException(nameof(securityFilter));

        var condPath = new SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent>.ConditionPath(securityFilter);

        return new SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent>.OrSecurityPath(this.SecurityPath, condPath);
    }

    public static SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent> Create<TSecurityContext>(
            Expression<Func<TDomainObject, TSecurityContext>> securityPath,
            SingleSecurityMode mode = SingleSecurityMode.AllowNull)
            where TSecurityContext : class, TPersistentDomainObjectBase, ISecurityContext
    {
        return new SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent>.SingleSecurityPath<TSecurityContext>(securityPath, mode);
    }

    public static SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent> Create<TSecurityContext>(
            Expression<Func<TDomainObject, IEnumerable<TSecurityContext>>> securityPath)
            where TSecurityContext : class, TPersistentDomainObjectBase, ISecurityContext
    {
        return Create(securityPath, ManySecurityPathMode.Any);
    }

    public static SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent> Create<TSecurityContext>(
            Expression<Func<TDomainObject, IEnumerable<TSecurityContext>>> securityPath,
            ManySecurityPathMode mode)
            where TSecurityContext : class, TPersistentDomainObjectBase, ISecurityContext
    {
        return new SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent>.ManySecurityPath<TSecurityContext>(securityPath, mode);
    }

    public static SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent> Create<TNestedObject>(
            Expression<Func<TDomainObject, IEnumerable<TNestedObject>>> nestedPath,
            SecurityPath<TPersistentDomainObjectBase, TNestedObject, TIdent> securityPath,
            ManySecurityPathMode mode)
            where TNestedObject : class, TPersistentDomainObjectBase
    {
        return new SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent>.NestedManySecurityPath<TNestedObject>(nestedPath, securityPath, mode);
    }

    public SecurityPath<TPersistentDomainObjectBase, TNewDomainObject, TIdent> OverrideInput<TNewDomainObject>(Expression<Func<TNewDomainObject, TDomainObject>> selector)
            where TNewDomainObject : class, TPersistentDomainObjectBase
    {
        return this.SecurityPath.OverrideInput(selector);
    }

    public static SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent> Create<TSecurityContext>(
            Func<IEnumerable<TIdent>, Expression<Func<TDomainObject, bool>>> securityFilter)
            where TSecurityContext : class, TPersistentDomainObjectBase, ISecurityContext
    {
        return new SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent>.SecurityPathByIdents<TSecurityContext>(securityFilter);
    }

    public static SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent> Condition(Expression<Func<TDomainObject, bool>> securityFilter)
    {
        return new SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent>.ConditionPath(securityFilter);
    }

    #endregion
}
