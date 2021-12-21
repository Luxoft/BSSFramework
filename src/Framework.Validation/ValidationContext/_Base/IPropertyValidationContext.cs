using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Core;

namespace Framework.Validation
{
    public interface IPropertyValidationContext<out TSource, out TProperty> : IValidationContext<TSource, IPropertyValidationMap>
    {
        TProperty Value { get; }
    }

    public static class PropertyValidationContextExtensions
    {
        /// <summary>
        /// Получение имени валидируемого свойства объекта
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="context">Контекст</param>
        /// <param name="withParent">Флаг разворачивания "наверх" стека валидации (цепочки expandable-свойств) и соединением их через '.'</param>
        /// <returns></returns>
        public static string GetPropertyName<TSource, TProperty>(this IPropertyValidationContext<TSource, TProperty> context, bool withParent = true)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            if (withParent)
            {
                return context.GetExpandPathStates().Join(".", state => state.PropertyMap.PropertyName);
            }
            else
            {
                return context.Map.PropertyName;
            }
        }

        /// <summary>
        /// Полученеи типа свойства
        /// </summary>
        /// <typeparam name="TSource">Тип объекта</typeparam>
        /// <typeparam name="TProperty">Тип свойства</typeparam>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetPropertyTypeName<TSource, TProperty>(this IPropertyValidationContext<TSource, TProperty> context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            return context.Map.PropertyTypeMap.TypeName;
        }

        /// <summary>
        /// Получение имени типа объекта валидации
        /// </summary>
        /// <typeparam name="TSource">Тип объекта</typeparam>
        /// <typeparam name="TProperty">Тип свойства</typeparam>
        /// <param name="context">Контекст</param>
        /// <param name="withParent">Флаг разворачивания "наверх" стека валидации (цепочки expandable-свойств) и выборкой самого самого верхнего объекта валидации</param>
        /// <returns></returns>
        public static string GetSourceTypeName<TSource, TProperty>(this IPropertyValidationContext<TSource, TProperty> context, bool withParent = true)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            if (withParent)
            {
                return context.GetExpandRootState().SourceTypeMap.TypeName;
            }
            else
            {
                return context.Map.ReflectedTypeMap.TypeName;
            }
        }

        public static object GetSource<TSource, TProperty>(this IPropertyValidationContext<TSource, TProperty> context, bool withExpand = true)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            if (withExpand)
            {
                return context.GetExpandRootState().Source;
            }
            else
            {
                return context.Source;
            }
        }

        public static IPropertyValidationContext<TExpectedSource, TExpectedProperty> Box<TBaseSource, TExpectedSource, TBaseProperty, TExpectedProperty>(this IPropertyValidationContext<TBaseSource, TBaseProperty> context)
            where TBaseProperty : TExpectedProperty
            where TBaseSource : TExpectedSource
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            return context.Cast<TBaseSource, TExpectedSource, TBaseProperty, TExpectedProperty>(v => v, v => v);
        }

        public static IPropertyValidationContext<TExpectedSource, TExpectedProperty> Cast<TBaseSource, TExpectedSource, TBaseProperty, TExpectedProperty>(this IPropertyValidationContext<TBaseSource, TBaseProperty> context, Func<TBaseSource, TExpectedSource> convertSource, Func<TBaseProperty, TExpectedProperty> convertProperty)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (convertSource == null) throw new ArgumentNullException(nameof(convertSource));
            if (convertProperty == null) throw new ArgumentNullException(nameof(convertProperty));

            return new BoxedPropertyValidationContext<TBaseSource, TExpectedSource, TBaseProperty, TExpectedProperty>(context, convertSource, convertProperty);
        }


        private static IValidationState GetExpandRootState<TSource, TProperty>(this IPropertyValidationContext<TSource, TProperty> context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            return context.ParentState.GetAllElements(state => state.Parent)
                                      .TakeWhile(state => state.PropertyMap.IsExpanded)
                                      .LastOrDefault()

                ?? new ValidationState(context.ParentState, context.Map, context.Source);
        }

        private static IEnumerable<IValidationState> GetExpandPathStates<TSource, TProperty>(this IPropertyValidationContext<TSource, TProperty> context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            return context.GetReverseExpandPathStates().Reverse();
        }

        private static IEnumerable<IValidationState> GetReverseExpandPathStates<TSource, TProperty>(this IPropertyValidationContext<TSource, TProperty> context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            yield return new ValidationState(context.ParentState, context.Map, context.Source);

            foreach (var state in context.ParentState.GetAllElements(state => state.Parent).TakeWhile(state => state.PropertyMap.IsExpanded))
            {
                yield return state;
            }
        }


        private class BoxedPropertyValidationContext<TBaseSource, TExpectedSource, TBaseProperty, TExpectedProperty> : IPropertyValidationContext<TExpectedSource, TExpectedProperty>
        {
            public BoxedPropertyValidationContext(IPropertyValidationContext<TBaseSource, TBaseProperty> baseContext, Func<TBaseSource, TExpectedSource> convertSource, Func<TBaseProperty, TExpectedProperty> convertProperty)
            {
                if (baseContext == null) throw new ArgumentNullException(nameof(baseContext));
                if (convertSource == null) throw new ArgumentNullException(nameof(convertSource));
                if (convertProperty == null) throw new ArgumentNullException(nameof(convertProperty));

                this.Validator = baseContext.Validator;
                this.OperationContext = baseContext.OperationContext;
                this.ParentState = baseContext.ParentState;
                this.Source = convertSource(baseContext.Source);
                this.Map = baseContext.Map;
                this.Value = convertProperty(baseContext.Value);
                this.ExtendedValidationData = baseContext.ExtendedValidationData;
            }


            public IValidator Validator { get; }

            public int OperationContext { get; }

            public IValidationState ParentState { get; }

            public TExpectedSource Source { get; }

            public IPropertyValidationMap Map { get; }

            public TExpectedProperty Value { get; }

            public IDynamicSource ExtendedValidationData { get; }
        }
    }
}