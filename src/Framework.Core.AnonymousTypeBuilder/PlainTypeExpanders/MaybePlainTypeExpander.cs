using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Framework.Core;

public class MaybePlainTypeExpander : PlainTypeExpander
{
    public MaybePlainTypeExpander(string pathSeparator, IAnonymousTypeBuilder<TypeMap> anonymousTypeBuilder)
            : base(pathSeparator, anonymousTypeBuilder)
    {

    }


    protected override Type GetUnwrappedType(Type propertyType, bool strong)
    {
        return base.GetUnwrappedType(propertyType.GetMaybeElementTypeOrSelf(), strong);
    }

    protected override Type GetWrappedType(Type propertyType)
    {
        return typeof(Maybe<>).MakeGenericType(base.GetWrappedType(propertyType));
    }

    protected override IExpressionConverter<TSource, TTarget> GetExpressionConverter<TSource, TTarget>()
    {
        return new MaybePlainExpressionConverter<TSource, TTarget>(this);
    }


    protected class MaybePlainExpressionConverter<TSource, TTarget> : PlainExpressionConverter<TSource, TTarget>
    {
        public MaybePlainExpressionConverter(PlainTypeExpander expander)
                : base(expander)
        {

        }


        protected override Expression GetWrappedExpression(Expression sourceExpression)
        {
            if (sourceExpression == null) throw new ArgumentNullException(nameof(sourceExpression));

            var maybeElementType = sourceExpression.Type.GetMaybeElementType();

            if (maybeElementType == null)
            {
                var baseWrappedExpr = base.GetWrappedExpression(sourceExpression);

                var method = new Func<Ignore, Maybe<Ignore>>(Maybe.Return).CreateGenericMethod(baseWrappedExpr.Type);

                return Expression.Call(method, baseWrappedExpr);
            }
            else
            {
                if (maybeElementType.IsGenericTypeImplementation(typeof(NullableObject<>)))
                {
                    return sourceExpression;
                }
                else
                {
                    var selectMethod = new Func<Maybe<Ignore>, Func<Ignore, Ignore>, Maybe<Ignore>>(MaybeExtensions.Select)
                            .CreateGenericMethod(maybeElementType, typeof(NullableObject<>).MakeGenericType(maybeElementType));

                    var elementParam = Expression.Parameter(maybeElementType);

                    var baseWrappedExpr = base.GetWrappedExpression(elementParam);

                    var selectWrapLambda = Expression.Lambda(baseWrappedExpr, elementParam);

                    return Expression.Call(selectMethod, sourceExpression, selectWrapLambda);
                }
            }
        }

        protected override MethodInfo GetSelectManyGenericMethod()
        {
            return new Func<Maybe<NullableObject<Ignore>>, Func<Ignore, Maybe<NullableObject<Ignore>>>, Maybe<NullableObject<Ignore>>>(MaybeNullableObject.SelectMany)
                   .Method
                   .GetGenericMethodDefinition();
        }
    }
}
