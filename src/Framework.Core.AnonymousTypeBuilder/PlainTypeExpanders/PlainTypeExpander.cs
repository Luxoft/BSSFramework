using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;

namespace Framework.Core
{
    public class PlainTypeExpander : IPlainTypeExpander
    {
        public readonly string PathSeparator;

        private readonly IAnonymousTypeBuilder<TypeMap> _anonymousTypeBuilder;


        public PlainTypeExpander(string pathSeparator, IAnonymousTypeBuilder<TypeMap> anonymousTypeBuilder)
        {
            if (pathSeparator == null) throw new ArgumentNullException(nameof(pathSeparator));
            if (anonymousTypeBuilder == null) throw new ArgumentNullException(nameof(anonymousTypeBuilder));

            this.PathSeparator = pathSeparator;
            this._anonymousTypeBuilder = anonymousTypeBuilder;
        }


        public IExpressionConverter GetExpressionConverter(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            var plainType = this.Expand(type);

            return new Func<IExpressionConverter<object, object>>(this.GetExpressionConverter<object, object>)
                  .CreateGenericMethod(type, plainType)
                  .Invoke<IExpressionConverter>(this);
        }

        protected virtual IExpressionConverter<TSource, TTarget> GetExpressionConverter<TSource, TTarget>()
        {
            return new PlainExpressionConverter<TSource, TTarget>(this);
        }



        private Type Expand(Type type)
        {
            var map = this.GetExpandMap(type);

            var propertyDict = map.ChangeValue(value => this.NormalizeType(value.Last().PropertyType));

            return this._anonymousTypeBuilder.GetAnonymousType(new TypeMap(type.Name, propertyDict));
        }

        private Dictionary<string, PropertyPath> GetExpandMap(Type type)
        {
            var paths = this.GetExpandToPlainElements(type).Select(path => path.ToPropertyPath());

            return paths.ToDictionary(path => path.Join(this.PathSeparator, prop => prop.Name), path => path);
        }



        private Type NormalizeType(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            var fullUnwrappedType = this.GetUnwrappedType(type, false);

            return this.GetWrappedType(fullUnwrappedType);
        }

        protected virtual Type GetUnwrappedType(Type type, bool strong)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            if (strong)
            {
                return type.GetNullableObjectElementTypeOrSelf();
            }
            else
            {
                return this.GetUnwrappedType(type.GetNullableElementTypeOrSelf(), true);
            }
        }

        protected virtual Type GetWrappedType(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            return typeof(NullableObject<>).MakeGenericType(type);
        }

        private IEnumerable<IEnumerable<PropertyInfo>> GetExpandToPlainElements(Type preType)
        {
            var type = this.GetUnwrappedType(preType, false);

            if (this.IsExpandable(type))
            {
                foreach (var property in this.GetTypeProperties(type))
                {
                    foreach (var propPath in this.GetExpandToPlainElements(property.PropertyType))
                    {
                        yield return new[] { property }.Concat(propPath);
                    }
                }
            }
            else
            {
                yield return Enumerable.Empty<PropertyInfo>();
            }
        }

        protected virtual bool IsExpandable(Type type)
        {
            return !type.IsPrimitiveType();
        }


        protected virtual IEnumerable<PropertyInfo> GetTypeProperties(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            var properties = from property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance)

                             where !property.GetIndexParameters().Any()

                             select property;

