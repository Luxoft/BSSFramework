using System.Linq.Expressions;

using Framework.Core;
using Framework.Persistent;


namespace Framework.SecuritySystem;

/// <summary>
/// Контекстное правило доступа (лямбды)
/// </summary>
/// <typeparam name="TPersistentDomainObjectBase"></typeparam>
/// <typeparam name="TDomainObject"></typeparam>
/// <typeparam name="TIdent"></typeparam>
public abstract class SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent> : SecurityPathBase<TPersistentDomainObjectBase, TDomainObject, TIdent>
        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
        where TDomainObject : class, TPersistentDomainObjectBase
{
    protected SecurityPath()
    {
    }

    public abstract SecurityPath<TPersistentDomainObjectBase, TNewDomainObject, TIdent> OverrideInput<TNewDomainObject>(Expression<Func<TNewDomainObject, TDomainObject>> selector)
            where TNewDomainObject : class, TPersistentDomainObjectBase;


    #region Create

    public static SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent> Create<TSecurityContext>(
            Func<IEnumerable<TIdent>, Expression<Func<TDomainObject, bool>>> securityFilter)
            where TSecurityContext : class, TPersistentDomainObjectBase, ISecurityContext
    {
        return new SecurityPathByIdents<TSecurityContext>(securityFilter);
    }


    public SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent> And<TSecurityContext>(Expression<Func<TDomainObject, IEnumerable<TSecurityContext>>> securityPath)
            where TSecurityContext : class, TPersistentDomainObjectBase, ISecurityContext
    {
        return this.And(securityPath, ManySecurityPathMode.Any);
    }

    public SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent> And<TSecurityContext>(Expression<Func<TDomainObject, IEnumerable<TSecurityContext>>> securityPath, ManySecurityPathMode mode)
            where TSecurityContext : class, TPersistentDomainObjectBase, ISecurityContext
    {
        return this.And(Create(securityPath, mode));
    }

    public SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent> And<TSecurityContext>(Func<IEnumerable<TIdent>, Expression<Func<TDomainObject, bool>>> otherSecurityFilter)
            where TSecurityContext : class, TPersistentDomainObjectBase, ISecurityContext
    {
        if (otherSecurityFilter == null) throw new ArgumentNullException(nameof(otherSecurityFilter));

        return new AndSecurityPath(this, Create<TSecurityContext>(otherSecurityFilter));
    }



    public SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent> Or<TSecurityContext>(
            Func<IEnumerable<TIdent>, Expression<Func<TDomainObject, bool>>> otherSecurityFilter)
            where TSecurityContext : class, TPersistentDomainObjectBase, ISecurityContext
    {
        if (otherSecurityFilter == null) throw new ArgumentNullException(nameof(otherSecurityFilter));

        return new OrSecurityPath(this, Create<TSecurityContext>(otherSecurityFilter));
    }


    public SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent> Or<TSecurityContext>(Expression<Func<TDomainObject, TSecurityContext>> securityPath)
            where TSecurityContext : class, TPersistentDomainObjectBase, ISecurityContext
    {
        return this.Or(Create(securityPath));
    }

    public SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent> Or<TSecurityContext>(Expression<Func<TDomainObject, IEnumerable<TSecurityContext>>> securityPath)
            where TSecurityContext : class, TPersistentDomainObjectBase, ISecurityContext
    {
        return this.Or(securityPath, ManySecurityPathMode.Any);
    }

    public SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent> Or<TSecurityContext>(Expression<Func<TDomainObject, IEnumerable<TSecurityContext>>> securityPath, ManySecurityPathMode mode)
            where TSecurityContext : class, TPersistentDomainObjectBase, ISecurityContext
    {
        return this.Or(Create(securityPath, mode));
    }


    public SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent> Or(SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent> other)
    {
        if (other == null) throw new ArgumentNullException(nameof(other));

        return new OrSecurityPath(this, other);
    }

    public SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent> And<TSecurityContext>(Expression<Func<TDomainObject, TSecurityContext>> securityPath, SingleSecurityMode mode = SingleSecurityMode.AllowNull)
            where TSecurityContext : class, TPersistentDomainObjectBase, ISecurityContext
    {
        return this.And(Create(securityPath, mode));
    }

    public SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent> And(SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent> other)
    {
        if (other == null) throw new ArgumentNullException(nameof(other));

        return new AndSecurityPath(this, other);
    }

    public SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent> And(Expression<Func<TDomainObject, bool>> securityFilter)
    {
        if (securityFilter == null) throw new ArgumentNullException(nameof(securityFilter));

        var condPath = new ConditionPath(securityFilter);

        return new AndSecurityPath(this, condPath);
    }

    public SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent> Or(Expression<Func<TDomainObject, bool>> securityFilter)
    {
        if (securityFilter == null) throw new ArgumentNullException(nameof(securityFilter));

        var condPath = new ConditionPath(securityFilter);

        return new OrSecurityPath(this, condPath);
    }

    public static SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent> Create<TSecurityContext>(Expression<Func<TDomainObject, TSecurityContext>> securityPath, SingleSecurityMode mode = SingleSecurityMode.AllowNull)
            where TSecurityContext : class, TPersistentDomainObjectBase, ISecurityContext
    {
        return new SingleSecurityPath<TSecurityContext>(securityPath, mode);
    }

    public static SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent> Create<TSecurityContext>(Expression<Func<TDomainObject, IEnumerable<TSecurityContext>>> securityPath)
            where TSecurityContext : class, TPersistentDomainObjectBase, ISecurityContext
    {
        return Create(securityPath, ManySecurityPathMode.Any);
    }

    public static SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent> Create<TSecurityContext>(Expression<Func<TDomainObject, IEnumerable<TSecurityContext>>> securityPath, ManySecurityPathMode mode)
            where TSecurityContext : class, TPersistentDomainObjectBase, ISecurityContext
    {
        return new ManySecurityPath<TSecurityContext>(securityPath, mode);
    }

    public static SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent> Condition(Expression<Func<TDomainObject, bool>> securityFilter)
    {
        return new ConditionPath(securityFilter);
    }


    #endregion


    public class ConditionPath : SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent>
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

        public override SecurityPath<TPersistentDomainObjectBase, TNewDomainObject, TIdent> OverrideInput<TNewDomainObject>(Expression<Func<TNewDomainObject, TDomainObject>> selector)
        {
            return new SecurityPath<TPersistentDomainObjectBase, TNewDomainObject, TIdent>.ConditionPath(this.SecurityFilter.OverrideInput(selector));
        }
    }

    public abstract class BinarySecurityPath : SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent>
    {
        public readonly SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent> Left;

        public readonly SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent> Right;


        protected BinarySecurityPath(SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent> left, SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent> right)
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
        internal OrSecurityPath(SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent> path, SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent> otherPath)
                : base(path, otherPath)
        {
        }

        public override SecurityPath<TPersistentDomainObjectBase, TNewDomainObject, TIdent> OverrideInput<TNewDomainObject>(Expression<Func<TNewDomainObject, TDomainObject>> selector)
        {
            return new SecurityPath<TPersistentDomainObjectBase, TNewDomainObject, TIdent>.OrSecurityPath(this.Left.OverrideInput(selector), this.Right.OverrideInput(selector));
        }
    }

    public class AndSecurityPath : BinarySecurityPath
    {
        internal AndSecurityPath(SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent> path, SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent> otherPath)
                : base(path, otherPath)
        {
        }

        public override SecurityPath<TPersistentDomainObjectBase, TNewDomainObject, TIdent> OverrideInput<TNewDomainObject>(Expression<Func<TNewDomainObject, TDomainObject>> selector)
        {
            return new SecurityPath<TPersistentDomainObjectBase, TNewDomainObject, TIdent>.AndSecurityPath(this.Left.OverrideInput(selector), this.Right.OverrideInput(selector));
        }
    }


    public abstract class FilterSecurityPath<TSecurityContext> : SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent>
    {
        protected internal override IEnumerable<Type> GetInternalUsedTypes()
        {
            yield return typeof(TSecurityContext);
        }
    }


    public class SecurityPathByIdents<TSecurityContext> : FilterSecurityPath<TSecurityContext>
            where TSecurityContext : class, TPersistentDomainObjectBase, ISecurityContext
    {
        public readonly Func<IEnumerable<TIdent>, Expression<Func<TDomainObject, bool>>> SecurityFilter;


        internal SecurityPathByIdents(Func<IEnumerable<TIdent>, Expression<Func<TDomainObject, bool>>> securityFilter)
        {
            this.SecurityFilter = securityFilter ?? throw new ArgumentNullException(nameof(securityFilter));
        }

        public override SecurityPath<TPersistentDomainObjectBase, TNewDomainObject, TIdent> OverrideInput<TNewDomainObject>(Expression<Func<TNewDomainObject, TDomainObject>> selector)
        {
            return new SecurityPath<TPersistentDomainObjectBase, TNewDomainObject, TIdent>.SecurityPathByIdents<TSecurityContext>(idents => this.SecurityFilter(idents).OverrideInput(selector));
        }
    }


    public class SingleSecurityPath<TSecurityContext> : FilterSecurityPath<TSecurityContext>
            where TSecurityContext : class, TPersistentDomainObjectBase, ISecurityContext
    {
        public readonly Expression<Func<TDomainObject, TSecurityContext>> SecurityPath;

        public readonly SingleSecurityMode Mode;


        internal SingleSecurityPath(Expression<Func<TDomainObject, TSecurityContext>> securityPath, SingleSecurityMode mode)
        {
            this.SecurityPath = securityPath ?? throw new ArgumentNullException(nameof(securityPath));

            this.Mode = mode;
        }


        public override SecurityPath<TPersistentDomainObjectBase, TNewDomainObject, TIdent> OverrideInput<TNewDomainObject>(Expression<Func<TNewDomainObject, TDomainObject>> selector)
        {
            return new SecurityPath<TPersistentDomainObjectBase, TNewDomainObject, TIdent>.SingleSecurityPath<TSecurityContext>(this.SecurityPath.OverrideInput(selector), this.Mode);
        }
    }

    public class ManySecurityPath<TSecurityContext> : FilterSecurityPath<TSecurityContext>
            where TSecurityContext : class, TPersistentDomainObjectBase, ISecurityContext
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


        public override SecurityPath<TPersistentDomainObjectBase, TNewDomainObject, TIdent> OverrideInput<TNewDomainObject>(Expression<Func<TNewDomainObject, TDomainObject>> selector)
        {
            return new SecurityPath<TPersistentDomainObjectBase, TNewDomainObject, TIdent>.ManySecurityPath<TSecurityContext>(this.SecurityPath.OverrideInput(selector), this.Mode);
        }
    }

    public class NestedManySecurityPath<TNestedObject> : SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent>
            where TNestedObject : class, TPersistentDomainObjectBase
    {
        public readonly Expression<Func<TDomainObject, IEnumerable<TNestedObject>>> NestedObjectsPath;

        public readonly SecurityPath<TPersistentDomainObjectBase, TNestedObject, TIdent> NestedSecurityPath;

        public readonly ManySecurityPathMode Mode;


        internal NestedManySecurityPath(
                Expression<Func<TDomainObject, IEnumerable<TNestedObject>>> nestedObjectsPath,
                SecurityPath<TPersistentDomainObjectBase, TNestedObject, TIdent> nestedSecurityPath,
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

        public override SecurityPath<TPersistentDomainObjectBase, TNewDomainObject, TIdent> OverrideInput<TNewDomainObject>(Expression<Func<TNewDomainObject, TDomainObject>> selector)
        {
            return new SecurityPath<TPersistentDomainObjectBase, TNewDomainObject, TIdent>.NestedManySecurityPath<TNestedObject>(this.NestedObjectsPath.OverrideInput(selector), this.NestedSecurityPath, this.Mode);
        }
    }
}
