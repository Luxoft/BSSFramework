using System.Linq.Expressions;

using Framework.Core;
using Framework.Core.ExpressionComparers;

namespace Framework.SecuritySystem;

/// <summary>
/// Контекстное правило доступа (лямбды)
/// </summary>
/// <typeparam name="TDomainObject"></typeparam>
public abstract record SecurityPath<TDomainObject>
{
    public IEnumerable<Type> GetUsedTypes() => this.GetInternalUsedTypes().Distinct();

    public abstract SecurityPath<TNewDomainObject> OverrideInput<TNewDomainObject>(
        Expression<Func<TNewDomainObject, TDomainObject>> selector);

    public static SecurityPath<TDomainObject> Empty { get; } = Condition(_ => true);

    protected abstract IEnumerable<Type> GetInternalUsedTypes();

    #region Create

    public SecurityPath<TDomainObject> And(Expression<Func<TDomainObject, bool>> securityFilter) => this.And(Condition(securityFilter));

    public SecurityPath<TDomainObject> And(SecurityPath<TDomainObject> other) => new AndSecurityPath(this, other);

    public SecurityPath<TDomainObject> And<TSecurityContext>(
        Expression<Func<TDomainObject, TSecurityContext>> securityPath,
        SingleSecurityMode mode = SingleSecurityMode.AllowNull,
        string? key = null)
        where TSecurityContext : ISecurityContext =>
        this.And(Create(securityPath, mode, key));

    public SecurityPath<TDomainObject> And<TSecurityContext>(
        Expression<Func<TDomainObject, IEnumerable<TSecurityContext>>> securityPath,
        ManySecurityPathMode mode = ManySecurityPathMode.Any,
        string? key = null)
        where TSecurityContext : ISecurityContext =>
        this.And(Create(securityPath, mode, key));

    public SecurityPath<TDomainObject> Or(Expression<Func<TDomainObject, bool>> securityFilter) => this.Or(Condition(securityFilter));

    public SecurityPath<TDomainObject> Or(SecurityPath<TDomainObject> other) => new OrSecurityPath(this, other);

    public SecurityPath<TDomainObject> Or<TSecurityContext>(
        Expression<Func<TDomainObject, TSecurityContext>> securityPath,
        SingleSecurityMode mode = SingleSecurityMode.AllowNull,
        string? key = null)
        where TSecurityContext : ISecurityContext =>
        this.Or(Create(securityPath, mode, key));

    public SecurityPath<TDomainObject> Or<TSecurityContext>(
        Expression<Func<TDomainObject, IEnumerable<TSecurityContext>>> securityPath,
        ManySecurityPathMode mode = ManySecurityPathMode.Any,
        string? key = null)
        where TSecurityContext : ISecurityContext =>
        this.Or(Create(securityPath, mode, key));

    public static SecurityPath<TDomainObject> Condition(Expression<Func<TDomainObject, bool>> securityFilter) =>
        new ConditionPath(securityFilter);

    public static SecurityPath<TDomainObject> Create<TSecurityContext>(
        Expression<Func<TDomainObject, TSecurityContext>> securityPath,
        SingleSecurityMode mode = SingleSecurityMode.AllowNull,
        string? key = null)
        where TSecurityContext : ISecurityContext =>
        new SingleSecurityPath<TSecurityContext>(securityPath, mode, key);

    public static SecurityPath<TDomainObject> Create<TSecurityContext>(
        Expression<Func<TDomainObject, IEnumerable<TSecurityContext>>> securityPath,
        ManySecurityPathMode mode = ManySecurityPathMode.Any,
        string? key = null)
        where TSecurityContext : ISecurityContext =>
        new ManySecurityPath<TSecurityContext>(securityPath, mode, key);

    public static SecurityPath<TDomainObject> CreateNested<TNestedObject>(
        Expression<Func<TDomainObject, IEnumerable<TNestedObject>>> nestedObjectsPath,
        SecurityPath<TNestedObject> nestedSecurityPath,
        ManySecurityPathMode mode) =>
        new NestedManySecurityPath<TNestedObject>(nestedObjectsPath, nestedSecurityPath, mode);

    #endregion

    public record ConditionPath(Expression<Func<TDomainObject, bool>> SecurityFilter) : SecurityPath<TDomainObject>
    {
        protected override IEnumerable<Type> GetInternalUsedTypes() => [];

        public override SecurityPath<TNewDomainObject> OverrideInput<TNewDomainObject>(
            Expression<Func<TNewDomainObject, TDomainObject>> selector) =>
            new SecurityPath<TNewDomainObject>.ConditionPath(this.SecurityFilter.OverrideInput(selector));

        public virtual bool Equals(ConditionPath? other) =>
            ReferenceEquals(this, other)
            || (other is not null && ExpressionComparer.Value.Equals(this.SecurityFilter, other.SecurityFilter));

        public override int GetHashCode() => 0;
    }

    public abstract record BinarySecurityPath(SecurityPath<TDomainObject> Left, SecurityPath<TDomainObject> Right)
        : SecurityPath<TDomainObject>
    {
        protected override IEnumerable<Type> GetInternalUsedTypes() =>
            this.Left.GetUsedTypes().Concat(this.Right.GetUsedTypes());
    }

