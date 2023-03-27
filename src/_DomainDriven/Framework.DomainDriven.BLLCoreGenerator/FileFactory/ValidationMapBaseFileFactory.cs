using System.CodeDom;
using System.Linq.Expressions;
using System.Reflection;

using Framework.CodeDom;
using Framework.Core;
using Framework.Validation;

namespace Framework.DomainDriven.BLLCoreGenerator;

public class ValidationMapBaseFileFactory<TConfiguration> : FileFactory<TConfiguration>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    public ValidationMapBaseFileFactory(TConfiguration configuration)
            : base(configuration, null)
    {
    }

    public override FileType FileType { get; } = FileType.ValidationMapBase;

    protected virtual string ExternalClassValidatorsMethodName { get; } = "GetExternalClassValidators";

    protected virtual string ExternalPropertyValidatorsMethodName { get; } = "GetExternalPropertyValidators";

    protected override CodeTypeDeclaration GetCodeTypeDeclaration()
    {
        return new CodeTypeDeclaration
               {
                       Name = this.Name,
#pragma warning disable S3265 // Non-flags enums should not be used in bitwise operations
                       Attributes = MemberAttributes.Public | MemberAttributes.Abstract,
#pragma warning restore S3265 // Non-flags enums should not be used in bitwise operations
                       IsPartial = true,
               };
    }

    protected override IEnumerable<CodeTypeReference> GetBaseTypes()
    {
        yield return typeof(Validation.ValidationMapBase).ToTypeReference();
    }

    protected override IEnumerable<CodeTypeMember> GetMembers()
    {
        foreach (var member in base.GetMembers())
        {
            yield return member;
        }

        var defaultValidatorMapField = new CodeMemberField(typeof(IValidationMap), "_defaultValidatorMap");

        yield return defaultValidatorMapField;

        {
            var extendedValidationDataParam = typeof(IDynamicSource).ToTypeReference().ToParameterDeclarationExpression("extendedValidationData");

            yield return new CodeConstructor
                         {
                                 Attributes = MemberAttributes.Family,
                                 Parameters = { extendedValidationDataParam },
                                 BaseConstructorArgs = { extendedValidationDataParam.ToVariableReferenceExpression() },
                                 Statements = { typeof(ValidationMap).ToTypeReference().ToObjectCreateExpression(extendedValidationDataParam.ToVariableReferenceExpression()).ToAssignStatement(new CodeThisReferenceExpression().ToFieldReference(defaultValidatorMapField)) }
                         };
        }

        if (this.Configuration.GenerateExternalClassValidators)
        {
            yield return this.GetExternalClassValidatorsMethod();
        }

        if (this.Configuration.GenerateExternalPropertyValidators)
        {
            yield return this.GetExternalPropertyValidatorsMethod();
        }

        foreach (var domainType in this.Configuration.ValidationTypes)
        {
            var domainTypeValidatorGenerator = this.Configuration.GetValidatorGenerator(domainType, new CodeThisReferenceExpression());

            var getMethod = new CodeMemberMethod
                            {
                                    Attributes = MemberAttributes.Family,
                                    Name = $"Get{domainType.Name}ValidationMap",
                                    ReturnType = typeof(IClassValidationMap<>).ToTypeReference(domainType)
                            };

            if (this.IsEmptyClassMap(domainType, domainTypeValidatorGenerator))
            {
                yield return getMethod.WithStatement(typeof(ClassValidationMap<>).MakeGenericType(domainType).ToTypeReferenceExpression().ToFieldReference("Empty").ToMethodReturnStatement());
            }
            else
            {
                var getPropertiesMethod = this.GetClassPropertiesMethod(domainType, domainTypeValidatorGenerator);

                var getValidatorsMethod = this.SkipClassValidatorsGeneration(domainType, domainTypeValidatorGenerator.ClassValidators) ? null : this.GetClassValidatorsMethod(domainType, domainTypeValidatorGenerator.ClassValidators);

                yield return getMethod
                        .WithStatement(typeof(ClassValidationMap<>)
                                       .ToTypeReference(domainType)
                                       .ToObjectCreateExpression(new CodeExpression[] { new CodeThisReferenceExpression().ToMethodReferenceExpression(getPropertiesMethod.Name) }.Concat(getValidatorsMethod.Maybe(m => new CodeThisReferenceExpression().ToMethodInvokeExpression(m)).MaybeYield()).ToArray())
                                       .ToMethodReturnStatement());

                yield return getPropertiesMethod;

                if (getValidatorsMethod != null)
                {
                    yield return getValidatorsMethod;
                }
            }

            foreach (var propertyValidatorPair in domainTypeValidatorGenerator.PropertyValidators.OrderBy(pair => pair.Key.Name))
            {
                if (!this.SkipPropertyGeneration(propertyValidatorPair))
                {
                    yield return this.GetPropertyValidatorsMethod(domainType, propertyValidatorPair);
                }
            }
        }

        yield return this.GetInternalClassMapMethod(defaultValidatorMapField);
    }

    private bool IsEmptyClassMap(Type domainType, IValidatorGenerator domainTypeValidatorGenerator)
    {
        return domainTypeValidatorGenerator.PropertyValidators.All(this.SkipPropertyGeneration) && this.SkipClassValidatorsGeneration(domainType, domainTypeValidatorGenerator.ClassValidators);
    }

    private bool SkipPropertyGeneration(KeyValuePair<PropertyInfo, IReadOnlyDictionary<CodeExpression, IValidationData>> propertyValidatorPair)
    {
        return this.Configuration.SquashPropertyValidators(propertyValidatorPair.Key)
               && !this.Configuration.GenerateExternalPropertyValidators
               && propertyValidatorPair.Value.IsEmpty();
    }

    private bool SkipClassValidatorsGeneration(Type domainType, IReadOnlyDictionary<CodeExpression, IValidationData> classValidatorPair)
    {
        return this.Configuration.SquashClassValidators(domainType)
               && !this.Configuration.GenerateExternalClassValidators
               && classValidatorPair.IsEmpty();
    }

    private CodeMemberMethod GetInternalClassMapMethod(CodeMemberField defaultValidatorMapField)
    {
        if (defaultValidatorMapField == null) throw new ArgumentNullException(nameof(defaultValidatorMapField));

        var sourceParameter = new CodeTypeParameter("TSource");
        var sourceTypeRef = sourceParameter.ToTypeReference();

        var statementsRequest = from domainType in this.Configuration.ValidationTypes

                                let condition = new CodeValueEqualityOperatorExpression(sourceTypeRef.ToTypeOfExpression(), domainType.ToTypeOfExpression())

                                let statement = new CodeThisReferenceExpression().ToMethodInvokeExpression($"Get{domainType.Name}ValidationMap")
                                                                                 .ToCastExpression(typeof(IClassValidationMap<>).ToTypeReference(sourceTypeRef))
                                                                                 .ToMethodReturnStatement()

                                select Tuple.Create((CodeExpression)condition, (CodeStatement)statement);

        return new CodeMemberMethod
               {
#pragma warning disable S3265 // Non-flags enums should not be used in bitwise operations
                       Attributes = MemberAttributes.Family | MemberAttributes.Override,
#pragma warning restore S3265 // Non-flags enums should not be used in bitwise operations
                       Name = "GetInternalClassMap",
                       ReturnType = typeof(IClassValidationMap<>).ToTypeReference(sourceTypeRef),
                       TypeParameters = { sourceParameter },
                       Statements =
                       {
                               statementsRequest.ToSwitchExpressionStatement(

                                                                             new CodeThisReferenceExpression().ToFieldReference(defaultValidatorMapField)
                                                                                     .ToStaticMethodInvokeExpression(typeof(ValidationMapExtensions).ToTypeReferenceExpression().ToMethodReferenceExpression("GetClassMap", sourceParameter.ToTypeReference()))
                                                                                     .ToMethodReturnStatement())
                       }
               };
    }


    private CodeMemberMethod GetExternalClassValidatorsMethod()
    {
        var sourceParameter = new CodeTypeParameter("TSource");

        var classValidatorType = typeof(IClassValidator<>).ToTypeReference(sourceParameter.ToTypeReference());

        return new CodeMemberMethod
               {
                       Attributes = MemberAttributes.Family,
                       Name = this.ExternalClassValidatorsMethodName,
                       ReturnType = classValidatorType.ToEnumerableReference(),
                       TypeParameters = { sourceParameter },
                       Statements =
                       {
                               new CodeMethodYieldBreakStatement()
                       }
               };
    }

    private CodeMemberMethod GetExternalPropertyValidatorsMethod()
    {
        var sourceParameter = new CodeTypeParameter("TSource");
        var propertyParameter = new CodeTypeParameter("TProperty");

        var expressionParam = new CodeParameterDeclarationExpression(
                                                                     typeof(Expression<>).ToTypeReference(typeof(Func<,>).ToTypeReference(sourceParameter.ToTypeReference(), propertyParameter.ToTypeReference())),
                                                                     "propertyExpr");

        var propertyValidatorType = typeof(IPropertyValidator<,>).ToTypeReference(sourceParameter.ToTypeReference(), propertyParameter.ToTypeReference());

        return new CodeMemberMethod
               {
                       Attributes = MemberAttributes.Family,
                       Name = this.ExternalPropertyValidatorsMethodName,
                       ReturnType = propertyValidatorType.ToEnumerableReference(),
                       TypeParameters = { sourceParameter, propertyParameter },
                       Parameters = { expressionParam },
                       Statements =
                       {
                               new CodeMethodYieldBreakStatement()
                       }
               };
    }

    private CodeMemberMethod GetClassPropertiesMethod(Type domainType, IValidatorGenerator validatorGenerator)
    {
        if (domainType == null) throw new ArgumentNullException(nameof(domainType));
        if (validatorGenerator == null) throw new ArgumentNullException(nameof(validatorGenerator));

        var currentClassValidatorParam = typeof(IClassValidationMap<>).MakeGenericType(domainType).ToTypeReference().ToParameterDeclarationExpression("currentClass");

        var propertyValidatorExpressions = from propertyValidatorPair in validatorGenerator.PropertyValidators

                                           orderby propertyValidatorPair.Key.Name

                                           where !this.SkipPropertyGeneration(propertyValidatorPair)

                                           let property = propertyValidatorPair.Key

                                           let collectionElementType = property.PropertyType.GetCollectionElementType()

                                           let validatorType = collectionElementType != null
                                                                       ? typeof(CollectionPropertyValidationMap<,,>).MakeGenericType(domainType, property.PropertyType, collectionElementType)
                                                                       : typeof(SinglePropertyValidationMap<,>).MakeGenericType(domainType, property.PropertyType)

                                           select validatorType.ToTypeReference().ToObjectCreateExpression(

                                            property.ToCodeLambdaExpression(),

                                            currentClassValidatorParam.ToVariableReferenceExpression(),

                                            new CodeThisReferenceExpression().ToMethodInvokeExpression($"Get{domainType.Name}_{property.Name}Validators"),

                                            new CodeThisReferenceExpression().ToMethodReferenceExpression("GetClassMap", (collectionElementType ?? property.PropertyType).ToTypeReference()).ToMethodInvokeExpression(true.ToPrimitiveExpression()));

        return new CodeMemberMethod
               {
                       Attributes = MemberAttributes.Family,
                       Name = $"Get{domainType.Name}Properties",
                       ReturnType = typeof(IPropertyValidationMap<>).ToTypeReference(domainType).ToEnumerableReference(),
                       Parameters = { currentClassValidatorParam }
               }.WithYield(propertyValidatorExpressions);
    }

    private CodeMemberMethod GetClassValidatorsMethod(Type domainType, IReadOnlyDictionary<CodeExpression, IValidationData> classValidators)
    {
        if (domainType == null) throw new ArgumentNullException(nameof(domainType));

        var classValidatorType = typeof(IClassValidator<>).ToTypeReference(domainType);

        var method = new CodeMemberMethod
                     {
                             Attributes = MemberAttributes.Family,
                             Name = $"Get{domainType.Name}Validators",
                             ReturnType = classValidatorType.ToEnumerableReference()
                     };

        var getExternalExpr = new CodeThisReferenceExpression().ToMethodReferenceExpression(this.ExternalClassValidatorsMethodName, domainType.ToTypeReference()).ToMethodInvokeExpression();

        if (!classValidators.Any() && this.Configuration.GenerateExternalClassValidators)
        {
            return method.WithStatement(getExternalExpr.ToMethodReturnStatement());
        }
        else
        {
            var validatorsExpressions = from classValidatorExpressionPair in classValidators

                                        select TryApplyValidatorDataExpression(classValidatorExpressionPair.Key, classValidatorExpressionPair.Value, typeof(ClassValidatorExtensions));

            var getForeachStatement = FuncHelper.Create(() => new CodeParameterDeclarationExpression { Name = "classValidator" }.Pipe(iterator => new CodeForeachStatement
                                                            {
                                                                    Iterator = iterator,

                                                                    Source = getExternalExpr,

                                                                    Statements = { iterator.ToVariableReferenceExpression().ToMethodYieldReturnStatement() }
                                                            }));

            return method.WithStatement(this.Configuration.GenerateExternalClassValidators, getForeachStatement)
                         .WithYield(validatorsExpressions, false)
                         .WithTryBreak();
        }
    }

    private CodeMemberMethod GetPropertyValidatorsMethod(Type domainType, KeyValuePair<PropertyInfo, IReadOnlyDictionary<CodeExpression, IValidationData>> propertyValidatorPair)
    {
        if (domainType == null) throw new ArgumentNullException(nameof(domainType));

        var propValidatorType = typeof(IPropertyValidator<,>).ToTypeReference(domainType, propertyValidatorPair.Key.PropertyType);

        var validatorsExpressions = from propertyValidatorExpressionPair in propertyValidatorPair.Value

                                    select TryApplyValidatorDataExpression(propertyValidatorExpressionPair.Key, propertyValidatorExpressionPair.Value, typeof(PropertyValidatorExtensions));

        var property = propertyValidatorPair.Key;

        var getForeachStatement = FuncHelper.Create(() => new CodeParameterDeclarationExpression { Name = "propertyValidator" }.Pipe(iterator => new CodeForeachStatement
                                                        {
                                                                Iterator = iterator,

                                                                Source = new CodeThisReferenceExpression().ToMethodReferenceExpression(this.ExternalPropertyValidatorsMethodName, domainType.ToTypeReference(), property.PropertyType.ToTypeReference()).ToMethodInvokeExpression(property.ToCodeLambdaExpression()),

                                                                Statements = { iterator.ToVariableReferenceExpression().ToMethodYieldReturnStatement() }
                                                        }));

        return new CodeMemberMethod
               {
                       Attributes = MemberAttributes.Family,
                       Name = $"Get{domainType.Name}_{property.Name}Validators",
                       ReturnType = propValidatorType.ToEnumerableReference()
               }
               .WithStatement(this.Configuration.GenerateExternalPropertyValidators, getForeachStatement)
               .WithYield(validatorsExpressions, false)
               .WithTryBreak();
    }

    private static CodeExpression TryApplyValidatorDataExpression(CodeExpression codeExpression, IValidationData validationData, Type extensionsClass)
    {
        if (codeExpression == null) throw new ArgumentNullException(nameof(codeExpression));
        if (extensionsClass == null) throw new ArgumentNullException(nameof(extensionsClass));

        return GetApplyValidatorDataFunc(codeExpression, validationData, extensionsClass).Aggregate(codeExpression, (state, f) => f(state));
    }

    private static IEnumerable<Func<CodeExpression, CodeExpression>> GetApplyValidatorDataFunc(CodeExpression codeExpression, IValidationData validationData, Type extensionsClass)
    {
        if (codeExpression == null) throw new ArgumentNullException(nameof(codeExpression));
        if (extensionsClass == null) throw new ArgumentNullException(nameof(extensionsClass));

        if (validationData != null)
        {
            if (validationData.CustomError != null)
            {
                yield return state => extensionsClass.ToTypeReferenceExpression().ToMethodInvokeExpression("ApplyCustomError", state, validationData.CustomError.ToDynamicPrimitiveExpression());
            }

            if (validationData.OperationContext != int.MaxValue)
            {
                yield return state => extensionsClass.ToTypeReferenceExpression().ToMethodInvokeExpression("ApplyCustomOperationContext", state, validationData.OperationContext.ToPrimitiveExpression());
            }
        }
    }
}
