using System.Linq.Expressions;
using System.Reflection;

using JetBrains.Annotations;

namespace Framework.DomainDriven.DBGenerator;

public class IgnoreLink
{
    public static IgnoreLink CreateFromType<T>()
    {
        return new IgnoreLink(typeof(T));

    }
    public static IgnoreLink Create<T, TProperty>(Expression<Func<T, TProperty>> propertyExpression)
    {
        if (!(propertyExpression.Body is MemberExpression))
        {
            throw new ArgumentException("Expected PropertyExpression, actual expression:{0}", propertyExpression.ToString());
        }

        var memberExpression = (MemberExpression)propertyExpression.Body;

        var member = memberExpression.Member;

        return new IgnoreLink((PropertyInfo)member);

    }
    public static IgnoreLink Create<T>(Expression<Func<T, object>> collectionExpression)
    {
        return IgnoreLink.Create<T, object>(collectionExpression);
    }
    public static IgnoreLink CreateMany<T>(Expression<Func<T, IEnumerable<object>>> collectionExpression)
    {
        if (!(collectionExpression.Body is MemberExpression))
        {
            throw new ArgumentException("Expected PropertyExpression, actual expression:{0}", collectionExpression.ToString());
        }

        var memberExpression = (MemberExpression) collectionExpression.Body;

        var member = memberExpression.Member;

        return new IgnoreLink((PropertyInfo)member);
    }

    private IgnoreLink([NotNull] PropertyInfo propertyInfo) : this(propertyInfo.DeclaringType)
    {
        if (propertyInfo == null) throw new ArgumentNullException(nameof(propertyInfo));

        this.MemberInfo = propertyInfo;
    }

    private IgnoreLink(Type fromType)
    {
        this.FromType = fromType;
    }

    public Type FromType { get; private set;}

    public PropertyInfo MemberInfo { get; private set; }
}
