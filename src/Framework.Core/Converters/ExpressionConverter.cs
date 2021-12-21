using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Framework.Core
{
    public abstract class ExpressionConverter<TSource, TTarget> : ExpressionConverter, IExpressionConverter<TSource, TTarget>
    {
        protected ExpressionConverter()
        {

        }


        protected override Type SourceType
        {
            get { return typeof(TSource); }
        }

        protected override Type TargetType
        {
            get { return typeof(TTarget); }
        }


        internal protected abstract ExpressionConverter<TSubSource, TSubTarget> GetSubConverter<TSubSource, TSubTarget>();


        protected override ExpressionConverter GetSubConverterBase(Type subSourceType, Type subTargetType)
        {
            var method = new Func<ExpressionConverter<object, object>>(this.GetSubConverter<object, object>).CreateGenericMethod(subSourceType, subTargetType);

            return (ExpressionConverter)method.Invoke(this, new object[0]);
        }

        public Expression<Func<TSource, TTarget>> GetConvertExpression()
        {
            return (Expression<Func<TSource, TTarget>>)this.GetConvertExpressionBase();
        }

        public override LambdaExpression GetConvertExpressionBase()
        {
            var sourceType = typeof(TSource);
            var targetType = typeof(TTarget);

            var sourceParam = Expression.Parameter(sourceType);

            var body = this.GetConvertExpressionBody(sourceParam);

            return Expression.Lambda(typeof(Func<,>).MakeGenericType(sourceType, targetType), body, sourceParam);
        }
    }




    public abstract class ExpressionConverter : IExpressionConverter
    {
        protected abstract Type SourceType { get; }

        protected abstract Type TargetType { get; }


        protected virtual bool WithReadOnly
        {
            get { return true; }
        }

        protected virtual IEnumerable<MemberInfo> GetMembers()
        {
            return from members in new IEnumerable<MemberInfo>[] { this.TargetType.GetFields(BindingFlags.Instance | BindingFlags.Public).Where(f => this.WithReadOnly || !f.IsInitOnly),

                                                                   this.TargetType.GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(p => this.WithReadOnly || p.HasSetMethod()) }

                   from member in members

                   select member;
        }

        protected virtual MethodInfo GetConvertCollectionMethod()
        {
            return this.GetInternalConvertCollectionMethod().GetGenericMethodDefinition();
        }

        private MethodInfo GetInternalConvertCollectionMethod()
        {
            if (this.TargetType.IsArray)
            {
                return new Func<IEnumerable<object>, Func<object, object>, object[]>(EnumerableExtensions.ToArray).Method;
            }

            var collectionType = this.TargetType.GetCollectionType();

            if (collectionType == typeof(ObservableCollection<>))
            {
                return new Func<IEnumerable<object>, Func<object, object>, ObservableCollection<object>>(EnumerableExtensions.ToObservableCollection).Method;
            }

            if (collectionType == typeof(List<>))
            {
                return new Func<IEnumerable<object>, Func<object, object>, List<object>>(EnumerableExtensions.ToList).Method;
            }

            return new Func<IEnumerable<object>, Func<object, object>, IEnumerable<object>>(Enumerable.Select).Method;
        }

        public abstract LambdaExpression GetConvertExpressionBase();

        protected abstract ExpressionConverter GetSubConverterBase(Type subSourceType, Type subTargetType);


        protected virtual Expression GetMemberBindExpression(MemberExpression sourcePropExpr, Type targetPropType)
        {
            if (sourcePropExpr == null) throw new ArgumentNullException(nameof(sourcePropExpr));

            return this.GetSubConverterBase(sourcePropExpr.Type, targetPropType).GetConvertExpressionBody(sourcePropExpr);
        }

        protected virtual MemberExpression GetSourceMemberExpression(Expression source, MemberInfo member)
        {
            return Expression.PropertyOrField(source, member.Name);
        }

        protected virtual Expression GetConvertExpressionBody(Expression sourceExpr)
        {
            if (sourceExpr == null) throw new ArgumentNullException(nameof(sourceExpr));

            if (sourceExpr.Type == this.TargetType)
            {
                return sourceExpr;
            }

            if (this.TargetType.IsCollectionOrArray())
            {
                var sourceElementType = sourceExpr.Type.GetCollectionOrArrayElementType();
                var targetElementType = this.TargetType.GetCollectionOrArrayElementType();

                var elementConvertLambda = this.GetSubConverterBase(sourceElementType, targetElementType).GetConvertExpressionBase();

                var convertMethod = this.GetConvertCollectionMethod().MakeGenericMethod(sourceElementType, targetElementType);

                return Expression.Call(null, convertMethod, sourceExpr, elementConvertLambda);
            }
            else
            {
                Func<Expression, Expression> getCreateObjExpr = inputSourceExpr =>
                {
                    var bindings = from member in this.GetMembers()

                                   let sourcePropExpr = this.GetSourceMemberExpression(inputSourceExpr, member)

                                   let targetPropType = Expression.PropertyOrField(Expression.Parameter(this.TargetType), member.Name).Type

                                   let bindExpr = this.GetMemberBindExpression(sourcePropExpr, targetPropType)

                                   select Expression.Bind(member, bindExpr);

                    return Expression.MemberInit(Expression.New(this.TargetType), bindings);
                };


                return this.SourceType.IsValueType
                     ? getCreateObjExpr(sourceExpr)
                     : Expression.Call(null,
                                       GenericMaybeMethod.MakeGenericMethod(this.SourceType, this.TargetType),
                                       sourceExpr,
                                       Expression.Parameter(this.SourceType).Pipe(param =>

                                            Expression.Lambda(getCreateObjExpr(param), param)));
            }
        }

        private static readonly MethodInfo GenericMaybeMethod = new Func<object, Func<object, object>, object>(PipeMaybeObjectExtensions.Maybe).Method.GetGenericMethodDefinition();
    }
}
