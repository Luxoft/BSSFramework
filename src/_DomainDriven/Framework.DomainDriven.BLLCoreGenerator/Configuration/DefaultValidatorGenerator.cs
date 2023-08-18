using System.CodeDom;
using System.Reflection;

using Framework.CodeDom;
using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.Generation.Domain;
using Framework.DomainDriven.Tracking.LegacyValidators;
using Framework.Persistent;
using Framework.Restriction;
using Framework.Validation;

using Microsoft.Extensions.DependencyInjection;

using ValidatorPairExpr = System.Collections.Generic.KeyValuePair<System.CodeDom.CodeExpression, Framework.Validation.IValidationData>;
using ValidatorExpr = System.Collections.Generic.IReadOnlyDictionary<System.CodeDom.CodeExpression, Framework.Validation.IValidationData>;

namespace Framework.DomainDriven.BLLCoreGenerator;

public class DefaultValidatorGenerator<TConfiguration> : GeneratorConfigurationContainer<TConfiguration>, IValidatorGenerator
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    protected readonly CodeExpression ValidatorMapExpr;

    protected readonly Type DomainType;

    private readonly IReadOnlyDictionary<PropertyInfo, IReadOnlyDictionary<CodeExpression, IValidationData>> _expandedClassAttributes;

    private static readonly IReadOnlyCollection<Type> ManyPropertyDynamicClassAttributes = new[]
        {
                typeof (DefaultStringMaxLengthValidatorAttribute),
                typeof (AvailableDateTimeValidatorAttribute),
                typeof (AvailablePeriodValidatorAttribute),
                typeof (AvailableDecimalValidatorAttribute)
        };


    public DefaultValidatorGenerator(TConfiguration configuration, Type domainType, CodeExpression validatorMapExpr)
            : base(configuration)
    {
        if (domainType == null) throw new ArgumentNullException(nameof(domainType));
        if (validatorMapExpr == null) throw new ArgumentNullException(nameof(validatorMapExpr));

        this.DomainType = domainType;
        this.ValidatorMapExpr = validatorMapExpr;

        this.ClassValidators = LazyInterfaceImplementHelper.CreateProxy(() => this.GetClassValidators().ToReadOnlyDictionaryI());
        this.PropertyValidators = domainType.GetValidationProperties().ToDictionary(property => property, property => LazyInterfaceImplementHelper.CreateProxy(() => this.GetPropertyValidators(property).ToReadOnlyDictionaryI()));

        this._expandedClassAttributes = LazyInterfaceImplementHelper.CreateProxy(this.ConvertClassAttributes);
    }




    public IReadOnlyDictionary<CodeExpression, IValidationData> ClassValidators { get; }

    public IReadOnlyDictionary<PropertyInfo, IReadOnlyDictionary<CodeExpression, IValidationData>> PropertyValidators { get; }

    protected virtual IEnumerable<ValidatorPairExpr> GetClassValidators()
    {
        if (typeof(IClassValidator<>).MakeGenericType(this.DomainType).IsAssignableFrom(this.DomainType))
        {
            yield return new ValidatorPairExpr(typeof(SelfClassValidator<>).ToTypeReference(this.DomainType).ToObjectCreateExpression(), null);
        }

        foreach (var attr in this.DomainType.GetCustomAttributes<ClassValidatorAttribute>())
        {
            if (!this.IsManyPropertyDynamicClassAttribute(attr))
            {
                var expr = this.ExpandClassAttributes(attr);

                yield return new ValidatorPairExpr(expr, attr);
            }
        }

        var attributes = this.DomainType
                             .TryGetRestrictionValidatorAttribute<UniqueGroupAttribute, UniDBGroupValidatorAttribute>(attr => new UniDBGroupValidatorAttribute { GroupKey = attr.Key, UseDbEvaluation = attr.UseDbEvaluation })
                             .Where(attr => attr.UseDbEvaluation || this.Configuration.UseDbUniquenessEvaluation);

        foreach (var attr in attributes)
        {
            var expr = this.ExpandClassAttributes(attr);
            yield return new ValidatorPairExpr(expr, attr);
        }
    }


    protected virtual IEnumerable<ValidatorPairExpr> GetPropertyValidators(PropertyInfo property)
    {
        if (property == null) throw new ArgumentNullException(nameof(property));

        if (!property.HasPrivateField(true) && !this.Configuration.AllowVirtualValidation(property))
        {
            yield break;
        }

        foreach (var expandedClassValidator in this._expandedClassAttributes.GetValueOrDefault(property, () => new Dictionary<CodeExpression, IValidationData>()))
        {
            yield return expandedClassValidator;
        }

        foreach (var attr in property.TryGetRestrictionValidatorAttributes().Concat(property.GetCustomAttributes<PropertyValidatorAttribute>()))
        {
            var expr = this.ExpandPropertyAttributes(property, attr);

            yield return new ValidatorPairExpr(expr, attr);
        }

        if (this.HasDeepValidation(property))
        {
            var collectionElementType = property.PropertyType.GetCollectionElementType();

            if (collectionElementType != null)
            {
                var expr = typeof(DeepCollectionValidator<,,>)
                           .MakeGenericType(this.DomainType, property.PropertyType, collectionElementType)
                           .ToTypeReference().ToObjectCreateExpression();

                yield return new ValidatorPairExpr(expr, null);
            }
            else
            {
                var expr = typeof(DeepSingleValidator<,>)
                           .MakeGenericType(this.DomainType, property.PropertyType)
                           .ToTypeReference().ToObjectCreateExpression();

                yield return new ValidatorPairExpr(expr, null);
            }
        }
    }

    protected virtual bool HasDeepValidation(PropertyInfo property)
    {
        return property.HasDeepValidation();
    }

    protected virtual bool IsManyPropertyDynamicClassAttribute(ClassValidatorAttribute attribute)
    {
        if (attribute == null) throw new ArgumentNullException(nameof(attribute));

        return ManyPropertyDynamicClassAttributes.Contains(attribute.GetType()); //&& !(attribute is IDynamicClassValidator);
    }

    protected virtual CodeExpression ExpandPropertyAttributes(PropertyInfo property, PropertyValidatorAttribute attribute)
    {
        switch (attribute)
        {
            case UniqueCollectionValidatorAttribute uniqueCollectionValidatorAttribute:
                return this.GetUniqueCollectionValidator(property, uniqueCollectionValidatorAttribute);

            case FixedPropertyValidatorAttribute fixedPropertyValidatorAttribute:
                return this.GetFixedPropertyValidator(property, fixedPropertyValidatorAttribute);

            default:
            {
                var autoExpr = this.TryAutoExpandPropertyAttributes(property, attribute);

                if (autoExpr != null)
                {
                    return autoExpr;
                }
                else
                {
                    throw new ArgumentOutOfRangeException(nameof(attribute));
                }
            }
        }
    }

    private CodeExpression TryAutoExpandPropertyAttributes(PropertyInfo property, PropertyValidatorAttribute attribute)
    {
        if (property == null) throw new ArgumentNullException(nameof(property));

        var instanceType = attribute.CreateValidator().GetLastPropertyValidator(property, new ServiceCollection().BuildServiceProvider()).GetType();

        if (instanceType.IsInterfaceImplementation(typeof(IPropertyValidator<,>)))
        {
            var attrProperties = attribute.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                          .Where(prop => !prop.GetIndexParameters().AnyA() && !prop.DeclaringType.IsAssignableFrom(typeof(ValidatorAttribute)))
                                          .ToDictionary(p => p, p => p.GetValue(attribute));

            var ctor = instanceType.GetConstructors().SingleMaybe().GetValueOrDefault();

            if (ctor != null)
            {
                var preArgs = ctor.GetParameters().ToDictionary(ctorP => ctorP, ctorP => attrProperties.Where(attrPair => attrPair.Key.Name.ToStartLowerCase() == ctorP.Name && attrPair.Key.PropertyType.IsAssignableFrom(ctorP.ParameterType))
                                                                                        .Select(attrPair => attrPair.Value).SingleMaybe());

                if (preArgs.Values.All(arg => arg.HasValue))
                {
                    var args = preArgs.ChangeValue(v => v.GetValue());

                    var createExpr = instanceType.ToTypeReference().ToObjectCreateExpression(ctor.GetParameters().ToArray(p => args[p].ToDynamicPrimitiveExpression()));

                    return this.TryUnboxProperty(createExpr, instanceType, property);
                }
            }
            else if (!attrProperties.Any())
            {
                var singletonProp = instanceType.GetProperties(BindingFlags.Public | BindingFlags.Static).Where(prop => prop.PropertyType == instanceType).SingleMaybe().GetValueOrDefault();

                if (singletonProp != null)
                {
                    var createExpr = instanceType.ToTypeReferenceExpression().ToPropertyReference(singletonProp);

                    return this.TryUnboxProperty(createExpr, instanceType, property);
                }
            }
        }

        return null;
    }

    private CodeExpression TryAutoExpandClassAttributes(ClassValidatorAttribute attribute)
    {
        if (attribute == null) throw new ArgumentNullException(nameof(attribute));

        var instanceType = attribute.CreateValidator().GetType();

        if (instanceType.IsInterfaceImplementation(typeof(IClassValidator<>)))
        {
            var attrProperties = attribute.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                          .Where(prop => !prop.GetIndexParameters().AnyA() && !prop.DeclaringType.IsAssignableFrom(typeof(ValidatorAttribute)))
                                          .ToDictionary(p => p, p => p.GetValue(attribute));

            var ctor = instanceType.GetConstructors().SingleMaybe().GetValueOrDefault();

            if (ctor != null)
            {
                var preArgs = ctor.GetParameters().ToDictionary(ctorP => ctorP, ctorP => attrProperties.Where(attrPair => attrPair.Key.Name.ToStartLowerCase() == ctorP.Name && attrPair.Key.PropertyType.IsAssignableFrom(ctorP.ParameterType))
                                                                                        .Select(attrPair => attrPair.Value).SingleMaybe());

                if (preArgs.Values.All(arg => arg.HasValue))
                {
                    var args = preArgs.ChangeValue(v => v.GetValue());

                    var createExpr = instanceType.ToTypeReference().ToObjectCreateExpression(ctor.GetParameters().ToArray(p => args[p].ToDynamicPrimitiveExpression()));

                    return this.TryUnboxClass(createExpr, instanceType);
                }
            }
            else if (!attrProperties.Any())
            {
                var singletonProp = instanceType.GetProperties(BindingFlags.Public | BindingFlags.Static).Where(prop => prop.PropertyType == instanceType).SingleMaybe().GetValueOrDefault();

                if (singletonProp != null)
                {
                    var createExpr = instanceType.ToTypeReferenceExpression().ToPropertyReference(singletonProp);

                    return this.TryUnboxClass(createExpr, instanceType);
                }
            }
        }

        return null;
    }

    protected CodeExpression TryUnboxProperty(CodeExpression expression, Type propertyValidatorType, PropertyInfo property)
    {
        if (expression == null) throw new ArgumentNullException(nameof(expression));
        if (propertyValidatorType == null) throw new ArgumentNullException(nameof(propertyValidatorType));
        if (property == null) throw new ArgumentNullException(nameof(property));

        var expectedType = typeof(IPropertyValidator<,>).MakeGenericType(this.DomainType, property.PropertyType);

        if (!expectedType.IsAssignableFrom(propertyValidatorType))
        {
            var args = propertyValidatorType.GetInterfaceImplementationArguments(typeof(IPropertyValidator<,>));

            if (args != null)
            {
                var validatorSourceType = args[0];
                var validatorPropertyType = args[1];

                if (validatorSourceType.IsAssignableFrom(this.DomainType) && validatorPropertyType.IsAssignableFrom(property.PropertyType))
                {
                    var methodRef = typeof(PropertyValidatorExtensions).ToTypeReferenceExpression().ToMethodReferenceExpression("Unbox", new[] { this.DomainType, validatorSourceType, property.PropertyType, validatorPropertyType });

                    return expression.ToStaticMethodInvokeExpression(methodRef);
                }
            }
        }

        return expression;
    }

    /// <summary>
    /// Вовзращает выражение, содержащее вызов Unbox для переданного classValidatorType.
    /// </summary>
    /// <param name="expression">Исходное выражение.</param>
    /// <param name="classValidatorType">Тип валидатора.</param>
    /// <returns>Экземпляр <see cref="CodeExpression"/>.</returns>
    /// <exception cref="ArgumentNullException">Аргумент
    /// <paramref name="expression"/>
    /// или
    /// <paramref name="classValidatorType"/> равен null.
    /// </exception>
    protected CodeExpression TryUnboxClass(CodeExpression expression, Type classValidatorType)
    {
        if (expression == null) throw new ArgumentNullException(nameof(expression));
        if (classValidatorType == null) throw new ArgumentNullException(nameof(classValidatorType));

        var expectedType = typeof(IClassValidator<>).MakeGenericType(this.DomainType);

        if (!expectedType.IsAssignableFrom(classValidatorType))
        {
            var args = classValidatorType.GetInterfaceImplementationArguments(typeof(IClassValidator<>));

            if (args != null)
            {
                var validatorSourceType = args[0];

                if (validatorSourceType.IsAssignableFrom(this.DomainType))
                {
                    var methodRef = typeof(ClassValidatorExtensions).ToTypeReferenceExpression().ToMethodReferenceExpression("Unbox", new[] { this.DomainType, validatorSourceType });

                    return expression.ToStaticMethodInvokeExpression(methodRef);
                }
            }
        }

        return expression;
    }

    private CodeExpression GetUniqueCollectionValidator(PropertyInfo property, UniqueCollectionValidatorAttribute attribute)
    {
        if (attribute == null) throw new ArgumentNullException(nameof(attribute));

        var elementType = property.PropertyType.GetCollectionElementType();

        var uniProperties = property.GetUniqueElementPropeties(attribute.GroupKey, true);

        var groupElementType = typeof(Tuple<>).Assembly
                                              .GetType(typeof(Tuple<>).FullName.SkipLast("1", true) + uniProperties.Length, true)
                                              .MakeGenericType(uniProperties.ToArray(p => p.PropertyType));

        var internalPropertyValidatorType = typeof(UniqueCollectionValidator<,,,>).MakeGenericType(property.ReflectedType, property.PropertyType, elementType, groupElementType);


        var trimNullMethod = typeof(Core.StringExtensions)
                             .ToTypeReferenceExpression()
                             .ToMethodReferenceExpression("TrimNull");


        var createTupleExpr = new CodeParameterDeclarationExpression { Name = "source" }.Pipe(param => new CodeLambdaExpression
            {
                    Parameters = { param },

                    Statements = { groupElementType.ToTypeReference().ToObjectCreateExpression(

                                        uniProperties.ToArray(prop => param.ToVariableReferenceExpression()
                                                                           .ToPropertyReference(prop)
                                                                           .Pipe(prop.PropertyType == typeof(string), expr => (CodeExpression)expr.ToStaticMethodInvokeExpression(trimNullMethod).ToMethodInvokeExpression("ToLower")))


                                       ).ToMethodReturnStatement() }

            });

        return internalPropertyValidatorType
               .ToTypeReference()
               .ToObjectCreateExpression(createTupleExpr, uniProperties.GetUniqueElementString(false).ToPrimitiveExpression());
    }

    private CodeExpression GetFixedPropertyValidator(PropertyInfo property, FixedPropertyValidatorAttribute attribute)
    {
        if (attribute == null) throw new ArgumentNullException(nameof(attribute));

        var identType = property.DeclaringType.GetIdentType();

        var validatorType = typeof(FixedPropertyValidator<,,,>).MakeGenericType(property.ReflectedType, property.PropertyType, identType, this.Configuration.Environment.PersistentDomainObjectBaseType);

        var propertyExprLambda = new CodeParameterDeclarationExpression { Name = "source" }.Pipe(p => new CodeLambdaExpression
            {
                    Parameters = { p },
                    Statements = { p.ToVariableReferenceExpression().ToPropertyReference(property) }
            });

        return validatorType.ToTypeReference().ToObjectCreateExpression(propertyExprLambda);
    }

    protected virtual CodeExpression ExpandClassAttributes(ClassValidatorAttribute attribute)
    {
        if (attribute is Framework.Validation.RequiredGroupValidatorAttribute)
        {
            return this.GetRequiredGroupValidator(attribute as RequiredGroupValidatorAttribute);
        }
        else if (attribute is UniDBGroupValidatorAttribute)
        {
            return this.GetUniDbGroupValidator(attribute as UniDBGroupValidatorAttribute);
        }
        else
        {
            var autoExpr = this.TryAutoExpandClassAttributes(attribute);

            if (autoExpr != null)
            {
                return autoExpr;
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(attribute));
            }
        }
    }

    private static CodeLambdaExpression GetGetPropertyValuesExpression(Type domainObjectType, IEnumerable<PropertyInfo> properties)
    {
        if (domainObjectType == null) throw new ArgumentNullException(nameof(domainObjectType));
        if (properties == null) throw new ArgumentNullException(nameof(properties));

        var domainObjectParameter = new CodeParameterDeclarationExpression { Name = "source" };


        var newValuesArrExpr = new CodeArrayCreateExpression(typeof(object).ToTypeReference(), properties.ToArray(property =>

                                                                     domainObjectParameter.ToVariableReferenceExpression().ToPropertyReference(property)));

        return new CodeLambdaExpression
               {
                       Parameters = { domainObjectParameter },
                       Statements = { newValuesArrExpr }
               };
    }

    private static CodeLambdaExpression GetGetFilterExpression(Type domainObjectType, Type identType, IEnumerable<PropertyInfo> properties)
    {
        if (domainObjectType == null) throw new ArgumentNullException(nameof(domainObjectType));
        if (identType == null) throw new ArgumentNullException(nameof(identType));
        if (properties == null) throw new ArgumentNullException(nameof(properties));

        var idProp = typeof(IIdentityObject<>).MakeGenericType(identType).GetProperties().Single();

        var sourceDomainObjectParameter = new CodeParameterDeclarationExpression { Name = "source" };

        var filterDomainObjectParameter = new CodeParameterDeclarationExpression { Name = "target" };

        Func<PropertyInfo, Func<CodeExpression, CodeExpression, CodeExpression>, CodeExpression> getBinaryExpr = (property, buildFunc) =>

                buildFunc(sourceDomainObjectParameter.ToVariableReferenceExpression().ToPropertyReference(property), filterDomainObjectParameter.ToVariableReferenceExpression().ToPropertyReference(property));


        var duplicateFilter = properties.Aggregate(

                                                   getBinaryExpr(idProp, (exp1, exp2) => new CodeBinaryOperatorExpression(exp1, CodeBinaryOperatorType.ValueEquality, exp2).ToNegateExpression()),

                                                   (filter, property) => new CodeBinaryOperatorExpression(filter, CodeBinaryOperatorType.BooleanAnd, getBinaryExpr(property, (e1, e2) => new CodeBinaryOperatorExpression(e1, CodeBinaryOperatorType.ValueEquality, e2))));


        var duplicateLambda = new CodeLambdaExpression
                              {
                                      Parameters = { filterDomainObjectParameter },
                                      Statements = { duplicateFilter }
                              };

        return new CodeLambdaExpression
               {
                       Parameters = { sourceDomainObjectParameter },
                       Statements = { duplicateLambda }
               };
    }

    private CodeExpression GetUniDbGroupValidator(UniDBGroupValidatorAttribute attribute)
    {
        if (attribute == null) throw new ArgumentNullException(nameof(attribute));

        var uniProperties = this.DomainType.GetUniqueElementPropeties(attribute.GroupKey, true);
        var uniqueElementString = uniProperties.GetUniqueElementString(false);

        var internalValidatorType = typeof(UniqueGroupDatabaseValidator<,,,>).ToTypeReference(
         this.Configuration.BLLContextInterfaceTypeReference,
         this.Configuration.Environment.PersistentDomainObjectBaseType.ToTypeReference(),
         this.DomainType.ToTypeReference(),
         this.Configuration.Environment.GetIdentityType().ToTypeReference());

        var getFilterExpressionLambda = GetGetFilterExpression(this.DomainType, this.Configuration.Environment.GetIdentityType(), uniProperties);

        var getPropertyValuesLambda = GetGetPropertyValuesExpression(this.DomainType, uniProperties);

        return internalValidatorType.ToObjectCreateExpression(getFilterExpressionLambda, getPropertyValuesLambda, uniqueElementString.ToPrimitiveExpression());
    }

    private CodeExpression GetRequiredGroupValidator(RequiredGroupValidatorAttribute attribute)
    {
        if (attribute == null) throw new ArgumentNullException(nameof(attribute));

        var uniProperties = this.DomainType.GetUniqueElementPropeties(attribute.GroupKey, true);

        var uniqueElementString = uniProperties.GetUniqueElementString(false);

        var sourceParam = new CodeParameterDeclarationExpression { Name = "source" };

        var propertyValidatorDict = uniProperties.ToDictionary(property => property.Name, property => new CodeLambdaExpression
                                                                   {
                                                                           Parameters = { sourceParam },

                                                                           Statements =
                                                                           {

                                                                                   typeof(RequiredHelper).ToTypeReferenceExpression().ToMethodInvokeExpression("IsValid",
                                                                                       sourceParam.ToVariableReferenceExpression().ToPropertyReference(property),
                                                                                       RequiredMode.Default.ToPrimitiveExpression())
                                                                           }
                                                                   });



        var keyPairType = typeof(KeyValuePair<,>).ToTypeReference(typeof(string).ToTypeReference(), typeof(Func<,>).ToTypeReference(this.DomainType, typeof(bool)));

        return typeof(RequiredGroupValidator<>)
               .ToTypeReference(this.DomainType)
               .ToObjectCreateExpression(attribute.Mode.ToPrimitiveExpression(),

                                         typeof(DictionaryHelper).ToTypeReferenceExpression()
                                                                 .ToMethodReferenceExpression("Create", typeof(string).ToTypeReference(), typeof(Func<,>).ToTypeReference(this.DomainType, typeof(bool)))
                                                                 .ToMethodInvokeExpression(propertyValidatorDict.ToArray(pair => keyPairType.ToObjectCreateExpression(pair.Key.ToPrimitiveExpression(), pair.Value))),

                                         uniqueElementString.ToPrimitiveExpression());
    }

    private IReadOnlyDictionary<PropertyInfo, ValidatorExpr> ConvertClassAttributes()
    {
        var request = from attr in this.DomainType.GetCustomAttributes<ClassValidatorAttribute>()

                      where this.IsManyPropertyDynamicClassAttribute(attr)

                      from pair in this.ConvertClassAttribute(attr)

                      group new ValidatorPairExpr(pair.Value, attr) by pair.Key;

        return request.ToDictionary(g => g.Key, g => g.ToReadOnlyDictionaryI());
    }

    protected virtual IReadOnlyDictionary<PropertyInfo, CodeExpression> ConvertClassAttribute(ClassValidatorAttribute attribute)
    {
        if (attribute == null) throw new ArgumentNullException(nameof(attribute));

        if (attribute is DefaultStringMaxLengthValidatorAttribute)
        {
            var request = from property in this.DomainType.GetValidationProperties()

                          where property.PropertyType == typeof(string)

                                && !property.HasAttribute<MaxLengthAttribute>()

                                && !property.HasAttribute<MaxLengthValidatorAttribute>()

                          select property;

            return request.ToDictionary(
                                        property => property,

                                        property => (CodeExpression)typeof(MaxLengthValidator.StringMaxLengthValidator<>)
                                                                    .ToTypeReference(this.DomainType)
                                                                    .ToObjectCreateExpression(this.ValidatorMapExpr.ToPropertyReference("AvailableValues").ToMethodReferenceExpression("GetAvailableSize", typeof(string).ToTypeReference()).ToMethodInvokeExpression()));
        }
        else if (attribute is AvailableDateTimeValidatorAttribute || attribute is AvailablePeriodValidatorAttribute || attribute is AvailableDecimalValidatorAttribute)
        {
            var propType = attribute is AvailableDateTimeValidatorAttribute ? typeof(DateTime)
                           : attribute is AvailablePeriodValidatorAttribute ? typeof(Period)
                           : typeof(decimal);

            var nullPropType = typeof(Nullable<>).MakeGenericType(propType);


            var request = from property in this.DomainType.GetValidationProperties()

                          where property.PropertyType == propType || property.PropertyType == nullPropType

                          select property;


            return request.ToDictionary(property => property, property =>

                                                                      (CodeExpression)typeof(RangePropertyValidatorHelper)
                                                                                      .ToTypeReferenceExpression()
                                                                                      .ToPropertyReference(propType.Name)
                                                                                      .ToMethodReferenceExpression(property.PropertyType.IsNullable() ? "CreateNullable" : "Create", this.DomainType.ToTypeReference())
                                                                                      .ToMethodInvokeExpression(this.ValidatorMapExpr.ToPropertyReference("AvailableValues").ToMethodReferenceExpression("GetAvailableRange", (propType == typeof(Period) ? typeof(DateTime) : propType).ToTypeReference()).ToMethodInvokeExpression())
                                       );
        }
        else
        {
            throw new ArgumentOutOfRangeException(nameof(attribute));
        }
    }
}
