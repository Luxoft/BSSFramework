using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using JetBrains.Annotations;

namespace Framework.Core
{
    public static class ExpressionExtensions
    {
        #region Build

        public static Expression<Func<T, bool>> BuildAnd<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            if (expr1 == null) throw new ArgumentNullException(nameof(expr1));
            if (expr2 == null) throw new ArgumentNullException(nameof(expr2));

            return from v1 in expr1
                   from v2 in expr2
                   select v1 && v2;
        }

        public static Expression<Func<T1, T2, bool>> BuildAnd<T1, T2>(this Expression<Func<T1, T2, bool>> expr1, Expression<Func<T1, T2, bool>> expr2)
        {
            if (expr1 == null) throw new ArgumentNullException(nameof(expr1));
            if (expr2 == null) throw new ArgumentNullException(nameof(expr2));

            var newExpr2Body = expr2.GetBodyWithOverrideParameters(expr1.Parameters.ToArray());

            var newBody = Expression.AndAlso(expr1.Body, newExpr2Body);

            return Expression.Lambda<Func<T1, T2, bool>>(newBody, expr1.Parameters);
        }

        public static Expression<Func<T, bool>> BuildOr<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            if (expr1 == null) throw new ArgumentNullException(nameof(expr1));
            if (expr2 == null) throw new ArgumentNullException(nameof(expr2));

