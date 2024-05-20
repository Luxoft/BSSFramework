using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

using Framework.Core;
using Framework.Persistent;

namespace Framework.DomainDriven._Visitors;

public class OverrideListContainsVisitor<TIdent> : ExpressionVisitor
{
    private static readonly MethodInfo VisitListCallMethod =
            new Func<MethodCallExpression, List<IIdentityObject<TIdent>>, Maybe<Expression>>(VisitListCall).Method.GetGenericMethodDefinition();

    private static readonly ConcurrentDictionary<PropertyInfo, OverrideListContainsVisitor<TIdent>> Cache = new ConcurrentDictionary<PropertyInfo, OverrideListContainsVisitor<TIdent>>();

    private readonly PropertyInfo idProperty;

    private OverrideListContainsVisitor(PropertyInfo idProperty)
    {
        if (idProperty == null)
        {
            throw new ArgumentNullException(nameof(idProperty));
        }

        this.idProperty = idProperty;
    }

    /// <summary> Returns <see cref="OverrideListContainsVisitor{TIdent}"/> for specified <paramref name="property"/>
    /// </summary>
    /// <param name="property">Property to get ExpressionVisitor for</param>
    /// <returns>Expression Visitor</returns>
    public static OverrideListContainsVisitor<TIdent> GetOrCreate(PropertyInfo property)
    {
        return Cache.GetOrAdd(property, pInfo => new OverrideListContainsVisitor<TIdent>(pInfo));
    }

    protected override Expression VisitMethodCall(MethodCallExpression node)
    {
        var idPropertyDeclaringType = this.idProperty.DeclaringType;

        // TODO gtsaplin: it's too complicated code, refactor
        var request = from obj in node.Object.ToMaybe()

                      from value in obj.GetDeepMemberConstValue()

                      let valueType = value.GetType()

                      where valueType.IsGenericType

                      let genericValueType = valueType.GetGenericTypeDefinition()

                      where genericValueType == typeof(List<>)

                      let genericArgType = valueType.GetGenericArguments().Single()

                      where idPropertyDeclaringType != null && idPropertyDeclaringType.IsAssignableFrom(genericArgType)

                      from result in (Maybe<Expression>)VisitListCallMethod.MakeGenericMethod(genericArgType).Invoke(null, new[] { node, value })

                      select result;

        return request.GetValueOrDefault(() => base.VisitMethodCall(node));
    }

    private static Maybe<Expression> VisitListCall<TDomainObject>(MethodCallExpression node, List<TDomainObject> preList)
            where TDomainObject : class, IIdentityObject<TIdent>
    {
        // TODO gtsaplin: it's too complicated code, refactor
        return from list in Maybe.Return(preList)
               where new Func<TDomainObject, bool>(list.Contains).Method == node.Method
               let arg = node.Arguments.Single()
               let argIdExpr = ExpressionHelper.Create((TDomainObject domainObject) => domainObject.Id)
               let prop = (PropertyInfo)((MemberExpression)argIdExpr.Body).Member
               let idents = list.Where(v => v != null).ToList(v => v.Id)
               let identListContainsMethod = new Func<TIdent, bool>(idents.Contains).Method
               select (Expression)Expression.Call(Expression.Constant(idents), identListContainsMethod, new Expression[]
                                                      {
                                                              Expression.Property(arg, prop)
                                                      });
    }
}