            if (type.HasAttribute<DataContractAttribute>())
            {
                return from property in properties

                       where property.HasAttribute<DataMemberAttribute>()

                       select property;
            }
            else
            {
                return properties;
            }
        }


        protected class PlainExpressionConverter<TSource, TTarget> : IExpressionConverter<TSource, TTarget>
        {
            private readonly PlainTypeExpander _expander;


            public PlainExpressionConverter(PlainTypeExpander expander)
            {
                if (expander == null) throw new ArgumentNullException(nameof(expander));

                this._expander = expander;
            }


            private Expression GetMemberInitExpression(Expression wrappedSource)
            {
                if (wrappedSource == null) throw new ArgumentNullException(nameof(wrappedSource));

                var expandMap = from mapPath in this._expander.GetExpandMap(typeof(TSource))

                                join targetProp in typeof(TTarget).GetProperties() on mapPath.Key equals targetProp.Name

                                select new { Key = targetProp, Value = mapPath.Value };

                var bindings = from expandPath in expandMap

                               let bindExpr = expandPath.Value.Aggregate(wrappedSource, this.GetBindExpresion)

                               select Expression.Bind(expandPath.Key, bindExpr);

                return Expression.MemberInit(Expression.New(typeof(TTarget)), bindings);
            }

            private LambdaExpression GetPipeBodyLambdaExpression()
            {
                var sourceWrappedParam = Expression.Parameter(this._expander.GetWrappedType(typeof(TSource)));

                var initExpr = this.GetMemberInitExpression(sourceWrappedParam);

                return Expression.Lambda(initExpr, sourceWrappedParam);
            }

            public Expression<Func<TSource, TTarget>> GetConvertExpression()
            {
                var sourceParam = Expression.Parameter(typeof(TSource));

                var wrappedSource = this.GetWrappedExpression(sourceParam);

                var args = new[] { wrappedSource.Type, typeof(TTarget) };

                var pipeMethod = new Func<object, Func<object, object>, object>(PipeObjectExtensions.Pipe).CreateGenericMethod(args);

                var pipeExpr = Expression.Call(pipeMethod, wrappedSource, this.GetPipeBodyLambdaExpression());

                return Expression.Lambda<Func<TSource, TTarget>>(pipeExpr, sourceParam);
            }


            protected virtual Expression GetWrappedExpression(Expression sourceExpression)
            {
                if (sourceExpression == null) throw new ArgumentNullException(nameof(sourceExpression));

                var type = sourceExpression.Type;

                if (type.IsNullableObject())
                {
                    return sourceExpression;
                }
                else if (type.IsClass)
                {
                    var method = new Func<object, NullableObject<object>>(NullableObject.OfReference).CreateGenericMethod(type);

                    return Expression.Call(method, sourceExpression);
                }
                else if (type.IsNullable())
                {
                    var method = new Func<Ignore?, NullableObject<Ignore>>(NullableObject.OfNullable).CreateGenericMethod(type.GetNullableElementType());

                    return Expression.Call(method, sourceExpression);
                }
                else if (type.IsValueType)
                {
                    var method = new Func<Ignore, NullableObject<Ignore>>(NullableObject.Return).CreateGenericMethod(type);

                    return Expression.Call(method, sourceExpression);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }


            protected virtual MethodInfo GetSelectManyGenericMethod()
            {
                return new Func<NullableObject<Ignore>, Func<Ignore, NullableObject<Ignore>>, NullableObject<Ignore>>(NullableObject.SelectMany)
                      .Method
                      .GetGenericMethodDefinition();
            }

            private Expression GetSelectManyExpression(Expression sourceExpression, LambdaExpression selectorExpression)
            {
                if (sourceExpression == null) throw new ArgumentNullException(nameof(sourceExpression));
                if (selectorExpression == null) throw new ArgumentNullException(nameof(selectorExpression));

                var sourceElementType = this._expander.GetUnwrappedType(sourceExpression.Type, true);

                var targetElementType = this._expander.GetUnwrappedType(selectorExpression.Body.Type, true);

                var selectManyMethod = this.GetSelectManyGenericMethod().MakeGenericMethod(sourceElementType, targetElementType);

                return Expression.Call(selectManyMethod, sourceExpression, selectorExpression);
            }

            private Expression GetBindExpresion(Expression sourceExpression, PropertyInfo prop)
            {
                var selectorParameter = Expression.Parameter(this._expander.GetUnwrappedType(sourceExpression.Type, true));

                var selectorBodyExpression = this.GetWrappedExpression(Expression.Property(selectorParameter, prop));

                var selectorExpression = Expression.Lambda(selectorBodyExpression, selectorParameter);

                return this.GetSelectManyExpression(sourceExpression, selectorExpression);
            }

            public LambdaExpression GetConvertExpressionBase()
            {
                return this.GetConvertExpression();
            }
        }
    }
}
