using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Framework.Core
{
    internal static class InjectMaybeExpressionExtensions
    {
        private static readonly MethodInfo SelectMethod = new Func<Maybe<object>, Func<object, object>, Maybe<object>>(MaybeExtensions.Select).Method.GetGenericMethodDefinition();

        private static readonly MethodInfo SelectManyMethod = new Func<Maybe<object>, Func<object, Maybe<object>>, Maybe<object>>(MaybeExtensions.SelectMany).Method.GetGenericMethodDefinition();

        public static Expression TryGetValueOrDefault(this Expression expression)
        {
            var elementType = expression.Type.GetMaybeElementType();

            if (elementType == null)
            {
                return expression;
            }
            else
            {
                var getValueMethod = new Func<Maybe<object>, object>(MaybeExtensions.GetValueOrDefault).CreateGenericMethod(elementType);

                return Expression.Call(getValueMethod, expression);
            }
        }

        public static Expression Return(this Expression expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            var nullableType = expression.Type.GetNullableElementType();

            if (nullableType != null)
            {
                var wrapMethod = new Func<Ignore?, Maybe<Ignore>>(Maybe.ToMaybe).Method;

                return Expression.Call(wrapMethod.GetGenericMethodDefinition().MakeGenericMethod(nullableType), expression);

            }
            else
            {
                var wrapMethod = expression.Type.IsValueType ? new Func<Ignore, Maybe<Ignore>>(Maybe.Return).Method
                                                             : new Func<object, Maybe<object>>(Maybe.ToMaybe).Method;

                return Expression.Call(wrapMethod.GetGenericMethodDefinition().MakeGenericMethod(expression.Type), expression);
            }
        }


        public static Expression OverrideSelect(this IEnumerable<Expression> expressions, Func<Expression[], Expression> getResult)
        {
            using (var enumerator = expressions.GetEnumerator())
            {
                return enumerator.OverrideSelect(Enumerable.Empty<Expression>(), getResult);
            }
        }

        private static Expression OverrideSelect(this IEnumerator<Expression> preExpressions, IEnumerable<Expression> postExpressions, Func<Expression[], Expression> getResult)
        {
            if (preExpressions.MoveNext())
            {
                return preExpressions.Current.OverrideSelect(arg => preExpressions.OverrideSelect(postExpressions.Concat(new[] { arg }), getResult));
            }
            else
            {
                return getResult(postExpressions.ToArray());
            }
        }

        public static Expression OverrideSelect(this Expression expression, Func<Expression, Expression> getResult)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            return expression.OptimizeSelectSingle(getResult).Or(() => expression.OverrideSelectInternal(getResult))
                .GetValueOrDefault(() => getResult(expression));
        }

        private static Maybe<Expression> OptimizeSelectSingle(this Expression expression, Func<Expression, Expression> getResult)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            return from callExpr in (expression as MethodCallExpression).ToMaybe()

                   where callExpr.Method.IsGenericMethodImplementation(SelectMethod)

                   let selectLambda = (LambdaExpression)callExpr.Arguments[1]

                   let newBody = getResult(selectLambda.Body)

                   let isMany = newBody.Type.IsMaybe()

                   let newLambda = Expression.Lambda(newBody, selectLambda.Parameters)

                   let selectMethod = isMany ? SelectManyMethod : SelectMethod

                   let inputType = callExpr.Method.GetGenericArguments()[0]

                   let resultType = isMany ? newBody.Type.GetMaybeElementType() : newBody.Type

                   let newMethod = selectMethod.MakeGenericMethod(inputType, resultType)

                   let newCallExpr = Expression.Call(newMethod, callExpr.Arguments[0], newLambda)

                   select (Expression)newCallExpr;
        }

        private static Maybe<Expression> OverrideSelectInternal(this Expression expression, Func<Expression, Expression> getResult)
        {
            return from inputType in expression.Type.GetMaybeElementType().ToMaybe()

                   let param = Expression.Parameter(inputType)

                   let newLambda = Expression.Lambda(getResult(param), param)

                   let newBody = newLambda.Body

                   let isMany = newBody.Type.IsMaybe()

                   let selectMethod = isMany ? SelectManyMethod : SelectMethod

                   let resultType = isMany ? newBody.Type.GetMaybeElementType() : newBody.Type

                   let newMethod = selectMethod.MakeGenericMethod(inputType, resultType)

                   select (Expression)Expression.Call(newMethod, expression, newLambda);
        }


        //private TMemberBinding UpdateMemberBinding<TMemberBinding>(TMemberBinding memberBinding, IEnumerator<Expression> source)
        //    where TMemberBinding : MemberBinding
        //{
        //    return (TMemberBinding)UpdateMemberBindingBase(memberBinding, source);
        //}


        public static MemberBinding UpdateMemberBindingBase(this MemberBinding memberBinding, IEnumerator<Expression> source)
        {
            if (memberBinding is MemberMemberBinding)
            {
                var s = (memberBinding as MemberMemberBinding);

                return s.Update(s.Bindings.Select(inner => inner.UpdateMemberBindingBase(source)));
            }
            else if (memberBinding is MemberListBinding)
            {
                var s = (memberBinding as MemberListBinding);

                return s.Update(s.Initializers.Select(i => i.Update(source.ReadMany(i.Arguments.Count))));
            }
            else if (memberBinding is MemberAssignment)
            {
                var s = memberBinding as MemberAssignment;

                return s.Update(source.ReadSingle());
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(memberBinding));
            }
        }

        public static IEnumerable<Expression> GetMemberBindingExpresisons(this MemberBinding memberBinding)
        {
            if (memberBinding is MemberMemberBinding)
            {
                var s = (memberBinding as MemberMemberBinding);

                return s.Bindings.SelectMany(b => b.GetMemberBindingExpresisons());
            }
            else if (memberBinding is MemberListBinding)
            {
                var s = (memberBinding as MemberListBinding);

                return s.Initializers.SelectMany(i => i.Arguments);
            }
            else if (memberBinding is MemberAssignment)
            {
                var s = memberBinding as MemberAssignment;

                return new[] { s.Expression };
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(memberBinding));
            }
        }
    }
}
