using System.Linq.Expressions;

using Framework.Core;

namespace Framework.SecuritySystem;

/// <summary>
/// Контекстное правило доступа (лямбды)
/// </summary>
/// <typeparam name="TDomainObject"></typeparam>
public abstract class SecurityPath<TDomainObject>
{
    protected SecurityPath()
    {
    }

    public abstract SecurityPath<TNewDomainObject> OverrideInput<TNewDomainObject>(Expression<Func<TNewDomainObject, TDomainObject>> selector)
        where TNewDomainObject : class;


    public IEnumerable<Type> GetUsedTypes()
    {
        return this.GetInternalUsedTypes().Distinct();
    }

    protected internal abstract IEnumerable<Type> GetInternalUsedTypes();

    #region Create


    public SecurityPath<TDomainObject> And<TSecurityContext>(Expression<Func<TDomainObject, IEnumerable<TSecurityContext>>> securityPath)
            where TSecurityContext : class, ISecurityContext
    {
        return this.And(securityPath, ManySecurityPathMode.Any);
    }

    public SecurityPath<TDomainObject> And<TSecurityContext>(Expression<Func<TDomainObject, IEnumerable<TSecurityContext>>> securityPath, ManySecurityPathMode mode)
            where TSecurityContext : class, ISecurityContext
    {
        return this.And(Create(securityPath, mode));
    }

    public SecurityPath<TDomainObject> Or<TSecurityContext>(Expression<Func<TDomainObject, TSecurityContext>> securityPath)
            where TSecurityContext : class, ISecurityContext
    {
        return this.Or(Create(securityPath));
    }

    public SecurityPath<TDomainObject> Or<TSecurityContext>(Expression<Func<TDomainObject, IEnumerable<TSecurityContext>>> securityPath)
            where TSecurityContext : class, ISecurityContext
    {
        return this.Or(securityPath, ManySecurityPathMode.Any);
    }

    public SecurityPath<TDomainObject> Or<TSecurityContext>(Expression<Func<TDomainObject, IEnumerable<TSecurityContext>>> securityPath, ManySecurityPathMode mode)
            where TSecurityContext : class, ISecurityContext
    {
        return this.Or(Create(securityPath, mode));
    }


    public SecurityPath<TDomainObject> Or(SecurityPath<TDomainObject> other)
    {
        if (other == null) throw new ArgumentNullException(nameof(other));

        return new OrSecurityPath(this, other);
    }

    public SecurityPath<TDomainObject> And<TSecurityContext>(Expression<Func<TDomainObject, TSecurityContext>> securityPath, SingleSecurityMode mode = SingleSecurityMode.AllowNull)
            where TSecurityContext : class, ISecurityContext
    {
        return this.And(Create(securityPath, mode));
    }

    public SecurityPath<TDomainObject> And(SecurityPath<TDomainObject> other)
    {
        if (other == null) throw new ArgumentNullException(nameof(other));

        return new AndSecurityPath(this, other);
    }

    public SecurityPath<TDomainObject> And(Expression<Func<TDomainObject, bool>> securityFilter)
    {
        if (securityFilter == null) throw new ArgumentNullException(nameof(securityFilter));

        var condPath = new ConditionPath(securityFilter);

        return new AndSecurityPath(this, condPath);
    }

    public SecurityPath<TDomainObject> Or(Expression<Func<TDomainObject, bool>> securityFilter)
    {
        if (securityFilter == null) throw new ArgumentNullException(nameof(securityFilter));

        var condPath = new ConditionPath(securityFilter);

        return new OrSecurityPath(this, condPath);
    }

    public static SecurityPath<TDomainObject> Create<TSecurityContext>(Expression<Func<TDomainObject, TSecurityContext>> securityPath, SingleSecurityMode mode = SingleSecurityMode.AllowNull)
            where TSecurityContext : class, ISecurityContext
    {
        return new SingleSecurityPath<TSecurityContext>(securityPath, mode);
    }

    public static SecurityPath<TDomainObject> Create<TSecurityContext>(Expression<Func<TDomainObject, IEnumerable<TSecurityContext>>> securityPath)
            where TSecurityContext : class, ISecurityContext
    {
        return Create(securityPath, ManySecurityPathMode.Any);
    }

    public static SecurityPath<TDomainObject> Create<TSecurityContext>(Expression<Func<TDomainObject, IEnumerable<TSecurityContext>>> securityPath, ManySecurityPathMode mode)
            where TSecurityContext : class, ISecurityContext
    {
        return new ManySecurityPath<TSecurityContext>(securityPath, mode);
    }

    public static SecurityPath<TDomainObject> Condition(Expression<Func<TDomainObject, bool>> securityFilter)
    {
        return new ConditionPath(securityFilter);
    }


    #endregion


    public class ConditionPath : SecurityPath<TDomainObject>
    {
        public readonly Expression<Func<TDomainObject, bool>> SecurityFilter;


        internal ConditionPath(Expression<Func<TDomainObject, bool>> securityFilter)
        {
            this.SecurityFilter = securityFilter ?? throw new ArgumentNullException(nameof(securityFilter));
        }


        protected internal override IEnumerable<Type> GetInternalUsedTypes()
        {
            yield break;
        }

        public override SecurityPath<TNewDomainObject> OverrideInput<TNewDomainObject>(Expression<Func<TNewDomainObject, TDomainObject>> selector)
        {
            return new SecurityPath<TNewDomainObject>.ConditionPath(this.SecurityFilter.OverrideInput(selector));
        }
    }

    public abstract class BinarySecurityPath : SecurityPath<TDomainObject>
    {
        public readonly SecurityPath<TDomainObject> Left;

        public readonly SecurityPath<TDomainObject> Right;