            return from v1 in expr1
                   from v2 in expr2
                   select v1 || v2;
        }

        public static Expression<Func<T1, T2, bool>> BuildOr<T1, T2>(this Expression<Func<T1, T2, bool>> expr1, Expression<Func<T1, T2, bool>> expr2)
        {
            if (expr1 == null) throw new ArgumentNullException(nameof(expr1));
            if (expr2 == null) throw new ArgumentNullException(nameof(expr2));

            var newExpr2Body = expr2.GetBodyWithOverrideParameters(expr1.Parameters.ToArray());

            var newBody = Expression.OrElse(expr1.Body, newExpr2Body);

            return Expression.Lambda<Func<T1, T2, bool>>(newBody, expr1.Parameters);
        }


        public static Expression<Func<T, bool>> Not<T>(this Expression<Func<T, bool>> expr)
        {
            return from v in expr
                   select !v;
        }


        public static Expression<Func<T, bool>> BuildOr<T>(this IEnumerable<Expression<Func<T, bool>>> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return source.Match(() => _ => false,
                                single => single,
                                many => many.Aggregate(BuildOr));
        }

        public static Expression<Func<T1, T2, bool>> BuildOr<T1, T2>(this IEnumerable<Expression<Func<T1, T2, bool>>> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return source.Match(() => (_, __) => true,
                                single => single,
                                many => many.Aggregate(BuildOr));
        }

        public static Expression<Func<T, bool>> BuildAnd<T>(this IEnumerable<Expression<Func<T, bool>>> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return source.Match(() => _ => true,
                                single => single,
                                many => many.Aggregate(BuildAnd));
        }

        public static Expression<Func<T1, T2, bool>> BuildAnd<T1, T2>(this IEnumerable<Expression<Func<T1, T2, bool>>> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return source.Match(() => (_, __) => true,
                                single => single,
                                many => many.Aggregate(BuildAnd));
        }

        public static Expression<Func<TResult, bool>> BuildOr<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, Expression<Func<TResult, bool>>> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return source.Select(selector).BuildOr();
        }

        public static Expression<Func<TResult, bool>> BuildAnd<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, Expression<Func<TResult, bool>>> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return source.Select(selector).BuildAnd();
        }

        #endregion

        #region ToPath

        public static string ToPath(this Expression source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));


            var result = string.Empty;
            var expressionType = source.GetType();
            if (source is LambdaExpression)
            {
                var body = ((LambdaExpression)source).Body;
                return body.ToPath();
            }
            if (source is MemberExpression)
            {
                var memberExpression = (MemberExpression)source;
                var leftExpression = memberExpression.Expression;
                var rightPath = memberExpression.Member.Name;

                if (leftExpression is ConstantExpression) // TODO: rewrite to common case
                {
                    var constantExpression = (ConstantExpression)leftExpression;
                    var field = constantExpression.Value.GetType().GetField(rightPath);

                    return field.GetValue(constantExpression.Value).ToString();
                }

                var leftPath = leftExpression.ToPath();
                return leftPath.MaybeString(z => z + "." + rightPath).IfDefaultString(rightPath);
            }
            if (source is UnaryExpression)
            {
                return ((UnaryExpression)source).Operand.ToPath();
            }
            if (source is BinaryExpression)
            {
                var binaryExpression = (BinaryExpression)source;
                return $"{binaryExpression.Left.ToPath()}{ToStringRepresentation(binaryExpression.NodeType, binaryExpression.Right)}";
            }
            if (source is ConstantExpression)
            {
                var constantExpression = (ConstantExpression)source;

                return constantExpression.Value.ToString();
            }
            if (source is MethodCallExpression)
            {
                var methodCallExpression = (MethodCallExpression)source;
                var @object = methodCallExpression.Object;
                var leftPath = @object.Maybe(z => z.ToPath(), string.Empty);
                if (string.Equals(methodCallExpression.Method.Name, "get_Item",
                                  StringComparison.InvariantCultureIgnoreCase))
                {
                    return $"{leftPath.MaybeString(z => z)}[{string.Join(",", methodCallExpression.Arguments.Select(z => ToPath(z)))}]";
                }
                return
                    $"{leftPath.MaybeString(z => z + ".")}{methodCallExpression.Method.Name}({string.Join(",", methodCallExpression.Arguments.Select(z => z.ToPath()))})";
            }
            return result;
        }

        private static string ToStringRepresentation(ExpressionType source, Expression expression)
        {
            switch (source)
            {
                case ExpressionType.ArrayIndex:
                    return $"[{expression.ToPath()}]";
                default:
                    throw new NotSupportedException(
                        $"Not supported expressionType format. ExpressionType:{source.ToString()}");
            }
        }

        public static string ToPath<TSource, TResult>(this TSource source, Expression<Func<TSource, TResult>> expr)
        {
            return expr.ToPath();
        }

        /// <summary>
        /// Получение имени мембера (поле или свойство)
        /// </summary>
        /// <typeparam name="TFunc"></typeparam>
        /// <param name="expr">Выражение</param>
        /// <returns></returns>
        public static string GetInstanceMemberName<TFunc>(this Expression<TFunc> expr)
        {
            if (expr == null) throw new ArgumentNullException(nameof(expr));

            var request = from memberExpr in (expr.Body as MemberExpression).ToMaybe()

                          let member = memberExpr.Member

                          where (member is PropertyInfo || member is FieldInfo)

                             && memberExpr.Expression != null

                          select member.Name;

            return request.GetValue(() => new Exception($"invalid expression: {expr}"));
        }

        /// <summary>
        /// Получение имени мембера (поле или свойство)
        /// </summary>
        /// /// <typeparam name="T"></typeparam>
        /// <param name="expr">Выражение</param>
        /// <returns></returns>
        public static string GetStaticMemberName<T>(this Expression<Func<T>> expr)
        {
            if (expr == null) throw new ArgumentNullException(nameof(expr));

            var request = from memberExpr in (expr.Body as MemberExpression).ToMaybe()

                          let member = memberExpr.Member

                          where (member is PropertyInfo || member is FieldInfo)

                             && memberExpr.Expression == null

                          select member.Name;

            return request.GetValue(() => new Exception($"invalid expression: {expr}"));
        }

        #endregion

        #region Select-SelectMany

        public static Expression<Func<TOwner, Func<TSourceArg, TSelectorResult>>> SelectO<TOwner, TSourceArg, TSourceResult, TSelectorResult>(this Expression<Func<TSourceArg, TSourceResult>> sourceExpr, Expression<Func<TOwner, TSourceResult, TSelectorResult>> selector)
        {
            if (sourceExpr == null) throw new ArgumentNullException(nameof(sourceExpr));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            var ownerParam = selector.Parameters[0];
            var internalParam = sourceExpr.Parameters[0];

            var internalBody = selector.Body.Override(selector.Parameters[1], sourceExpr.Body);

            var internalLambda = Expression.Lambda<Func<TSourceArg, TSelectorResult>>(internalBody, internalParam);

            return Expression.Lambda<Func<TOwner, Func<TSourceArg, TSelectorResult>>>(internalLambda, ownerParam);
        }

        public static Expression<Func<TSourceArg, TSelectorResult>> Select<TSourceArg, TSourceResult, TSelectorResult>(this Expression<Func<TSourceArg, TSourceResult>> sourceExpr, Expression<Func<TSourceResult, TSelectorResult>> selector)
        {
            if (sourceExpr == null) throw new ArgumentNullException(nameof(sourceExpr));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return Expression.Lambda<Func<TSourceArg, TSelectorResult>>(selector.Body.Override(selector.Parameters.Single(), sourceExpr.Body), sourceExpr.Parameters);
        }

        public static Expression<Func<TSourceArg, TSelectorResult>> Select<TSourceArg, TSourceResult, TSelectorResult>(this Expression<Func<TSourceArg, TSourceResult>> sourceExpr, Expression<Func<TSourceResult, TSelectorResult>> selector, bool cacheSelectorSource)
        {
            if (sourceExpr == null) throw new ArgumentNullException(nameof(sourceExpr));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            if (cacheSelectorSource)
            {
                return Expression.Lambda<Func<TSourceArg, TSelectorResult>>(sourceExpr.Body.WithPipe(selector), sourceExpr.Parameters);
            }
            else
            {
                return sourceExpr.Select(selector);
            }
        }

        public static Expression<Func<TSourceArg1, TSourceArg2, TSelectorResult>> Select<TSourceArg1, TSourceArg2, TSourceResult, TSelectorResult>(this Expression<Func<TSourceArg1, TSourceArg2, TSourceResult>> sourceExpr, Expression<Func<TSourceResult, TSelectorResult>> selector)
        {
            if (sourceExpr == null) throw new ArgumentNullException(nameof(sourceExpr));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return Expression.Lambda<Func<TSourceArg1, TSourceArg2, TSelectorResult>>(selector.Body.Override(selector.Parameters.Single(), sourceExpr.Body), sourceExpr.Parameters);
        }

        public static Expression<Func<TSourceArg1, TSourceArg2, TSelectorResult>> Select<TSourceArg1, TSourceArg2, TSourceResult, TSelectorResult>(this Expression<Func<TSourceArg1, TSourceArg2, TSourceResult>> sourceExpr, Expression<Func<TSourceResult, TSelectorResult>> selector, bool cacheSelectorSource)
        {
            if (sourceExpr == null) throw new ArgumentNullException(nameof(sourceExpr));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            if (cacheSelectorSource)
            {
                return Expression.Lambda<Func<TSourceArg1, TSourceArg2, TSelectorResult>>(sourceExpr.Body.WithPipe(selector), sourceExpr.Parameters);
            }
            else
            {
                return sourceExpr.Select(selector);
            }
        }

        public static Expression<Func<TSourceArg1, TSourceArg2, TSourceArg3, TSelectorResult>> Select<TSourceArg1, TSourceArg2, TSourceArg3, TSourceResult, TSelectorResult>(this Expression<Func<TSourceArg1, TSourceArg2, TSourceArg3, TSourceResult>> sourceExpr, Expression<Func<TSourceResult, TSelectorResult>> selector)
        {
            if (sourceExpr == null) throw new ArgumentNullException(nameof(sourceExpr));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return Expression.Lambda<Func<TSourceArg1, TSourceArg2, TSourceArg3, TSelectorResult>>(selector.Body.Override(selector.Parameters.Single(), sourceExpr.Body), sourceExpr.Parameters);
        }

        public static Expression<Func<TSourceArg1, TSourceArg2, TSourceArg3, TSelectorResult>> Select<TSourceArg1, TSourceArg2, TSourceArg3, TSourceResult, TSelectorResult>(this Expression<Func<TSourceArg1, TSourceArg2, TSourceArg3, TSourceResult>> sourceExpr, Expression<Func<TSourceResult, TSelectorResult>> selector, bool cacheSelectorSource)
        {
            if (sourceExpr == null) throw new ArgumentNullException(nameof(sourceExpr));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            if (cacheSelectorSource)
            {
                return Expression.Lambda<Func<TSourceArg1, TSourceArg2, TSourceArg3, TSelectorResult>>(sourceExpr.Body.WithPipe(selector), sourceExpr.Parameters);
            }
            else
            {
                return sourceExpr.Select(selector);
            }
        }



        public static Expression<Func<TSourceArg, TSelectorResult>> SelectMany<TSourceArg, TSourceResult, TNextResult, TSelectorResult>(
            this Expression<Func<TSourceArg, TSourceResult>> sourceExpr,
            Expression<Func<TSourceResult, Expression<Func<TSourceArg, TNextResult>>>> nextExpr,
            Expression<Func<TSourceResult, TNextResult, TSelectorResult>> resultSelector)
        {
            var nextBody = VisitNextResult<TSourceArg, TSourceResult, TNextResult>(sourceExpr, nextExpr.Body);

            return Expression.Lambda<Func<TSourceArg, TSelectorResult>>(

                resultSelector.Body.Override(resultSelector.Parameters[1], nextBody)
                                   .Override(resultSelector.Parameters[0], sourceExpr.Body),

                sourceExpr.Parameters);
        }

        private static Expression VisitNextResult<TSourceArg, TSourceResult, TNextResult>(Expression<Func<TSourceArg, TSourceResult>> inputExpr, Expression nextExprBlock)
        {
            if (nextExprBlock is ConditionalExpression)
            {
                var conditionalExpression = nextExprBlock as ConditionalExpression;

                return Expression.Condition(
                    VisitNextTest(inputExpr, inputExpr),
                    VisitNextResult<TSourceArg, TSourceResult, TNextResult>(inputExpr, conditionalExpression.IfTrue),
                    VisitNextResult<TSourceArg, TSourceResult, TNextResult>(inputExpr, conditionalExpression.IfFalse));
            }

            if (nextExprBlock == inputExpr.Parameters.Single())
            {
                return inputExpr.Body;
            }

            //if (nextExprBlock is ConstantExpression)
            //{
            //    var constantExpression = nextExprBlock as ConstantExpression;

            //    var lambda = constantExpression.Value as LambdaExpression;

            //    return new OverrideExpressionVisitor(lambda.Parameters.Single(), inputExpr.Parameters.Single()).Visit(lambda.Body);
            //}

            if (nextExprBlock is MemberExpression)
            {
                var memberExpression = nextExprBlock as MemberExpression;

                if (memberExpression.Expression is ConstantExpression)
                {
                    var lambda = (memberExpression.Member as FieldInfo).GetValue((memberExpression.Expression as ConstantExpression).Value) as LambdaExpression;

                    return lambda.Body.Override(lambda.Parameters.Single(), inputExpr.Parameters.Single());
                }
            }

            throw new NotImplementedException();
            //var evalMethod = new Func<Expression<Func<TSource, TResult2>>, TSource, TResult2> (Eval).Method;

            //return Expression.Call(null, evalMethod, nextExprBlock);
        }

        private static Expression VisitNextTest<TSourceArg, TSourceResult>(Expression<Func<TSourceArg, TSourceResult>> sourceExpr, Expression nextExprBlock)
        {
            if (nextExprBlock == sourceExpr.Parameters.Single())
            {
                return sourceExpr.Body;
            }

            return nextExprBlock;
        }

        #endregion

        #region Eval

        public static System.Delegate Compile(this LambdaExpression lambdaExpression, ILambdaCompileCache cache)
        {
            if (lambdaExpression == null) throw new ArgumentNullException(nameof(lambdaExpression));

            return (System.Delegate)new Func<Expression<Action>, ILambdaCompileCache, Action>(Compile).CreateGenericMethod(lambdaExpression.Type)
                                                                                               .Invoke(null, new object[] { lambdaExpression, cache });
        }

        public static TDelegate Compile<TDelegate>(this Expression<TDelegate> inputExpr, bool cache)
        {
            if (inputExpr == null) throw new ArgumentNullException(nameof(inputExpr));

            return cache ? inputExpr.Compile(default(ILambdaCompileCache)) : inputExpr.Compile();
        }

        public static TDelegate Compile<TDelegate>(this Expression<TDelegate> inputExpr, ILambdaCompileCache cache)
        {
            if (inputExpr == null) throw new ArgumentNullException(nameof(inputExpr));

            return (cache ?? LambdaCompileCache.Default).GetFunc(inputExpr);
        }

        public static void Eval(this Expression<Action> inputExpr, ILambdaCompileCache cache = null)
        {
            if (inputExpr == null) throw new ArgumentNullException(nameof(inputExpr));

            inputExpr.Compile(cache)();
        }

        public static void Eval<TArg>(this Expression<Action<TArg>> inputExpr, TArg arg1, ILambdaCompileCache cache = null)
        {
            if (inputExpr == null) throw new ArgumentNullException(nameof(inputExpr));

            inputExpr.Compile(cache)(arg1);
        }

        public static TResult Eval<TResult>(this Expression<Func<TResult>> inputExpr)
        {
            if (inputExpr == null) throw new ArgumentNullException(nameof(inputExpr));

            return inputExpr.Eval(null);
        }

        public static TResult Eval<TResult>(this Expression<Func<TResult>> inputExpr, ILambdaCompileCache cache)
        {
            if (inputExpr == null) throw new ArgumentNullException(nameof(inputExpr));

            return inputExpr.Compile(cache)();
        }

        public static TResult Eval<T1, TResult>(this Expression<Func<T1, TResult>> inputExpr, T1 arg1)
        {
            if (inputExpr == null) throw new ArgumentNullException(nameof(inputExpr));

            return inputExpr.Eval(arg1, null);
        }

        public static TResult Eval<T1, TResult>(this Expression<Func<T1, TResult>> inputExpr, T1 arg1, ILambdaCompileCache cache)
        {
            if (inputExpr == null) throw new ArgumentNullException(nameof(inputExpr));

            return inputExpr.Compile(cache)(arg1);
        }

        public static TResult Eval<T1, T2, TResult>(this Expression<Func<T1, T2, TResult>> inputExpr, T1 arg1, T2 arg2)
        {
            if (inputExpr == null) throw new ArgumentNullException(nameof(inputExpr));

            return inputExpr.Eval(arg1, arg2, null);
        }

        public static TResult Eval<T1, T2, TResult>(this Expression<Func<T1, T2, TResult>> inputExpr, T1 arg1, T2 arg2, ILambdaCompileCache cache)
        {
            if (inputExpr == null) throw new ArgumentNullException(nameof(inputExpr));

            return inputExpr.Compile(cache)(arg1, arg2);
        }

        public static TResult Eval<T1, T2, T3, TResult>(this Expression<Func<T1, T2, T3, TResult>> inputExpr, T1 arg1, T2 arg2, T3 arg3, ILambdaCompileCache cache = null)
        {
            if (inputExpr == null) throw new ArgumentNullException(nameof(inputExpr));

            return inputExpr.Compile(cache)(arg1, arg2, arg3);
        }

        #endregion

        #region InjectMaybe

        /// <summary>
        /// ќбрамление всех ссылочных вызовов через Maybe
        /// </summary>
        /// <typeparam name="TDelegate"></typeparam>
        /// <param name="expr"></param>
        /// <returns></returns>
        public static Expression<TDelegate> InjectMaybe<TDelegate>(this Expression<TDelegate> expr)
        {
            if (expr == null) throw new ArgumentNullException(nameof(expr));

            return InjectMaybeVisitor.Value.VisitAndGetValueOrDefault(expr);
        }

        public static LambdaExpression InjectMaybeBase(this LambdaExpression expr)
        {
            if (expr == null) throw new ArgumentNullException(nameof(expr));

            var newBody = InjectMaybeVisitor.Value.Visit(expr.Body);

            return Expression.Lambda(newBody, expr.Parameters);
        }

        #endregion

        #region ToIgnoreArgFunc

        public static Expression<Func<TResult>> ToIgnoreArgFunc<TInput, TResult>(this Expression<Func<TInput, TResult>> expr, TInput input)
        {
            if (expr == null) throw new ArgumentNullException(nameof(expr));

            return Expression.Lambda<Func<TResult>>(expr.Body.Override(expr.Parameters[0], Expression.Constant(input)));
        }

        public static Expression<Func<TArg1, TResult>> ToIgnoreArgFunc<TArg1, TArg2, TResult>(this Expression<Func<TArg1, TArg2, TResult>> expr, TArg2 arg2)
        {
            if (expr == null) throw new ArgumentNullException(nameof(expr));

            return Expression.Lambda<Func<TArg1, TResult>>(expr.Body.Override(expr.Parameters[1], Expression.Constant(arg2)));
        }

        #endregion

        #region Optimize

        public static Expression<TDelegate> Optimize<TDelegate>(this Expression<TDelegate> expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            return expression.UpdateBody(OptimizeBooleanLogicVisitor.Value);
        }

        public static Expression<TDelegate> ExpandConst<TDelegate>(this Expression<TDelegate> expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            return expression.UpdateBody(ExpandConstVisitor.Value);
        }

        #endregion

        public static Expression WithSelect(this Expression sourceExpression, Func<ParameterExpression, Expression> getBody)
        {
            if (sourceExpression == null) throw new ArgumentNullException(nameof(sourceExpression));
            if (getBody == null) throw new ArgumentNullException(nameof(getBody));

            var inputElementType = sourceExpression.Type.GetMaybeElementType() ?? sourceExpression.Type.GetEnumerableElementType();

            var parameter = Expression.Parameter(inputElementType);

            var body = getBody(parameter);

            var selector = Expression.Lambda(body, parameter);

            return sourceExpression.WithSelect(selector);
        }

        public static Expression WithSelect(this Expression sourceExpression, LambdaExpression elementSelector)
        {
            if (sourceExpression == null) throw new ArgumentNullException(nameof(sourceExpression));
            if (elementSelector == null) throw new ArgumentNullException(nameof(elementSelector));

            var selectMethod = sourceExpression.Type.IsMaybe()

                             ? new Func<Maybe<object>, Func<object, object>, Maybe<object>>(MaybeExtensions.Select).CreateGenericMethod(elementSelector.Parameters.Single().Type, elementSelector.ReturnType)

                             : new Func<IEnumerable<object>, Func<object, object>, IEnumerable<object>>(Enumerable.Select).CreateGenericMethod(elementSelector.Parameters.Single().Type, elementSelector.ReturnType);

            return Expression.Call(null, selectMethod, sourceExpression, elementSelector);
        }


        public static Expression WithPipe(this Expression sourceExpression, LambdaExpression pipeSelector)
        {
            if (sourceExpression == null) throw new ArgumentNullException(nameof(sourceExpression));
            if (pipeSelector == null) throw new ArgumentNullException(nameof(pipeSelector));

            var pipeMethod = new Func<object, Func<object, object>, object>(PipeObjectExtensions.Pipe).CreateGenericMethod(pipeSelector.Parameters.Single().Type, pipeSelector.ReturnType);

            return Expression.Call(null, pipeMethod, sourceExpression, pipeSelector);
        }

        public static Expression WithPipe(this Expression sourceExpression, Func<ParameterExpression, Expression> getBody)
        {
            if (sourceExpression == null) throw new ArgumentNullException(nameof(sourceExpression));
            if (getBody == null) throw new ArgumentNullException(nameof(getBody));

            var parameter = Expression.Parameter(sourceExpression.Type);

            var body = getBody(parameter);

            var selector = Expression.Lambda(body, parameter);

            return sourceExpression.WithPipe(selector);
        }


        public static Expression WithPipe(this IEnumerable<Expression> sourceExpressions, Func<ParameterExpression[], Expression> getBody)
        {
            if (sourceExpressions == null) throw new ArgumentNullException(nameof(sourceExpressions));
            if (getBody == null) throw new ArgumentNullException(nameof(getBody));

            using (var sourceEnumerator = sourceExpressions.GetEnumerator())
            {
                return sourceEnumerator.WithPipe(new ParameterExpression[0], getBody);
            }
        }

        private static Expression WithPipe(this IEnumerator<Expression> sourceEnumerator, IEnumerable<ParameterExpression> parameters, Func<ParameterExpression[], Expression> getBody)
        {
            if (sourceEnumerator == null) throw new ArgumentNullException(nameof(sourceEnumerator));
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));
            if (getBody == null) throw new ArgumentNullException(nameof(getBody));


            if (sourceEnumerator.MoveNext())
            {
                return sourceEnumerator.Current.WithPipe(parameter =>

                    sourceEnumerator.WithPipe(parameters.Concat(new[] { parameter }), getBody));
            }
            else
            {
                return getBody(parameters.ToArray());
            }
        }

        public static Expression<Func<IEnumerable<T>, IEnumerable<T>>> ToCollectionFilter<T>(this Expression<Func<T, bool>> filter)
        {
            if (filter == null) throw new ArgumentNullException(nameof(filter));

            var param = Expression.Parameter(typeof(IEnumerable<T>));

            var whereMethod = new Func<IEnumerable<T>, Func<T, bool>, IEnumerable<T>>(Enumerable.Where).Method;

            return Expression.Lambda<Func<IEnumerable<T>, IEnumerable<T>>>(Expression.Call(null, whereMethod, param, filter), param);
        }


        public static Expression<Func<IEnumerable<TArg>, IEnumerable<TResult>>> ToEnumerable<TArg, TResult>(this Expression<Func<TArg, TResult>> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var param = Expression.Parameter(typeof(IEnumerable<TArg>));

            var selectMethod = new Func<IEnumerable<TArg>, Func<TArg, TResult>, IEnumerable<TResult>>(Enumerable.Select).Method;

            var callExpr = Expression.Call(null, selectMethod, param, source);

            return Expression.Lambda<Func<IEnumerable<TArg>, IEnumerable<TResult>>>(callExpr, param);
        }

        public static Expression<Func<IEnumerable<TArg>, bool>> ToEnumerableAny<TArg>(this Expression<Func<TArg, bool>> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var param = Expression.Parameter(typeof(IEnumerable<TArg>));

            var anyMethod = new Func<IEnumerable<TArg>, Func<TArg, bool>, bool>(Enumerable.Any).Method;

            var callExpr = Expression.Call(null, anyMethod, param, source);

            return Expression.Lambda<Func<IEnumerable<TArg>, bool>>(callExpr, param);
        }

        public static Expression<TDelegate> UpdateBody<TDelegate>(this Expression<TDelegate> expression, ExpressionVisitor bodyVisitor)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));
            if (bodyVisitor == null) throw new ArgumentNullException(nameof(bodyVisitor));

            return expression.Update(bodyVisitor.Visit(expression.Body), expression.Parameters);
        }

        public static LambdaExpression UpdateBodyBase(this LambdaExpression expression, ExpressionVisitor bodyVisitor)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));
            if (bodyVisitor == null) throw new ArgumentNullException(nameof(bodyVisitor));

            return Expression.Lambda(bodyVisitor.Visit(expression.Body), expression.Parameters);
        }

        public static Expression<Func<TTo, TRetType>> Covariance<TTo, TFrom, TRetType>(this Expression<Func<TFrom, TRetType>> source)
            where TTo : TFrom
        {
            return source.OverrideInput((TTo to) => (TFrom)to);
        }

        public static Expression<Action<TTo>> Covariance<TTo, TFrom>(this Expression<Action<TFrom>> source)
            where TTo : TFrom
        {
            return source.OverrideInput((TTo to) => (TFrom)to);
        }

        public static Expression<Func<T1, T3>> OverrideInput<T1, T2, T3>(this Expression<Func<T2, T3>> expr2, Expression<Func<T1, T2>> expr1)
        {
            if (expr1 == null) throw new ArgumentNullException(nameof(expr1));
            if (expr2 == null) throw new ArgumentNullException(nameof(expr2));

            return Expression.Lambda<Func<T1, T3>>(expr2.Body.Override(expr2.Parameters.Single(), expr1.Body), expr1.Parameters);
        }

        public static Expression<Action<T1>> OverrideInput<T1, T2>(this Expression<Action<T2>> expr2, Expression<Func<T1, T2>> expr1)
        {
            if (expr1 == null) throw new ArgumentNullException(nameof(expr1));
            if (expr2 == null) throw new ArgumentNullException(nameof(expr2));

            return Expression.Lambda<Action<T1>>(expr2.Body.Override(expr2.Parameters.Single(), expr1.Body), expr1.Parameters);
        }

        public static Expression<Action<T1, T2, TNew>> OverrideThird<T1, T2, TOld, TNew>(this Expression<Action<T1, T2, TOld>> expr, Expression<Func<TNew, TOld>> convertExpr)
        {
            if (expr == null) throw new ArgumentNullException(nameof(expr));
            if (convertExpr == null) throw new ArgumentNullException(nameof(convertExpr));


            var oldInputParam = expr.Parameters[2];
            var newInputParam = Expression.Parameter(typeof(TNew));

            var convertPipeExpr = Expression.Call(new Func<TNew, Func<TNew, TOld>, TOld>(PipeObjectExtensions.Pipe).Method, newInputParam, convertExpr);

            var continuePipeExpr = Expression.Call(new Action<TOld, Action<TOld>>(PipeObjectExtensions.Pipe).Method, convertPipeExpr, Expression.Lambda(expr.Body, oldInputParam));

            return Expression.Lambda<Action<T1, T2, TNew>>(continuePipeExpr, expr.Parameters[0], expr.Parameters[1], newInputParam);
        }

        public static Expression<Func<T1, T2, TNew, TResult>> OverrideThird<T1, T2, TOld, TNew, TResult>(this Expression<Func<T1, T2, TOld, TResult>> expr, Expression<Func<TNew, TOld>> convertExpr)
        {
            if (expr == null) throw new ArgumentNullException(nameof(expr));
            if (convertExpr == null) throw new ArgumentNullException(nameof(convertExpr));


            var oldInputParam = expr.Parameters[2];
            var newInputParam = Expression.Parameter(typeof(TNew));

            var convertPipeExpr = Expression.Call(new Func<TNew, Func<TNew, TOld>, TOld>(PipeObjectExtensions.Pipe).Method, newInputParam, convertExpr);

            var continuePipeExpr = Expression.Call(new Func<TNew, Func<TNew, TOld>, TOld>(PipeObjectExtensions.Pipe).Method, convertPipeExpr, Expression.Lambda(expr.Body, oldInputParam));

            return Expression.Lambda<Func<T1, T2, TNew, TResult>>(continuePipeExpr, expr.Parameters[0], expr.Parameters[1], newInputParam);
        }

        public static Expression<Func<T1, TNew, T3, TResult>> OverrideSecond<T1, TOld, TNew, T3, TResult>(this Expression<Func<T1, TOld, T3, TResult>> expr, Expression<Func<TNew, TOld>> convertExpr)
        {
            if (expr == null) throw new ArgumentNullException(nameof(expr));
            if (convertExpr == null) throw new ArgumentNullException(nameof(convertExpr));


            var oldInputParam = expr.Parameters[1];
            var newInputParam = Expression.Parameter(typeof(TNew));

            var convertPipeExpr = Expression.Call(new Func<TNew, Func<TNew, TOld>, TOld>(PipeObjectExtensions.Pipe).Method, newInputParam, convertExpr);

            var continuePipeExpr = Expression.Call(new Func<TOld, Func<TOld, TNew>, TNew>(PipeObjectExtensions.Pipe).Method, convertPipeExpr, Expression.Lambda(expr.Body, oldInputParam));

            return Expression.Lambda<Func<T1, TNew, T3, TResult>>(continuePipeExpr, expr.Parameters[0], expr.Parameters[1], newInputParam);
        }

        public static Expression<Func<T1, TNew, TResult>> OverrideSecond<T1, TOld, TNew, TResult>(this Expression<Func<T1, TOld, TResult>> expr, Expression<Func<TNew, TOld>> convertExpr)
        {
            if (expr == null) throw new ArgumentNullException(nameof(expr));
            if (convertExpr == null) throw new ArgumentNullException(nameof(convertExpr));


            var oldInputParam = expr.Parameters[1];
            var newInputParam = Expression.Parameter(typeof(TNew));

            var convertPipeExpr = Expression.Call(new Func<TNew, Func<TNew, TOld>, TOld>(PipeObjectExtensions.Pipe).Method, newInputParam, convertExpr);

            var continuePipeExpr = Expression.Call(new Func<TOld, Func<TOld, TNew>, TNew>(PipeObjectExtensions.Pipe).Method, convertPipeExpr, Expression.Lambda(expr.Body, oldInputParam));

            return Expression.Lambda<Func<T1, TNew, TResult>>(continuePipeExpr, expr.Parameters[0], expr.Parameters[1], newInputParam);
        }

        public static Expression ExtractBoxingValue(this Expression expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            return expression.GetConvertOperand().GetValueOrDefault(expression);
        }

        public static Maybe<Expression> GetConvertOperand(this Expression expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            return from unaryExpression in (expression as UnaryExpression).ToMaybe()

                   where expression.NodeType == ExpressionType.Convert

                   select unaryExpression.Operand;
        }

        public static Expression TryLiftToNullable(this Expression expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            if (expression.Type.IsNullable() || !expression.Type.IsValueType)
            {
                return expression;
            }
            else
            {
                return Expression.Convert(expression, typeof(Nullable<>).MakeGenericType(expression.Type));
            }
        }

        public static Maybe<ConstantExpression> GetMemberConstExpression(this Expression expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            return (expression as ConstantExpression).ToMaybe()

                .Or(() => from memberExpr in (expression as MemberExpression).ToMaybe()

                          from constExpr in (memberExpr.Expression as ConstantExpression).ToMaybe()

                          from fieldInfo in (memberExpr.Member as FieldInfo).ToMaybe()

                          select Expression.Constant(fieldInfo.GetValue(constExpr.Value), fieldInfo.FieldType));
        }

        /// <summary> Get Member <seealso cref="ConstantExpression"/> from <paramref name="expression"/> with fall down to hierarchy
        /// </summary>
        /// <param name="expression">original expression</param>
        /// <returns>Member <seealso cref="ConstantExpression"/> if exists at specified <paramref name="expression"/></returns>
        public static Maybe<ConstantExpression> GetDeepMemberConstExpression(this Expression expression)
        {
            var result = expression.GetPureDeepMemberConstExpression();
            if (result == null)
            {
                return Maybe<ConstantExpression>.Nothing;
            }

            var constExpr = expression as ConstantExpression;
            return constExpr != null ? constExpr.ToMaybe() : Maybe.Return(result);
        }

        /// <summary>
        /// Get Member <seealso cref="ConstantExpression"/> from <paramref name="expression"/> with fall down to hierarchy
        /// </summary>
        /// <param name="expression">original expression</param>
        /// <returns>Member <seealso cref="ConstantExpression"/> if exists at specified <paramref name="expression"/></returns>
        public static ConstantExpression GetPureDeepMemberConstExpression(this Expression expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            var constExpr = expression as ConstantExpression;
            if (constExpr != null)
            {
                return constExpr;
            }

            var memberExpr = expression as MemberExpression;
            if (memberExpr == null)
            {
                return null;
            }

            var memberChains = memberExpr.GetAllElements(z => z.Expression as MemberExpression).TakeWhile(x => x != null).ToList();

            var startExpr = memberChains.Last();

            constExpr = startExpr.Expression as ConstantExpression;
            if (constExpr == null)
            {
                return constExpr;
            }

            var constValue = ValueTuple.Create(startExpr.Member.GetValue(constExpr.Value), startExpr.Member.GetMemberType());
            memberChains.Reverse();
            var finalValue = memberChains
                    .Skip(1) // выше обработали самый первый (var startExpr = memberChains.Last();)
                    .Select(z => z.Member)
                    .Aggregate(
                               constValue,
                               (prevValue, memberInfo) => ValueTuple.Create(memberInfo.GetValue(prevValue.Item1), memberInfo.GetMemberType()));

            return Expression.Constant(finalValue.Item1, finalValue.Item2);
        }

        private static object GetValue(this MemberInfo source, object arg)
        {
            var field = source as FieldInfo;
            if(field != null)
            {
                return field.GetValue(arg);
            }

            // To prevent #IADFRAME-1191
            if (arg == null)
            {
                return null;
            }

            // TODO Разобраться с последним Cast, Что делать если это не PropertyInfo
            return (source as PropertyInfo)?.GetValue(arg);
        }

        private static Type GetMemberType(this MemberInfo source)
        {
            var field = source as FieldInfo;
            if (field != null)
            {
                return field.FieldType;
            }

            // TODO Разобраться с последним Cast, Что делать если это не PropertyInfo
            return (source as PropertyInfo).PropertyType;
        }

        public static PropertyInfo GetProperty([NotNull] this Expression source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var request = from memberExpr in (source as MemberExpression).ToMaybe()

                          from property in (memberExpr.Member as PropertyInfo).ToMaybe()

                          select property;

            return request.GetValue(() => "Invalid expression");
        }

        public static IEnumerable<PropertyInfo> GetReverseProperties([NotNull] this LambdaExpression source)
        {
            var parameter = source.Parameters.Single();

            for (var state = source.Body; state != parameter;)
            {
                var memberExpr = (MemberExpression)state;

                yield return (PropertyInfo)memberExpr.Member;

                state = memberExpr.Expression;
            }
        }

        /// <summary> Returns constant value from expression
        /// </summary>
        /// <param name="expression">expression to get value from</param>
        /// <returns>constant value</returns>
        public static Maybe<object> GetMemberConstValue(this Expression expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            return expression.GetMemberConstExpression().Select(expr => expr.Value);
        }

        /// <summary> Returns constant value from expression
        /// </summary>
        /// <typeparam name="TValue">cast value to specified Type if possible</typeparam>
        /// <param name="expression">expression to get value from</param>
        /// <returns>constant value of specified Type</returns>
        public static Maybe<TValue> GetMemberConstValue<TValue>(this Expression expression)
        {
            return GetMemberConstValue(expression).Where(v => v is TValue).Select(v => (TValue)v);
        }

        /// <summary> Returns constant value from expression with fall down to hierarchy
        /// </summary>
        /// <param name="expression">expression to get value from</param>
        /// <returns>constant value</returns>
        public static Maybe<object> GetDeepMemberConstValue(this Expression expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            return expression.GetDeepMemberConstExpression().Select(expr => expr.Value);
        }

        /// <summary> Returns constant value from expression with fall down to hierarchy
        /// </summary>
        /// <typeparam name="TValue">cast value to specified Type if possible</typeparam>
        /// <param name="expression">expression to get value from</param>
        /// <returns>constant value of specified Type</returns>
        public static Maybe<TValue> GetDeepMemberConstValue<TValue>(this Expression expression)
        {
            return GetDeepMemberConstValue(expression).Where(v => v is TValue).Select(v => (TValue)v);
        }

        internal static Func<object, object, object> GetBinaryMethod(this ExpressionType type)
        {
            switch (type)
            {
                case ExpressionType.Equal:
                    return (v1, v2) => object.Equals(v1, v2);

                case ExpressionType.NotEqual:
                    return (v1, v2) => !object.Equals(v1, v2);

                case ExpressionType.OrElse:
                    return (v1, v2) => ((bool)v1) || ((bool)v2);

                case ExpressionType.AndAlso:
                    return (v1, v2) => ((bool)v1) && ((bool)v2);

                default:
                    return null;
            }
        }

        public static Expression Override(this Expression baseExpression, Expression oldExpr, Expression newExpr)
        {
            if (baseExpression == null) throw new ArgumentNullException(nameof(baseExpression));
            if (oldExpr == null) throw new ArgumentNullException(nameof(oldExpr));
            if (newExpr == null) throw new ArgumentNullException(nameof(newExpr));

            return new OverrideExpressionVisitor(oldExpr, newExpr).Visit(baseExpression);
        }

        public static Expression GetBodyWithOverrideParameters(this LambdaExpression lambda, params Expression[] newExpressions)
        {
            if (lambda == null) throw new ArgumentNullException(nameof(lambda));
            if (newExpressions == null) throw new ArgumentNullException(nameof(newExpressions));

            var pairs = lambda.Parameters.ZipStrong(newExpressions, (parameter, newExpression) => new { Parameter = parameter, NewExpression = newExpression });


            return pairs.Aggregate(lambda.Body, (expr, pair) => expr.Override(pair.Parameter, pair.NewExpression));
        }

        public static Expression<TDelegate> OverrideStringEquality<TDelegate>(this Expression<TDelegate> expression, StringComparison stringComparison)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            return expression.UpdateBody(new OverrideStringEqualityExpressionVisitor(stringComparison));
        }

        public static IEnumerable<Expression> GetChildren(this MethodCallExpression expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            if (expression.Object != null)
            {
                yield return expression.Object;
            }

            foreach (var arg in expression.Arguments)
            {
                yield return arg;
            }
        }

        public static MethodCallExpression ToCallExpression(this MethodInfo methodInfo, IEnumerable<Expression> children)
        {
            if (methodInfo == null) throw new ArgumentNullException(nameof(methodInfo));
            if (children == null) throw new ArgumentNullException(nameof(children));

            if (methodInfo.IsStatic)
            {
                return Expression.Call(methodInfo, children);
            }
            else
            {
                return children.GetByFirst((first, other) => Expression.Call(first, methodInfo, other));
            }
        }

        public static Expression UpdateBase(this Expression source, IEnumerable<ExpressionVisitor> visitors)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (visitors == null) throw new ArgumentNullException(nameof(visitors));

            return visitors.Aggregate(source, (expr, visitor) => visitor.Visit(expr));
        }

        public static Expression UpdateBase(this Expression source, params ExpressionVisitor[] visitors)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (visitors == null) throw new ArgumentNullException(nameof(visitors));

            return source.UpdateBase((IEnumerable<ExpressionVisitor>)visitors);
        }

        /// <summary>
        /// Оборачиваем результат в Maybe<>, если он уже Maybe<>, то ничего не делаем
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static Expression SafeWrapToMaybe(this Expression expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            return expression.Type.IsMaybe() ? expression : expression.Return();
        }
    }
}
