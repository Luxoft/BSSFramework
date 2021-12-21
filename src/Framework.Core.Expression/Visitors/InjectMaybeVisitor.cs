using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Framework.Core
{
    public class InjectMaybeVisitor : ExpressionVisitor
    {
        //private static readonly MethodInfo BoolOrMethodSimple = new Func<Maybe<bool>, Func<bool>, Maybe<bool>>(MaybeExtensions.Or).Method;

        //private static readonly MethodInfo BoolOrMethod = new Func<Maybe<bool>, Func<Maybe<bool>>, Maybe<bool>>(MaybeExtensions.Or).Method;

        private static readonly MethodInfo LogicOrMethod = new Func<Maybe<bool>, Func<Maybe<bool>>, Maybe<bool>>(MaybeExtensions.LogicOr).Method;

        private static readonly MethodInfo LogicAndMethod = new Func<Maybe<bool>, Func<Maybe<bool>>, Maybe<bool>>(MaybeExtensions.LogicAnd).Method;



        private static readonly MethodInfo SelectMethod = new Func<Maybe<object>, Func<object, object>, Maybe<object>>(MaybeExtensions.Select).Method.GetGenericMethodDefinition();

        private static readonly MethodInfo OfConditionMethod = new Func<bool, Func<Maybe<object>>, Func<Maybe<object>>, Maybe<object>>(Maybe.OfCondition).Method.GetGenericMethodDefinition();


        private InjectMaybeVisitor()
        {

        }


        public Expression<TDelegate> VisitAndGetValueOrDefault<TDelegate>(Expression<TDelegate> expr)
        {
            return (Expression<TDelegate>)this.VisitAndGetValueOrDefaultBase(expr);
        }



        public LambdaExpression VisitAndGetValueOrDefaultBase(LambdaExpression expr)
        {
            if (expr == null) throw new ArgumentNullException(nameof(expr));

            var newBody = this.Visit(expr.Body);

            return Expression.Lambda(newBody.TryGetValueOrDefault(), expr.Parameters);
        }



        protected override Expression VisitMember(MemberExpression node)
        {
            var baseVisitedExression = this.Visit(node.Expression);

            var isWrapped = baseVisitedExression.Type.IsMaybe();

            var isValueSource = node.Expression.Type.IsValueType;

            var param = Expression.Parameter(node.Expression.Type);


            if (isWrapped)
            {
                if (isValueSource)
                {
                    var nullableType = node.Expression.Type.GetNullableElementType();

                    if (nullableType != null && node.Member.Name == "Value")
                    {
                        return baseVisitedExression.OverrideSelect(v => v.Return());
                    }
                    else
                    {
                        var method = new Func<Maybe<object>, Func<object, object>, Maybe<object>>(MaybeExtensions.Select).CreateGenericMethod(param.Type, node.Type);

                        var lambda = Expression.Lambda(Expression.MakeMemberAccess(param, node.Member), param);

                        return Expression.Call(method, baseVisitedExression, lambda);
                    }
                }
                else
                {
                    var visitedExression = baseVisitedExression.OverrideSelect(ex => ex.Return());

                    var method = SelectMethod.MakeGenericMethod(param.Type, node.Type);

                    var lambda = Expression.Lambda(Expression.MakeMemberAccess(param, node.Member), param);

                    return Expression.Call(method, visitedExression, lambda);
                }
            }
            else
            {
                if (isValueSource)
                {
                    var nullableType = node.Expression.Type.GetNullableElementType();

                    if (nullableType != null && node.Member.Name == "Value")
                    {
                        return baseVisitedExression.OverrideSelect(v => v.Return());
                    }
                    else
                    {
                        return base.VisitMember(node);
                    }
                }
                else
                {
                    var visitedExression = baseVisitedExression.Return();

                    var method = new Func<Maybe<object>, Func<object, object>, Maybe<object>>(MaybeExtensions.Select).CreateGenericMethod(param.Type, node.Type);

                    var lambda = Expression.Lambda(Expression.MakeMemberAccess(param, node.Member), param);

                    return Expression.Call(method, visitedExression, lambda);
                }
            }
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            return node.GetChildren().Select(this.Visit).OverrideSelect(elements => node.Method.ToCallExpression(elements));
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            var visitedLeft = this.Visit(node.Left);
            var visitedRight = this.Visit(node.Right);

            if (node.Type == typeof (bool))
            {
                switch (node.NodeType)
                {
                    case ExpressionType.OrElse:
                    {
                        var wrappedLeft = visitedLeft.SafeWrapToMaybe();
                        var wrappedRight = Expression.Lambda(visitedRight.SafeWrapToMaybe());

                        if (visitedLeft.Type.IsMaybe() || visitedRight.Type.IsMaybe())
                        {
                            return Expression.Call(LogicOrMethod, wrappedLeft, wrappedRight);
                        }
                        else
                        {
                            return Expression.OrElse(visitedLeft, visitedRight);
                        }
                    }

                    case ExpressionType.AndAlso:
                    {
                        var wrappedLeft = visitedLeft.SafeWrapToMaybe();
                        var wrappedRight = Expression.Lambda(visitedRight.SafeWrapToMaybe());

                        if (visitedLeft.Type.IsMaybe() || visitedRight.Type.IsMaybe())
                        {
                            return Expression.Call(LogicAndMethod, wrappedLeft, wrappedRight);
                        }
                        else
                        {
                            return Expression.AndAlso(visitedLeft, visitedRight);
                        }
                    }

                    case ExpressionType.Equal:
                    {
                        var unwrappedLeft = visitedLeft.TryGetValueOrDefault();
                        var unwrappedRight = visitedRight.TryGetValueOrDefault();

                        return Expression.Equal(unwrappedLeft, unwrappedRight, false, node.Method);
                    }

                    case ExpressionType.NotEqual:
                    {
                        var unwrappedLeft = visitedLeft.TryGetValueOrDefault();
                        var unwrappedRight = visitedRight.TryGetValueOrDefault();

                        return Expression.NotEqual(unwrappedLeft, unwrappedRight, false, node.Method);
                    }
                }
            }

            return new[] { visitedLeft, visitedRight }.OverrideSelect(args =>
                    Expression.MakeBinary(node.NodeType, args[0], args[1], node.IsLiftedToNull, node.Method, node.Conversion));
        }

        protected override Expression VisitUnary(UnaryExpression node)
        {
            return this.Visit(node.Operand).OverrideSelect(res => Expression.MakeUnary(node.NodeType, res, node.Type, node.Method));
        }

        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            return this.VisitAndGetValueOrDefault(node);
        }

        protected override Expression VisitConditional(ConditionalExpression node)
        {
            var visitedTest = this.Visit(node.Test);
            var visitedIfTrue = this.Visit(node.IfTrue);
            var visitedIfFalse = this.Visit(node.IfFalse);

            return visitedTest.OverrideSelect(test =>
            {
                if (visitedIfTrue.Type.IsMaybe() || visitedIfFalse.Type.IsMaybe())
                {
                    var wrappedTrue = visitedIfTrue.SafeWrapToMaybe();
                    var wrappedFalse = visitedIfFalse.SafeWrapToMaybe();

                    return Expression.Call(OfConditionMethod.MakeGenericMethod(node.Type), test, Expression.Lambda(wrappedTrue), Expression.Lambda(wrappedFalse));
                }
                else
                {
                    return Expression.Condition(test, visitedIfTrue, visitedIfFalse, node.Type);
                }
            });
        }

        protected override Expression VisitNewArray(NewArrayExpression node)
        {
            return node.Expressions.Select(this.Visit).OverrideSelect(items =>

                Expression.NewArrayInit(node.Type.GetElementType(), items));
        }

        protected override Expression VisitNew(NewExpression node)
        {
            return node.Arguments.Select(this.Visit).OverrideSelect(args => Expression.New(node.Constructor, args));
        }

        protected override Expression VisitListInit(ListInitExpression node)
        {
            return new[] { node.NewExpression }.Concat(node.Initializers.SelectMany(i => i.Arguments)).OverrideSelect(innerSource =>
            {
                using (var enumerator = innerSource.Select(v => v).GetEnumerator())
                {
                    var newExpr = (NewExpression)enumerator.ReadSingle();

                    var initializers = node.Initializers.ToArray(i =>

                        i.Update(enumerator.ReadMany(i.Arguments.Count)));

                    if (enumerator.MoveNext())
                    {
                        throw new Exception("invalid map");
                    }

                    return node.Update(newExpr, initializers);
                }
            });
        }

        protected override Expression VisitMemberInit(MemberInitExpression node)
        {
            return new[] { node.NewExpression }.Concat(node.Bindings.SelectMany(ex => ex.GetMemberBindingExpresisons())).OverrideSelect(innerSource =>
            {
                using (var enumerator = innerSource.Select(v => v).GetEnumerator())
                {
                    var newExpr = (NewExpression)enumerator.ReadSingle();

                    var initializers = node.Bindings.ToArray(i => i.UpdateMemberBindingBase(enumerator));

                    if (enumerator.MoveNext())
                    {
                        throw new Exception("invalid map");
                    }

                    return node.Update(newExpr, initializers);
                }
            });
        }


        public static readonly InjectMaybeVisitor Value = new InjectMaybeVisitor();
    }
}