        protected BinarySecurityPath(SecurityPath<TDomainObject> left, SecurityPath<TDomainObject> right)
        {
            this.Left = left ?? throw new ArgumentNullException(nameof(left));
            this.Right = right ?? throw new ArgumentNullException(nameof(right));
        }

        protected internal override IEnumerable<Type> GetInternalUsedTypes()
        {
            return this.Left.GetInternalUsedTypes().Concat(this.Right.GetInternalUsedTypes());
        }
    }

    public class OrSecurityPath : BinarySecurityPath
    {
        internal OrSecurityPath(SecurityPath<TDomainObject> path, SecurityPath<TDomainObject> otherPath)
                : base(path, otherPath)
        {
        }

        public override SecurityPath<TNewDomainObject> OverrideInput<TNewDomainObject>(Expression<Func<TNewDomainObject, TDomainObject>> selector)
        {
            return new SecurityPath<TNewDomainObject>.OrSecurityPath(this.Left.OverrideInput(selector), this.Right.OverrideInput(selector));
        }
    }

    public class AndSecurityPath : BinarySecurityPath
    {
        internal AndSecurityPath(SecurityPath<TDomainObject> path, SecurityPath<TDomainObject> otherPath)
                : base(path, otherPath)
        {
        }

        public override SecurityPath<TNewDomainObject> OverrideInput<TNewDomainObject>(Expression<Func<TNewDomainObject, TDomainObject>> selector)
        {
            return new SecurityPath<TNewDomainObject>.AndSecurityPath(this.Left.OverrideInput(selector), this.Right.OverrideInput(selector));
        }
    }


    public abstract class FilterSecurityPath<TSecurityContext> : SecurityPath<TDomainObject>
    {
        protected internal override IEnumerable<Type> GetInternalUsedTypes()
        {
            yield return typeof(TSecurityContext);
        }
    }


    public class SingleSecurityPath<TSecurityContext> : FilterSecurityPath<TSecurityContext>
            where TSecurityContext : class, ISecurityContext
    {
        public readonly Expression<Func<TDomainObject, TSecurityContext>> SecurityPath;

        public readonly SingleSecurityMode Mode;


        internal SingleSecurityPath(Expression<Func<TDomainObject, TSecurityContext>> securityPath, SingleSecurityMode mode)
        {
            this.SecurityPath = securityPath ?? throw new ArgumentNullException(nameof(securityPath));

            this.Mode = mode;
        }


        public override SecurityPath<TNewDomainObject> OverrideInput<TNewDomainObject>(Expression<Func<TNewDomainObject, TDomainObject>> selector)
        {
            return new SecurityPath<TNewDomainObject>.SingleSecurityPath<TSecurityContext>(this.SecurityPath.OverrideInput(selector), this.Mode);
        }
    }

    public class ManySecurityPath<TSecurityContext> : FilterSecurityPath<TSecurityContext>
            where TSecurityContext : class, ISecurityContext
    {
        public readonly Expression<Func<TDomainObject, IEnumerable<TSecurityContext>>> SecurityPath;

        public readonly Expression<Func<TDomainObject, IQueryable<TSecurityContext>>> SecurityPathQ;

        public readonly ManySecurityPathMode Mode;


        internal ManySecurityPath(Expression<Func<TDomainObject, IEnumerable<TSecurityContext>>> securityPath, ManySecurityPathMode mode)
        {
            this.SecurityPath = securityPath ?? throw new ArgumentNullException(nameof(securityPath));
            this.Mode = mode;

            this.SecurityPathQ = this.TryExtractSecurityPathQ();
        }

        private Expression<Func<TDomainObject, IQueryable<TSecurityContext>>> TryExtractSecurityPathQ()
        {
            if (this.SecurityPath.Body.Type == typeof(IQueryable<TSecurityContext>))
            {
                return Expression.Lambda<Func<TDomainObject, IQueryable<TSecurityContext>>>(this.SecurityPath.Body, this.SecurityPath.Parameters);
            }

            return null;
        }


        public override SecurityPath<TNewDomainObject> OverrideInput<TNewDomainObject>(Expression<Func<TNewDomainObject, TDomainObject>> selector)
        {
            return new SecurityPath<TNewDomainObject>.ManySecurityPath<TSecurityContext>(this.SecurityPath.OverrideInput(selector), this.Mode);
        }
    }

    public class NestedManySecurityPath<TNestedObject> : SecurityPath<TDomainObject>
        where TNestedObject : class
    {
        public readonly Expression<Func<TDomainObject, IEnumerable<TNestedObject>>> NestedObjectsPath;

        public readonly SecurityPath<TNestedObject> NestedSecurityPath;

        public readonly ManySecurityPathMode Mode;


        internal NestedManySecurityPath(
                Expression<Func<TDomainObject, IEnumerable<TNestedObject>>> nestedObjectsPath,
                SecurityPath<TNestedObject> nestedSecurityPath,
                ManySecurityPathMode mode)
        {
            this.NestedObjectsPath = nestedObjectsPath ?? throw new ArgumentNullException(nameof(nestedObjectsPath));
            this.NestedSecurityPath = nestedSecurityPath ?? throw new ArgumentNullException(nameof(nestedSecurityPath));
            this.Mode = mode;
        }

        protected internal override IEnumerable<Type> GetInternalUsedTypes()
        {
            return this.NestedSecurityPath.GetInternalUsedTypes();
        }

        public override SecurityPath<TNewDomainObject> OverrideInput<TNewDomainObject>(Expression<Func<TNewDomainObject, TDomainObject>> selector)
        {
            return new SecurityPath<TNewDomainObject>.NestedManySecurityPath<TNestedObject>(this.NestedObjectsPath.OverrideInput(selector), this.NestedSecurityPath, this.Mode);
        }
    }
}
