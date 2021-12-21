using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Framework.Core
{
    public class ExpandExplicitPropertyVisitor : ExpressionVisitor
    {
        private static readonly IDictionaryCache<Tuple<Type, MemberInfo>, PropertyInfo> ExpandPropertyCache = new DictionaryCache<Tuple<Type, MemberInfo>, PropertyInfo>(memberPair =>
        {
            var pathsRequest = from property in (memberPair.Item2 as PropertyInfo).ToMaybe()

                               from implementedProperty in memberPair.Item1.GetImplementedProperty(property).ToMaybe()

                               where property != implementedProperty

                               select implementedProperty;

            return pathsRequest.GetValueOrDefault();
        }).WithLock();


        private ExpandExplicitPropertyVisitor()
        {

        }


        protected override Expression VisitMember(MemberExpression node)
        {
            var visitedBody = this.Visit(node.Expression);

            var request = from body in visitedBody.ToMaybe()

                          from property in ExpandPropertyCache.GetValue(body.Type, node.Member).ToMaybe()

                          select Expression.Property(body, property);


            return request.GetValueOrDefault(() =>
            {
                if (visitedBody == node.Expression)
                {
                    return node;
                }
                else
                {
                    return Expression.MakeMemberAccess(visitedBody, node.Member);
                }
            });
        }


        public static readonly ExpandExplicitPropertyVisitor Value = new ExpandExplicitPropertyVisitor();
    }
}