    public record OrSecurityPath(SecurityPath<TDomainObject> Left, SecurityPath<TDomainObject> Right) : BinarySecurityPath(Left, Right)
    {
        public override SecurityPath<TNewDomainObject> OverrideInput<TNewDomainObject>(
            Expression<Func<TNewDomainObject, TDomainObject>> selector) =>
            new SecurityPath<TNewDomainObject>.OrSecurityPath(this.Left.OverrideInput(selector), this.Right.OverrideInput(selector));
    }

    public record AndSecurityPath(SecurityPath<TDomainObject> Left, SecurityPath<TDomainObject> Right) : BinarySecurityPath(Left, Right)
    {
        public override SecurityPath<TNewDomainObject> OverrideInput<TNewDomainObject>(
            Expression<Func<TNewDomainObject, TDomainObject>> selector) =>
            new SecurityPath<TNewDomainObject>.AndSecurityPath(
                this.Left.OverrideInput(selector),
                this.Right.OverrideInput(selector));
    }

    public record SingleSecurityPath<TSecurityContext>(
        Expression<Func<TDomainObject, TSecurityContext>> SecurityPath,
        SingleSecurityMode Mode,
        string? Key) : SecurityPath<TDomainObject>, IContextSecurityPath
        where TSecurityContext : ISecurityContext
    {
        Type IContextSecurityPath.SecurityContextType => typeof(TSecurityContext);

        protected override IEnumerable<Type> GetInternalUsedTypes() => [typeof(TSecurityContext)];

        public override SecurityPath<TNewDomainObject> OverrideInput<TNewDomainObject>(
            Expression<Func<TNewDomainObject, TDomainObject>> selector) =>
            new SecurityPath<TNewDomainObject>.SingleSecurityPath<TSecurityContext>(
                this.SecurityPath.OverrideInput(selector),
                this.Mode,
                this.Key);

        public virtual bool Equals(SingleSecurityPath<TSecurityContext>? other) =>
            ReferenceEquals(this, other)
            || (other is not null
                && this.Mode == other.Mode
                && this.Key == other.Key
                && ExpressionComparer.Value.Equals(this.SecurityPath, other.SecurityPath));

        public override int GetHashCode() => this.Mode.GetHashCode();
    }

    public record ManySecurityPath<TSecurityContext>(
        Expression<Func<TDomainObject, IEnumerable<TSecurityContext>>> SecurityPath,
        ManySecurityPathMode Mode,
        string? Key) : SecurityPath<TDomainObject>, IContextSecurityPath
        where TSecurityContext : ISecurityContext
    {
        Type IContextSecurityPath.SecurityContextType => typeof(TSecurityContext);

        public Expression<Func<TDomainObject, IQueryable<TSecurityContext>>>? SecurityPathQ { get; } =
            TryExtractSecurityPathQ(SecurityPath);

        protected override IEnumerable<Type> GetInternalUsedTypes() => [typeof(TSecurityContext)];

        private static Expression<Func<TDomainObject, IQueryable<TSecurityContext>>>? TryExtractSecurityPathQ(
            Expression<Func<TDomainObject, IEnumerable<TSecurityContext>>> securityPath)
        {
            if (securityPath.Body.Type == typeof(IQueryable<TSecurityContext>))
            {
                return Expression.Lambda<Func<TDomainObject, IQueryable<TSecurityContext>>>(securityPath.Body, securityPath.Parameters);
            }

            return null;
        }

        public override SecurityPath<TNewDomainObject> OverrideInput<TNewDomainObject>(
            Expression<Func<TNewDomainObject, TDomainObject>> selector) =>
            new SecurityPath<TNewDomainObject>.ManySecurityPath<TSecurityContext>(
                this.SecurityPath.OverrideInput(selector),
                this.Mode,
                this.Key);

        public virtual bool Equals(ManySecurityPath<TSecurityContext>? other) =>
            ReferenceEquals(this, other)
            || (other is not null
                && this.Mode == other.Mode
                && this.Key == other.Key
                && ExpressionComparer.Value.Equals(this.SecurityPath, other.SecurityPath));

        public override int GetHashCode() => this.Mode.GetHashCode();
    }

    public record NestedManySecurityPath<TNestedObject>(
        Expression<Func<TDomainObject, IEnumerable<TNestedObject>>> NestedObjectsPath,
        SecurityPath<TNestedObject> NestedSecurityPath,
        ManySecurityPathMode Mode) : SecurityPath<TDomainObject>
    {
        protected override IEnumerable<Type> GetInternalUsedTypes() => this.NestedSecurityPath.GetUsedTypes();

        public override SecurityPath<TNewDomainObject> OverrideInput<TNewDomainObject>(
            Expression<Func<TNewDomainObject, TDomainObject>> selector) =>
            new SecurityPath<TNewDomainObject>.NestedManySecurityPath<TNestedObject>(
                this.NestedObjectsPath.OverrideInput(selector),
                this.NestedSecurityPath,
                this.Mode);

        public virtual bool Equals(NestedManySecurityPath<TNestedObject>? other) =>
            ReferenceEquals(this, other)
            || (other is not null
                && this.Mode == other.Mode
                && this.NestedSecurityPath == other.NestedSecurityPath
                && ExpressionComparer.Value.Equals(this.NestedObjectsPath, other.NestedObjectsPath));

        public override int GetHashCode() => this.Mode.GetHashCode();
    }
}
