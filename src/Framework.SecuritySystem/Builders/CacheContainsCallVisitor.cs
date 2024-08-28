using System.Linq.Expressions;
using System.Reflection;

using Framework.Core;

namespace Framework.SecuritySystem.Builders;

internal class CacheContainsCallVisitor : ExpressionVisitor
{
    private CacheContainsCallVisitor()
    {
    }

    public override Expression Visit(Expression? node)
    {
        return node.UpdateBase(new InternalStateVisitor());
    }

    private class InternalStateVisitor : ExpressionVisitor
    {
        private static readonly MethodInfo EnumerableContainsMethod = new Func<IEnumerable<Guid>, Guid, bool>(Enumerable.Contains).Method;

        private static readonly MethodInfo HashSetContainsMethod = new Func<Guid, bool>(new HashSet<Guid>().Contains).Method;



        private readonly IDictionaryCache<IEnumerable<Guid>, Expression> constCache =

                new DictionaryCache<IEnumerable<Guid>, Expression>(source => Expression.Constant(source.ToHashSet()));


        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            var request = from _ in Maybe.Return()

                          where node.Method == EnumerableContainsMethod

                          from hashSet in this.GetSource(node.Arguments[0])

                          select Expression.Call(this.constCache[hashSet], HashSetContainsMethod, node.Arguments[1]);


            return request.GetValueOrDefault(() => base.VisitMethodCall(node));
        }

        private Maybe<IEnumerable<Guid>> GetSource(Expression enumerable)
        {
            return enumerable.GetDeepMemberConstValue<HashSet<Guid>>().Select(v => (IEnumerable<Guid>)v)
                             .Or(enumerable.GetDeepMemberConstValue<IQueryable>().Select(v => (IEnumerable<Guid>)v));
        }
    }




    public static readonly CacheContainsCallVisitor Value = new CacheContainsCallVisitor();
}
