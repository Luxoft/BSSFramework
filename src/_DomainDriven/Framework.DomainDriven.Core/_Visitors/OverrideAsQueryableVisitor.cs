using System.Linq.Expressions;

using Framework.Core;

namespace Framework.DomainDriven.BLL;
//public class OverrideAsQueryableConstVisitor : ExpressionVisitor
//{
//    private OverrideAsQueryableConstVisitor()
//    {

//    }

//    public override Expression Visit(Expression node)
//    {
//        return new InternalStateVisitor().Visit(node);
//    }

//    public class InternalStateVisitor : ExpressionVisitor
//    {
//        //private static readonly MethodInfo AsQueryableMethod = new Func<IEnumerable<object>, IQueryable<object>>(Queryable.AsQueryable)
//        //                                                      .Method
//        //                                                      .GetGenericMethodDefinition();

//        private readonly IDictionaryCache<IQueryable, Expression> _constCache = new DictionaryCache<IQueryable, Expression>(source =>
//            (Expression)new Func<IQueryable<object>, Expression>(ExtractConstList<object>).CreateGenericMethod(source.ElementType)
//                                                                                          .Invoke(null, new object[] { source })
//        );

//        private static Expression ExtractConstList<T>(IQueryable<T> baseSource)
//        {
//            return Expression.Constant(baseSource.ToList());
//        }

//        protected override Expression VisitMethodCall(MethodCallExpression node)
//        {
//            if (node == null) throw new ArgumentNullException("node");

//            var request = from _ in Maybe.Return()

//                          let method = node.Method

//                          where node.Object == null && method.DeclaringType == typeof(Queryable)

//                          from newSource in this.ExtractConstList(node.Arguments[0])

//                          let elementType = method.GetGenericArguments().First()

//                          let args = new[] { typeof(IEnumerable<>).MakeGenericType(elementType) }
//                                        .Concat(method.GetParameters().Skip(1).ToArray(p => p.ParameterType))
//                                        .ToArray()

//                          select (Expression)Expression.Call(typeof(Enumerable), method.Name, method.GetGenericArguments(), new[] { newSource }.Concat(node.Arguments.Skip(1)).ToArray());

//            return request.GetValueOrDefault(() => base.VisitMethodCall(node));
//        }

//        private Maybe<Expression> ExtractConstList(Expression baseNode)
//        {
//            return from const1 in baseNode.GetMemberConstValue<IQueryable>()

//                   from const2 in const1.Expression.GetMemberConstValue<IQueryable>()

//                   where const1 == const2 // is AsQueryable

//                   select this._constCache[const1];
//        }
//    }

//    public static readonly OverrideAsQueryableConstVisitor Value = new OverrideAsQueryableConstVisitor();
//}

public class RestoreQueryableCallsVisitor : ExpressionVisitor
{
    private RestoreQueryableCallsVisitor()
    {

    }


    protected override Expression VisitMethodCall(MethodCallExpression node)
    {
        var request = from _ in Maybe.Return()

                      let method = node.Method

                      where node.Object == null && method.DeclaringType == typeof(Enumerable)

                      from newSource in node.Arguments[0].GetDeepMemberConstValue<IQueryable>()

                      let tail = node.Arguments.Skip(1).ToArray()

                      let newArgs = new[] { Expression.Constant(newSource) }.Concat(tail).ToArray(this.Visit)

                      let newExpr = Expression.Call(typeof(Queryable), method.Name, method.GetGenericArguments(), newArgs)

                      select newExpr;


        return request.GetValueOrDefault(() => base.VisitMethodCall(node));
    }


    public static readonly RestoreQueryableCallsVisitor Value = new RestoreQueryableCallsVisitor();
}
