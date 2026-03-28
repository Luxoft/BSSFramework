//using System.Collections.Concurrent;
//using System.Linq.Expressions;
//using System.Reflection;

//using CommonFramework;
//using CommonFramework.Maybe;

//using Framework.Application.Domain;

//using ExpressionHelper = CommonFramework.ExpressionHelper;

//namespace Framework.Database._Visitors;

//public class OverrideListContainsVisitor<TIdent>(PropertyInfo idProperty) : ExpressionVisitor
//{
//    private static readonly MethodInfo VisitListCallMethod =
//        new Func<MethodCallExpression, List<IIdentityObject<TIdent>>, Maybe<Expression>>(VisitListCall).Method.GetGenericMethodDefinition();

//    private static readonly ConcurrentDictionary<PropertyInfo, OverrideListContainsVisitor<TIdent>> Cache = [];

//    /// <summary> Returns <see cref="OverrideListContainsVisitor{TIdent}"/> for specified <paramref name="property"/>
//    /// </summary>
//    /// <param name="property">Property to get ExpressionVisitor for</param>
//    /// <returns>Expression Visitor</returns>
//    public static OverrideListContainsVisitor<TIdent> GetOrCreate(PropertyInfo property) => Cache.GetOrAdd(property, pInfo => new OverrideListContainsVisitor<TIdent>(pInfo));

//    protected override Expression VisitMethodCall(MethodCallExpression node)
//    {
//        var idPropertyDeclaringType = idProperty.DeclaringType;

//        var request = from obj in node.Object.ToMaybe()

//                      from value in obj.GetDeepMemberConstValue()

//                      let valueType = value.GetType()

//                      where valueType.IsGenericType

//                      let genericValueType = valueType.GetGenericTypeDefinition()

//                      where genericValueType == typeof(List<>)

//                      let genericArgType = valueType.GetGenericArguments().Single()

//                      where idPropertyDeclaringType != null && idPropertyDeclaringType.IsAssignableFrom(genericArgType)

//                      from result in VisitListCallMethod.MakeGenericMethod(genericArgType).Invoke<Maybe<Expression>>(null, [node, value])

//                      select result;

//        return request.GetValueOrDefault(() => base.VisitMethodCall(node));
//    }

//    private static Maybe<Expression> VisitListCall<TDomainObject>(MethodCallExpression node, List<TDomainObject> list)
//        where TDomainObject : class, IIdentityObject<TIdent> =>

//        from _ in Maybe.Return()

//        where new Func<TDomainObject, bool>(list.Contains).Method == node.Method

//        let arg = node.Arguments.Single()

//        let argIdExpr = ExpressionHelper.Create((TDomainObject domainObject) => domainObject.Id)

//        let prop = (PropertyInfo)((MemberExpression)argIdExpr.Body).Member

//        let idents = list.Where(v => v != null).Select(v => v.Id).ToList()

//        let identListContainsMethod = new Func<TIdent, bool>(idents.Contains).Method

//        select (Expression)Expression.Call(Expression.Constant(idents), identListContainsMethod, Expression.Property(arg, prop));
//}
