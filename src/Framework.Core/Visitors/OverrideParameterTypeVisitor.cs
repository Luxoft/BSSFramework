using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Framework.Core
{
    public class OverrideParameterTypeVisitor : ExpressionVisitor
    {
        private readonly Dictionary<Type, Type> _map;


        public OverrideParameterTypeVisitor(Type from, Type to)
            : this(new Dictionary<Type, Type> { { from, to }})
        {

        }

        public OverrideParameterTypeVisitor(Dictionary<Type, Type> map)
        {
            if (map == null) throw new ArgumentNullException(nameof(map));

            this._map = map;
        }


        public override Expression Visit(Expression node)
        {
            return new InternalStateVisitor(this._map).Visit(node);
        }

        private class InternalStateVisitor : ExpressionVisitor
        {
            private readonly Dictionary<Type, Type> _map;

            private readonly IDictionaryCache<ParameterExpression, ParameterExpression> _parameterDict;

            public InternalStateVisitor(Dictionary<Type, Type> map)
            {
                this._map = map;

                if (map == null) throw new ArgumentNullException(nameof(map));

                this._parameterDict = new DictionaryCache<ParameterExpression, ParameterExpression>(fromParam =>
                    map.GetMaybeValue(fromParam.Type)
                       .Select(toType => Expression.Parameter(toType, fromParam.Name))
                       .GetValueOrDefault(fromParam));
            }


            protected override Expression VisitParameter(ParameterExpression node)
            {
                return this._parameterDict[node];
            }

            protected override Expression VisitLambda<T>(Expression<T> node)
            {
                var newParameters = node.Parameters.ToReadOnlyCollection(p => this._parameterDict[p]);

                var newBody = this.Visit(node.Body);

                return Expression.Lambda(newBody, newParameters);
            }

            protected override Expression VisitMember(MemberExpression node)
            {
                var request = from prop in (node.Member as PropertyInfo).ToMaybe()

                              from expr in this.Visit(node.Expression).ToMaybe()

                              select Expression.Property(expr, prop.Name);

                return request.GetValueOrDefault(() => base.VisitMember(node));
            }

            protected override Expression VisitMethodCall(MethodCallExpression node)
            {
                var newObject = this.Visit(node.Object);

                var newArgs = node.Arguments.ToReadOnlyCollection(this.Visit);

                var newMethod = node.Method.IsGenericMethod

                              ? node.Method.GetGenericMethodDefinition()
                                           .MakeGenericMethod(node.Method.GetGenericArguments().ToArray(t =>

                                            this._map.GetMaybeValue(t).GetValueOrDefault(t)))

                              : node.Method;


                return Expression.Call(newObject, newMethod, newArgs);
            }
        }
    }
}