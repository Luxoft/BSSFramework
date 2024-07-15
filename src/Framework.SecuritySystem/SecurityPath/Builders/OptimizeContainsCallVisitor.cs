using System.Linq.Expressions;
using System.Reflection;

using Framework.Core;

namespace Framework.SecuritySystem.Rules.Builders;

internal class OptimizeContainsCallVisitor<TIdent> : ExpressionVisitor
{
    private OptimizeContainsCallVisitor()
    {

    }


    public override Expression Visit(Expression? node)
    {
        return node.UpdateBase(new InternalStateVisitor());
    }

    private class InternalStateVisitor : ExpressionVisitor
    {
        private static readonly MethodInfo EnumerableContainsMethod = new Func<IEnumerable<TIdent>, TIdent, bool>(Enumerable.Contains).Method;

        private static readonly MethodInfo HashSetContainsMethod = new Func<TIdent, bool>(new HashSet<TIdent>().Contains).Method;



        private readonly IDictionaryCache<IEnumerable<TIdent>, Expression> constCache =

                new DictionaryCache<IEnumerable<TIdent>, Expression>(source => Expression.Constant(source.ToHashSet()));


        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            var request = from _ in Maybe.Return()

                          where node.Method == EnumerableContainsMethod

                          from hashSet in this.GetSource(node.Arguments[0])

                          select Expression.Call(this.constCache[hashSet], HashSetContainsMethod, node.Arguments[1]);


            return request.GetValueOrDefault(() => base.VisitMethodCall(node));
        }

        private Maybe<IEnumerable<TIdent>> GetSource(Expression enumerable)
        {
            return enumerable.GetDeepMemberConstValue<HashSet<TIdent>>().Select(v => (IEnumerable<TIdent>)v)
                             .Or(enumerable.GetDeepMemberConstValue<IQueryable<TIdent>>().Select(v => (IEnumerable<TIdent>)v));
        }
    }




    public static readonly OptimizeContainsCallVisitor<TIdent> Value = new OptimizeContainsCallVisitor<TIdent>();
}
