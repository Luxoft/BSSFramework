using System.CodeDom;
using System.Collections.ObjectModel;
using System.Reflection;

using Framework.CodeDom;
using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;
using Framework.DomainDriven.Generation.Domain;
using Framework.Persistent;
using Framework.Projection;
using Framework.Restriction;
using Framework.Security;
using Framework.SecuritySystem;
using Framework.Transfering;
using Framework.Validation;

#pragma warning disable S100 // Methods and properties should be named in camel case
namespace Framework.DomainDriven.BLLCoreGenerator;

public abstract class GeneratorConfigurationBase<TEnvironment> : GeneratorConfiguration<TEnvironment, FileType>, IGeneratorConfigurationBase<TEnvironment>
        where TEnvironment : class, IGenerationEnvironmentBase
{
    private readonly Lazy<ReadOnlyCollection<Type>> lazySecurityServiceDomainTypes;

    private readonly Lazy<ReadOnlyCollection<Type>> lazyBLLDomainTypes;

    private readonly Lazy<IRootSecurityServiceGenerator> lazyRootSecurityServerGenerator;

    private readonly Lazy<ReadOnlyCollection<Type>> lazyValidationTypes;

    private readonly IDictionaryCache<Type, IReadOnlyList<CodeTypeParameter>> domainTypeSecurityParameters;

    protected GeneratorConfigurationBase(TEnvironment environment)
            : base(environment)
    {
        this.lazySecurityServiceDomainTypes = LazyHelper.Create(() => this.GetSecurityServiceDomainTypes().ToReadOnlyCollection());

        this.lazyBLLDomainTypes = LazyHelper.Create(() => this.GetBLLDomainTypes().ToReadOnlyCollection());

        this.lazyRootSecurityServerGenerator = LazyHelper.Create(this.GetRootSecurityServerGenerator);

        this.lazyValidationTypes = LazyHelper.Create(() => this.GetValidationTypes().ToReadOnlyCollection());

        this.Logics = LazyInterfaceImplementHelper.CreateProxy(this.GetLogics);

        IFetchPathFactory<FetchBuildRule.DTOFetchBuildRule> factory = new ExpandFetchPathFactory(this.Environment.PersistentDomainObjectBaseType);

        this.FetchPathFactory = factory.WithCompress();

        this.domainTypeSecurityParameters = new DictionaryCache<Type, IReadOnlyList<CodeTypeParameter>>(domainType => this.GetInternalDomainTypeSecurityParameters(domainType).ToArray()).WithLock();
    }

    /// <inheritdoc />
    public virtual bool GenerateExternalPropertyValidators { get; } = false;

    /// <inheritdoc />
    public virtual bool GenerateExternalClassValidators { get; } = false;

    public ReadOnlyCollection<Type> ValidationTypes => this.lazyValidationTypes.Value;

    public virtual IBLLFactoryContainerInterfaceGeneratorConfiguration Logics { get; }

    public virtual bool GenerateValidationMap => true;

    public virtual bool GenerateValidator => true;

    public virtual bool GenerateFetchService => true;

    public virtual bool UseRemoveMappingExtension => true;

    public Type DefaultBLLFactoryContainerType => typeof(IBLLFactoryContainer<>).MakeGenericType(this.DefaultBLLFactoryType);

    public virtual Type SecurityBLLFactoryType => typeof(IDefaultSecurityBLLFactory<,,>).MakeGenericType(this.Environment.PersistentDomainObjectBaseType, this.Environment.SecurityOperationCodeType, this.Environment.GetIdentityType());

    public Type SecurityBLLFactoryContainerType => typeof(IBLLFactoryContainer<>).MakeGenericType(this.SecurityBLLFactoryType);

    protected override string NamespacePostfix { get; } = "BLL";

    public virtual string GetOperationByCodeMethodName { get; } = "GetByCode";

    public virtual string GetOperationByModeMethodName { get; } = "GetByMode";

    public ReadOnlyCollection<Type> SecurityServiceDomainTypes => this.lazySecurityServiceDomainTypes.Value;

    public ReadOnlyCollection<Type> BLLDomainTypes => this.lazyBLLDomainTypes.Value;

    public virtual IFetchPathFactory<FetchBuildRule.DTOFetchBuildRule> FetchPathFactory { get; }

    /// <inheritdoc />
    public virtual bool UseDbUniquenessEvaluation { get; } = false;

    public virtual string IntegrationSaveMethodName { get; } = "IntegrationSave";

    protected virtual ICodeFileFactoryHeader<FileType> BLLContextFileFactoryHeader =>

            FileType.BLLContext.ToHeader(this.Environment.TargetSystemName);

    protected virtual ICodeFileFactoryHeader<FileType> BLLContextInterfaceFileFactoryHeader =>

            new CodeFileFactoryHeader<FileType>(FileType.BLLContextInterface, string.Empty, _ => $"I{this.Environment.TargetSystemName}{FileType.BLLContext}");

    protected virtual ICodeFileFactoryHeader<FileType> DomainObjectMappingServiceFileFactoryHeader =>

            FileType.DomainObjectMappingService.ToHeader(this.Environment.TargetSystemName);

    protected virtual ICodeFileFactoryHeader<FileType> RootSecurityServiceInterfaceFileFactoryHeader =>

            new CodeFileFactoryHeader<FileType>(FileType.RootSecurityServiceInterface, string.Empty, _ => $"I{this.Environment.TargetSystemName}SecurityService");

    protected virtual ICodeFileFactoryHeader<FileType> RootSecurityServicePathContainerInterfaceFileFactoryHeader =>

            new CodeFileFactoryHeader<FileType>(FileType.RootSecurityServicePathContainerInterface, string.Empty, _ => $"I{this.Environment.TargetSystemName}SecurityPathContainer");


    protected virtual ICodeFileFactoryHeader<FileType> RootSecurityServiceFileFactoryHeader =>

            new CodeFileFactoryHeader<FileType>(FileType.RootSecurityService, string.Empty, _ => $"{this.Environment.TargetSystemName}SecurityService");

    protected virtual ICodeFileFactoryHeader<FileType> RootSecurityServiceBaseFileFactoryHeader =>

            new CodeFileFactoryHeader<FileType>(FileType.RootSecurityServiceBase, string.Empty, _ => $"{this.Environment.TargetSystemName}SecurityServiceBase");

    protected virtual ICodeFileFactoryHeader<FileType> SecurityOperationFileFactoryHeader =>

            FileType.SecurityOperation.ToHeader(this.Environment.TargetSystemName);

    protected virtual ICodeFileFactoryHeader<FileType> DomainSecurityServiceFileFactoryHeader =>

            new CodeFileFactoryHeader<FileType>(FileType.DomainSecurityService, string.Empty, domainType => $"{this.Environment.TargetSystemName}{domainType.Name}SecurityService");

    protected virtual ICodeFileFactoryHeader<FileType> DomainBLLBaseFileFactoryHeader { get; } =

        new CodeFileFactoryHeader<FileType>(FileType.DomainBLLBase, string.Empty, _ => FileType.DomainBLLBase.ToString());

    protected virtual ICodeFileFactoryHeader<FileType> DefaultOperationDomainBLLBaseFileFactoryHeader { get; } =

        new CodeFileFactoryHeader<FileType>(FileType.DefaultOperationDomainBLLBase, string.Empty, _ => FileType.DomainBLLBase.ToString());

    protected virtual ICodeFileFactoryHeader<FileType> SecurityDomainBLLBaseFileFactoryHeader { get; } =

        new CodeFileFactoryHeader<FileType>(FileType.SecurityDomainBLLBase, string.Empty, _ => FileType.SecurityDomainBLLBase.ToString());

    protected virtual ICodeFileFactoryHeader<FileType> SecurityHierarchyDomainBLLBaseFileFactoryHeader { get; } =

        new CodeFileFactoryHeader<FileType>(FileType.SecurityHierarchyDomainBLLBase, string.Empty, _ => FileType.SecurityHierarchyDomainBLLBase.ToString());

    protected virtual ICodeFileFactoryHeader<FileType> DefaultOperationSecurityDomainBLLBaseFileFactoryHeader { get; } =

        new CodeFileFactoryHeader<FileType>(FileType.DefaultOperationSecurityDomainBLLBase, string.Empty, _ => FileType.SecurityDomainBLLBase.ToString());
    
    protected virtual ICodeFileFactoryHeader<FileType> BLLInterfaceFileFactoryHeader { get; } =

        new CodeFileFactoryHeader<FileType>(FileType.BLLInterface, string.Empty, domainType => $"I{domainType.Name}BLL");

    protected virtual ICodeFileFactoryHeader<FileType> BLLFactoryInterfaceFileFactoryHeader { get; } =

        new CodeFileFactoryHeader<FileType>(FileType.BLLFactoryInterface, string.Empty, domainType => $"I{domainType.Name}BLLFactory");

    protected virtual ICodeFileFactoryHeader<FileType> BLLFactoryContainerInterfaceFileFactoryHeader =>

            new CodeFileFactoryHeader<FileType>(FileType.BLLFactoryContainerInterface, string.Empty, _ => $"I{this.Environment.TargetSystemName}BLLFactoryContainer");

    protected virtual ICodeFileFactoryHeader<FileType> ValidationMapFileFactoryHeader =>

            new CodeFileFactoryHeader<FileType>(FileType.ValidationMap, string.Empty, _ => $"{this.Environment.TargetSystemName}{FileType.ValidationMap}");

    protected virtual ICodeFileFactoryHeader<FileType> ValidationMapBaseFileFactoryHeader =>

            new CodeFileFactoryHeader<FileType>(FileType.ValidationMapBase, string.Empty, _ => $"{this.Environment.TargetSystemName}{FileType.ValidationMapBase}");

    protected virtual ICodeFileFactoryHeader<FileType> ValidatorFileFactoryHeader =>

            new CodeFileFactoryHeader<FileType>(FileType.Validator, string.Empty, _ => $"{this.Environment.TargetSystemName}{FileType.Validator}");

    protected virtual ICodeFileFactoryHeader<FileType> ValidatorBaseFileFactoryHeader =>

            new CodeFileFactoryHeader<FileType>(FileType.ValidatorBase, string.Empty, _ => $"{this.Environment.TargetSystemName}{FileType.ValidatorBase}");

    protected virtual ICodeFileFactoryHeader<FileType> MainFetchServiceFileFactoryHeader =>

            new CodeFileFactoryHeader<FileType>(FileType.MainFetchService, string.Empty, _ => $"{this.Environment.TargetSystemName}{FileType.MainFetchService}");

    protected virtual ICodeFileFactoryHeader<FileType> MainFetchServiceBaseFileFactoryHeader =>

            new CodeFileFactoryHeader<FileType>(FileType.MainFetchServiceBase, string.Empty, _ => $"{this.Environment.TargetSystemName}{FileType.MainFetchServiceBase}");

    private Type DefaultBLLFactoryType =>

            typeof(IDefaultBLLFactory<,>).MakeGenericType(this.Environment.PersistentDomainObjectBaseType, this.Environment.GetIdentityType());

    public IEnumerable<PropertyInfo> GetMappingProperties(Type domainType, MainDTOType fileType)
    {
        if (domainType == null) throw new ArgumentNullException(nameof(domainType));

        var properties = domainType.GetInternalProperties();

        switch (fileType)
        {
            case MainDTOType.SimpleDTO:

                return from property in properties

                       let type = property.PropertyType

                       where property.GetTopDeclaringType() != this.Environment.DomainObjectBaseType

                             && property.GetTopDeclaringType() != this.Environment.PersistentDomainObjectBaseType
                             && property.HasFamilySetMethod()
                             && !type.IsCollection()
                             && !type.IsArray
                             && !type.IsAbstract
                             && !this.Environment.DomainObjectBaseType.IsAssignableFrom(type)

                       select property;

            case MainDTOType.FullDTO:

                return from property in properties

                       where !property.IsDetail()

                             && property.HasFamilySetMethod()

                       let type = property.PropertyType

                       where !type.IsAbstract
                             && this.Environment.DomainObjectBaseType.IsAssignableFrom(type)

                       select property;

            case MainDTOType.RichDTO:

                return from property in properties

                       let type = property.PropertyType

                       let elementType = type.GetCollectionOrArrayElementType()

                       where property.IsDetail() || (elementType != null && !elementType.IsAbstract)

                       select property;

            default:
                throw new ArgumentOutOfRangeException(nameof(fileType));
        }
    }

    protected virtual IEnumerable<CodeTypeParameter> GetInternalDomainTypeSecurityParameters(Type domainType)
    {
        if (domainType == null) throw new ArgumentNullException(nameof(domainType));

        if (domainType.IsProjection())
        {
            return this.GetDomainTypeGenericSecurityParameters(domainType);
        }
        else
        {
            var hasProjections = this.DomainTypes.Any(otherDomainType => otherDomainType.IsProjection(otherSourceType => otherSourceType == domainType)
                                                                         && !this.Environment.GetProjectionEnvironment(otherDomainType).Maybe(pe => pe.UseDependencySecurity));

            if (hasProjections)
            {
                return this.GetDomainTypeGenericSecurityParameters(domainType);
            }
            else
            {
                return new CodeTypeParameter[0];
            }
        }
    }

    protected override IEnumerable<Type> GetDomainTypes()
    {
        var projectionTypes = from projectionEnvironment in this.Environment.ProjectionEnvironments

                              from type in projectionEnvironment.Assembly.GetTypes()

                              where this.IsPersistentObject(type) && type.HasAttribute<ProjectionAttribute>(attr => attr.Role == ProjectionRole.Default)

                              select type;

        return base.GetDomainTypes().Concat(projectionTypes);
    }

    protected virtual IEnumerable<Type> GetBLLDomainTypes()
    {
        return from type in this.DomainTypes

               where type.HasAttribute<BLLRoleAttribute>()

               select type;
    }

    protected virtual IEnumerable<Type> GetSecurityServiceDomainTypes()
    {
        return this.RootSecurityServerGenerator.GetSecurityServiceDomainTypes();
    }

    protected internal virtual CodeExpression GetSecurityOperationFieldReference(Enum securityOperationCode)
    {
        if (securityOperationCode == null) throw new ArgumentNullException(nameof(securityOperationCode));

        return new CodeThisReferenceExpression()
                .ToMethodInvokeExpression(
                                          "GetSecurityProvider",
                                          this.GetCodeTypeReference(null, FileType.SecurityOperation).ToTypeReferenceExpression()
                                              .ToFieldReference(securityOperationCode.ToString()));
    }

    protected virtual IRootSecurityServiceGenerator GetRootSecurityServerGenerator()
    {
        return this.Environment.SecurityOperationCodeType.IsEnum
                       ? (IRootSecurityServiceGenerator)new EnumRootSecurityServiceGenerator<GeneratorConfigurationBase<TEnvironment>>(this)
                       : new FixedRootSecurityServiceGenerator<GeneratorConfigurationBase<TEnvironment>>(this);
    }

    public virtual IRootSecurityServiceGenerator RootSecurityServerGenerator => this.lazyRootSecurityServerGenerator.Value;

    protected virtual IBLLFactoryContainerInterfaceGeneratorConfiguration GetLogics()
    {
        return new BLLFactoryContainerInterfaceGeneratorConfiguration<GeneratorConfigurationBase<TEnvironment>>(this);
    }

    public virtual IValidatorGenerator GetValidatorGenerator(Type domainType, CodeExpression validatorMapExpr)
    {
        if (domainType == null) throw new ArgumentNullException(nameof(domainType));
        if (validatorMapExpr == null) throw new ArgumentNullException(nameof(validatorMapExpr));

        return new DefaultValidatorGenerator<IGeneratorConfigurationBase<TEnvironment>>(this, domainType, validatorMapExpr);
    }

    protected override IEnumerable<ICodeFileFactoryHeader<FileType>> GetFileFactoryHeaders()
    {
        return new[]
               {
                       this.BLLContextFileFactoryHeader,
                       this.BLLContextInterfaceFileFactoryHeader,
                       this.RootSecurityServiceFileFactoryHeader,
                       this.RootSecurityServiceBaseFileFactoryHeader,
                       this.SecurityOperationFileFactoryHeader,
                       this.DomainSecurityServiceFileFactoryHeader,

                       this.DomainBLLBaseFileFactoryHeader,
                       this.DefaultOperationDomainBLLBaseFileFactoryHeader,
                       this.SecurityDomainBLLBaseFileFactoryHeader,
                       this.DefaultOperationSecurityDomainBLLBaseFileFactoryHeader,

                       this.BLLInterfaceFileFactoryHeader,
                       this.BLLFactoryInterfaceFileFactoryHeader,
                       this.BLLFactoryContainerInterfaceFileFactoryHeader,

                       this.RootSecurityServiceInterfaceFileFactoryHeader,
                       this.RootSecurityServicePathContainerInterfaceFileFactoryHeader,

                       this.DomainObjectMappingServiceFileFactoryHeader,

                       this.SecurityHierarchyDomainBLLBaseFileFactoryHeader,

                       this.ValidationMapBaseFileFactoryHeader,
                       this.ValidationMapFileFactoryHeader,

                       this.ValidatorBaseFileFactoryHeader,
                       this.ValidatorFileFactoryHeader,

                       this.MainFetchServiceBaseFileFactoryHeader,
                       this.MainFetchServiceFileFactoryHeader
               };
    }

    public CodeTypeReference BLLContextInterfaceTypeReference => this.GetCodeTypeReference(null, FileType.BLLContextInterface);

    public CodeTypeReference BLLFactoryInterfaceTypeReference => this.GetCodeTypeReference(null, FileType.BLLFactoryContainerInterface);

    public CodeTypeReference SecurityOperationTypeReference => this.GetCodeTypeReference(null, FileType.SecurityOperation);

    public CodeTypeReference RootSecurityServiceInterface => this.GetCodeTypeReference(null, FileType.RootSecurityServiceInterface);

    public CodeTypeReference DomainBLLBaseTypeReference => this.GetCodeTypeReference(null, FileType.DomainBLLBase);

    public CodeTypeReference SecurityDomainBLLBaseTypeReference => this.GetSecurityDomainBLLBaseTypeReference(null);

    public CodeTypeReference GetSecurityDomainBLLBaseTypeReference(Type type)
    {
        return this.GetCodeTypeReference(type, FileType.SecurityDomainBLLBase);
    }

    public CodeTypeReference GetSecurityHierarchyDomainBLLBaseTypeReference(Type type)
    {
        return this.GetCodeTypeReference(type, FileType.SecurityHierarchyDomainBLLBase);
    }

    public CodeTypeReference DefaultOperationDomainBLLBaseTypeReference => this.GetCodeTypeReference(null, FileType.DefaultOperationDomainBLLBase);

    public CodeTypeReference DefaultOperationSecurityDomainBLLBaseTypeReference => this.GetCodeTypeReference(null, FileType.DefaultOperationSecurityDomainBLLBase);

    public virtual Type FilterModelType { get; }

    public virtual Type ODataFilterModelType { get; }

    public virtual Type ContextFilterModelType { get; }

    public virtual Type ODataContextFilterModelType { get; }

    public virtual Type CreateModelType { get; }

    public virtual Type FormatModelType { get; }

    /// <inheritdoc />
    public virtual Type ChangeModelType { get; }

    /// <inheritdoc />
    public virtual Type MassChangeModelType { get; }

    public virtual Type ExtendedModelType { get; }

    public virtual Type IntegrationSaveModelType { get; }

    public virtual CodeExpression GetSecurityService(CodeExpression contextExpr)
    {
        return contextExpr.ToPropertyReference((ISecurityServiceContainer<object> container) => container.SecurityService);
    }

    public CodeMethodReferenceExpression GetGetSecurityProviderMethodReferenceExpression(CodeExpression contextExpression, Type domainType)
    {
        if (contextExpression == null) throw new ArgumentNullException(nameof(contextExpression));
        if (domainType == null) throw new ArgumentNullException(nameof(domainType));

        var securityServiceExpr = this.GetSecurityService(contextExpression);

        return securityServiceExpr.ToMethodReferenceExpression("GetSecurityProvider", domainType.ToTypeReference());
    }

    public IEnumerable<CodeTypeParameter> GetDomainTypeSecurityParameters(Type domainType)
    {
        if (domainType == null) throw new ArgumentNullException(nameof(domainType));

        if (this.Environment.GetProjectionEnvironment(domainType).Maybe(pe => pe.UseDependencySecurity))
        {
            return new CodeTypeParameter[0];
        }
        else
        {
            return this.domainTypeSecurityParameters[domainType];
        }
    }

    public CodeExpression GetCreateDefaultBLLExpression(CodeExpression contextExpression, CodeTypeReference genericType)
    {
        return contextExpression.ToPropertyReference((IBLLFactoryContainerContext<object> context) => context.Logics)
                                .ToPropertyReference((IBLLFactoryContainer<Ignore> container) => container.Default)
                                .ToMethodReferenceExpression("Create", genericType).ToMethodInvokeExpression();
    }

    public Type GetBLLSecurityModeType(Type domainType)
    {
        return typeof(BLLSecurityMode);

        //if (domainType == null) throw new ArgumentNullException(nameof(domainType));

        //var depSecAttr = domainType.GetCustomAttribute<DependencySecurityAttribute>();

        //if (depSecAttr != null)
        //{
        //    return this.GetBLLSecurityModeType(depSecAttr.SourceType) ?? typeof(BLLSecurityMode);
        //}

        //var hasViewEditAttr = domainType.GetViewDomainObjectAttribute() != null || domainType.GetEditDomainObjectAttribute() != null;

        //if (!this.SecurityMode.HasFlag(SecurityModeGenerate.Domain) && hasViewEditAttr)
        //{
        //    return typeof(BLLSecurityMode);
        //}
        //else
        //{
        //    if (!hasViewEditAttr && domainType.HasAttribute<CustomContextSecurityAttribute>())
        //    {
        //        return typeof(BLLSecurityMode);
        //    }

        //    return null;
        //}
    }

    public bool HasSecurityContext(Type domainType)
    {
        if (domainType == null) throw new ArgumentNullException(nameof(domainType));

        if (!this.Environment.SecurityOperationCodeType.IsEnum)
        {
            return false;
        }

        var viewCode = domainType.GetViewSecurityOperation();
        var editCode = domainType.GetEditSecurityOperation();

        var membersRequest = from securityOperationCode in this.Environment.GetSecurityOperationCodes()

                             let fieldInfo = securityOperationCode.ToFieldInfo()

                             let securityOperationAttribute = securityOperationCode.GetSecurityOperationAttribute()

                             where securityOperationAttribute != null

                                   && !securityOperationAttribute.IsClient

                                   && domainType.Name.Equals(securityOperationAttribute.DomainType, StringComparison.CurrentCultureIgnoreCase)

                             let isContext = securityOperationAttribute.IsContext

                             select new
                                    {
                                            IsContext = isContext
                                    };

        return viewCode.Maybe(v => v.GetSecurityOperationAttribute().IsContext)
               || editCode.Maybe(v => v.GetSecurityOperationAttribute().IsContext)
               || membersRequest.Any(info => info.IsContext);
    }

    private IEnumerable<CodeTypeParameter> GetDomainTypeGenericSecurityParameters(Type domainType)
    {
        if (domainType == null) throw new ArgumentNullException(nameof(domainType));

        var genericSecurityNodeInterfaces = domainType.GetGenericSecurityNodeInterfaces().Select(node => node.GetGenericTypeDefinition()).ToArray();

        var genericsRequest = from genericSecurityNodeInterface in genericSecurityNodeInterfaces

                              let argsDict = domainType.GetInterfaceImplementationArgumentDict(genericSecurityNodeInterface)

                              from pair in argsDict

                              group pair.Key by pair.Value into argGroup

                              let genericName = argGroup.Select(genArg => genArg.Name).Distinct().SingleMaybe().GetValueOrDefault(() => $"T{argGroup.Key.Name}")

                              select new CodeTypeParameter(genericName).Self(param => param.UserData["SourceType"] = argGroup.Key).ToKeyValuePair(argGroup.ToReadOnlyCollectionI());

        var genericDict = genericsRequest.ToDictionary();

        var reverseDict = genericDict.SelectMany(pair => pair.Value.Select(genArg => genArg.ToKeyValuePair(pair.Key))).ToDictionary();

        {
            var mainGeneric = new CodeTypeParameter("TDomainObject") { Constraints = { this.Environment.PersistentDomainObjectBaseType } };

            mainGeneric.UserData["SourceType"] = domainType;

            foreach (var securityNodeInterface in genericSecurityNodeInterfaces)
            {
                var constraint = this.GetConstraint(securityNodeInterface, reverseDict);

                mainGeneric.Constraints.Add(constraint);
            }

            yield return mainGeneric;
        }


        foreach (var genericPair in reverseDict)
        {
            if (genericPair.Key.ContainsGenericParameters)
            {
                genericPair.Value.Constraints.Add(this.Environment.PersistentDomainObjectBaseType);
                genericPair.Value.Constraints.AddRange(genericPair.Key.GetGenericParameterConstraints().ToArray(c => this.GetConstraint(c, reverseDict)));
            }
        }

        foreach (var generic in genericDict.Keys)
        {
            var uniConstraints = generic.Constraints.OfType<CodeTypeReference>().Distinct(CodeTypeReferenceComparer.Value).ToArray();

            generic.Constraints.Clear();
            generic.Constraints.AddRange(uniConstraints);

            yield return generic;
        }
    }

    private CodeTypeReference GetConstraint(Type constraintType, IReadOnlyDictionary<Type, CodeTypeParameter> generics)
    {
        if (constraintType == null) throw new ArgumentNullException(nameof(constraintType));
        if (generics == null) throw new ArgumentNullException(nameof(generics));

        if (constraintType.IsGenericType)
        {
            return constraintType.GetGenericTypeDefinition().ToTypeReference(constraintType.GetGenericArguments().ToArray(v => generics[v].ToTypeReference()));
        }
        else
        {
            return constraintType.ToTypeReference();
        }
    }

    protected virtual IEnumerable<Type> GetValidationTypes()
    {
        var baseTypes = this.Environment.GetDefaultDomainTypes(false).ToArray();

        var referencedTypes = baseTypes.GetReferencedTypes().Where(referencedType => referencedType.HasExpandValidation()).OrderBy(type => type.FullName).ToArray();

        var classes = from referencedType in referencedTypes

                      where referencedType.IsClass
                            && !referencedType.IsGenericTypeDefinition
                            && referencedType != typeof(string)
                            && referencedType != typeof(object)
                            && !referencedType.IsArray
                            && !this.Environment.DomainObjectBaseType.IsAssignableFrom(referencedType)

                      select referencedType;

        var structs = from referencedType in referencedTypes

                      where referencedType.IsValueType
                            && !referencedType.IsPrimitiveType()
                            && !referencedType.IsEnum

                      select referencedType;

        return baseTypes.Concat(classes).Concat(structs).Distinct();
    }

    public override CodeTypeReference GetCodeTypeReference(Type domainType, FileType fileType)
    {
        var baseRef = base.GetCodeTypeReference(domainType, fileType);

        if (domainType?.Name == "TestSecurityObjItem")
        {

        }

        if (fileType == FileType.DomainSecurityService && !this.Environment.GetProjectionEnvironment(domainType).Maybe(v => v.UseDependencySecurity))
        {
            var arguments = this.GetDomainTypeSecurityParameters(domainType).ToArray(genParam => (Type)genParam.UserData["SourceType"]);


            var realBaseRef = domainType.IsProjection() ? base.GetCodeTypeReference(domainType.GetProjectionSourceType(), fileType) : baseRef;


            realBaseRef.TypeArguments.AddRange(arguments.ToArray(arg => arg.ToTypeReference()));

            return realBaseRef;
        }

        return baseRef;
    }

    /// <inheritdoc />
    public virtual bool AllowVirtualValidation(PropertyInfo property)
    {
        var modeAttr = property.GetCustomAttribute<PropertyValidationModeAttribute>();

        if (modeAttr != null && modeAttr.HasValue(true))
        {
            return true;
        }

        return property.HasAttribute(attr => attr is PropertyValidatorAttribute || attr is IRestrictionAttribute);
    }

    /// <inheritdoc />
    public virtual bool SquashClassValidators(Type domainType)
    {
        return true;
    }

    /// <inheritdoc />
    public virtual bool SquashPropertyValidators(PropertyInfo property)
    {
        return true;
    }

    public virtual bool GenerateDomainServiceConstructor(Type domainType)
    {
        if (domainType == null) throw new ArgumentNullException(nameof(domainType));

        return true;
    }
}
#pragma warning restore S100 // Methods and properties should be named in camel case
