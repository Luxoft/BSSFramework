using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using Framework.Core;

namespace Framework.SecuritySystem.Rules.Builders
{
    internal class OptimizeContainsCallVisitor<TIdent> : ExpressionVisitor
    {
        private OptimizeContainsCallVisitor()
        {

        }


        public override Expression Visit(Expression node)
        {
            return node.UpdateBase(new InternalStateVisitor());
        }

        private class InternalStateVisitor : ExpressionVisitor
        {
            private static readonly MethodInfo EnumerableConstainsMethod = new Func<IEnumerable<TIdent>, TIdent, bool>(Enumerable.Contains).Method;

            private static readonly MethodInfo HashSetConstainsMethod = new Func<TIdent, bool>(new HashSet<TIdent>().Contains).Method;



            private readonly IDictionaryCache<IEnumerable<TIdent>, Expression> _constCache =

                new DictionaryCache<IEnumerable<TIdent>, Expression>(source => Expression.Constant(source.ToHashSet()));


            protected override Expression VisitMethodCall(MethodCallExpression node)
            {
                var request = from _ in Maybe.Return()

                              where node.Method == EnumerableConstainsMethod

                              from hashSet in this.GetSource(node.Arguments[0])

                              select Expression.Call(this._constCache[hashSet], HashSetConstainsMethod, node.Arguments[1]);


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
}
