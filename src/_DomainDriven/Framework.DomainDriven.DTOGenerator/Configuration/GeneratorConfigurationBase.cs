using System.CodeDom;
using System.Collections.ObjectModel;
using System.Reflection;

using Framework.CodeDom;
using Framework.Core;
using Framework.DomainDriven.Generation.Domain;
using Framework.DomainDriven.Serialization;
using Framework.Persistent;
using Framework.Projection;
using Framework.Security;
using Framework.SecuritySystem;
using Framework.Transfering;

namespace Framework.DomainDriven.DTOGenerator;

public abstract class GeneratorConfigurationBase<TEnvironment> : GeneratorConfiguration<TEnvironment, FileType>, IGeneratorConfigurationBase<TEnvironment>
        where TEnvironment : class, IGenerationEnvironmentBase
{
    private readonly IDictionaryCache<Tuple<Type, DTOFileType>, ReadOnlyCollection<PropertyInfo>> domainTypePropertiesCache;

    protected GeneratorConfigurationBase(TEnvironment environment)
            : base(environment)
    {
        this.domainTypePropertiesCache = new DictionaryCache<Tuple<Type, DTOFileType>, ReadOnlyCollection<PropertyInfo>>(t =>

                t.Pipe(this.GetInternalDomainTypeProperties).OrderBy(prop => prop.Name).ToReadOnlyCollection()).WithLock();

        this.DefaultCodeTypeReferenceService = new ConfigurationCodeTypeReferenceService<GeneratorConfigurationBase<TEnvironment>>(this);

        this.DomainObjectSecurityOperationCodeFileFactoryHeader = new CodeFileFactoryHeader<RoleFileType>(FileType.DomainObjectSecurityOperationCode, string.Empty, domainType => $"{this.Environment.TargetSystemName}{domainType.Name}SecurityOperationCode");

        this.ProjectionTypes = LazyInterfaceImplementHelper.CreateProxy(() => this.GetProjectionTypes().ToReadOnlyCollectionI());

        this.GeneratePolicy = LazyInterfaceImplementHelper.CreateProxy(this.CreateGeneratePolicy);

        this.TypesWithSecondarySecurityOperations = LazyInterfaceImplementHelper.CreateProxy(() => this.GetTypesWithSecondarySecurityOperations().ToReadOnlyDictionaryI());


        this.ClientDTOMappingServiceInterfaceFileFactoryHeader = FileType.ClientDTOMappingServiceInterface.ToHeader("", _ => $"I{this.Environment.TargetSystemName}ClientDTOMappingService");

        this.ClientPrimitiveDTOMappingServiceFactoryHeader = FileType.ClientPrimitiveDTOMappingService.ToHeader("", _ => this.Environment.TargetSystemName + FileType.ClientPrimitiveDTOMappingService);

        this.ClientPrimitiveDTOMappingServiceBaseFactoryHeader = FileType.ClientPrimitiveDTOMappingServiceBase.ToHeader("", _ => this.Environment.TargetSystemName + FileType.ClientPrimitiveDTOMappingServiceBase);
    }

    public IGeneratePolicy<RoleFileType> GeneratePolicy { get; }

    public IReadOnlyCollection<Type> ProjectionTypes { get; }

    public IReadOnlyDictionary<Type, ReadOnlyCollection<SecurityOperation>> TypesWithSecondarySecurityOperations { get; }

    public virtual bool ExpandStrictMaybeToDefault { get; } = false;

    public virtual bool IdentityIsReference { get; } = false;

    public virtual ICodeTypeReferenceService DefaultCodeTypeReferenceService { get; }

    public virtual Type CollectionType { get; } = typeof(List<>);

    public virtual Type ClientEditCollectionType => this.CollectionType;


    public virtual string DTOIdentityPropertyName { get; } = "Identity";

    public virtual string DTOEmptyPropertyName { get; } = "Empty";

    public virtual string DataContractNamespace { get; } = string.Empty;


    protected override string NamespacePostfix { get; } = "Generated.DTO";


    protected virtual ICodeFileFactoryHeader<DTOFileType> IdentityDTOFileFactoryHeader { get; } = FileType.IdentityDTO.ToHeader();

    protected virtual ICodeFileFactoryHeader<DTOFileType> StrictDTOFileFactoryHeader { get; } = FileType.StrictDTO.ToHeader();

    protected virtual ICodeFileFactoryHeader<DTOFileType> UpdateDTOFileFactoryHeader { get; } = FileType.UpdateDTO.ToHeader();

    protected virtual ICodeFileFactoryHeader<DTOFileType> ProjectionDTOFileFactoryHeader { get; } =

        FileType.ProjectionDTO.ToHeader($@"{FileType.ProjectionDTO}\", domainType => domainType.Name.SkipLast("Projection", false) + FileType.ProjectionDTO);


    protected virtual ICodeFileFactoryHeader<MainDTOFileType> BaseAbstractDTOFileFactoryHeader { get; } = FileType.BaseAbstractDTO.ToHeader(string.Empty, _ => FileType.BaseAbstractDTO.Name);

    protected virtual ICodeFileFactoryHeader<MainDTOFileType> BasePersistentDTOFileFactoryHeader { get; } = FileType.BasePersistentDTO.ToHeader(string.Empty, _ => FileType.BasePersistentDTO.Name);

    protected virtual ICodeFileFactoryHeader<MainDTOFileType> BaseAuditPersistentDTOFileFactoryHeader { get; } = FileType.BaseAuditPersistentDTO.ToHeader(string.Empty, _ => FileType.BaseAuditPersistentDTO.Name);


    protected virtual ICodeFileFactoryHeader<MainDTOFileType> VisualDTOFileFactoryHeader { get; } = FileType.VisualDTO.ToHeader();


    protected virtual ICodeFileFactoryHeader<MainDTOFileType> SimpleDTOFileFactoryHeader { get; } = FileType.SimpleDTO.ToHeader();

    protected virtual ICodeFileFactoryHeader<MainDTOFileType> FullDTOFileFactoryHeader { get; } = FileType.FullDTO.ToHeader();

    protected virtual ICodeFileFactoryHeader<MainDTOFileType> RichDTOFileFactoryHeader { get; } = FileType.RichDTO.ToHeader();

    protected virtual ICodeFileFactoryHeader<RoleFileType> DomainObjectSecurityOperationCodeFileFactoryHeader { get; }

    protected virtual ICodeFileFactoryHeader<FileType> ClientDTOMappingServiceInterfaceFileFactoryHeader { get; }

    protected virtual ICodeFileFactoryHeader<FileType> ClientPrimitiveDTOMappingServiceBaseFactoryHeader { get; }

    protected virtual ICodeFileFactoryHeader<FileType> ClientPrimitiveDTOMappingServiceFactoryHeader { get; }


    public virtual bool ForceGenerateProperties(Type domainType, DTOFileType fileType)
    {
        if (domainType == null) throw new ArgumentNullException(nameof(domainType));
        if (fileType == null) throw new ArgumentNullException(nameof(fileType));

        return false;
    }


    protected virtual IEnumerable<KeyValuePair<Type, ReadOnlyCollection<SecurityOperation>>> GetMainTypesWithSecondarySecurityOperations()
    {
        return this.DomainTypes.GetTypesWithSecondarySecurityOperations();
    }

    protected virtual IEnumerable<KeyValuePair<Type, ReadOnlyCollection<SecurityOperation>>> GetTypesWithSecondarySecurityOperations()
    {
        var mainResult = this.GetMainTypesWithSecondarySecurityOperations().ToDictionary();

        var dependencyRequest = from domainType in this.GetDomainTypes()

                                where !mainResult.ContainsKey(domainType)

                                let rootSourceType = domainType.GetDependencySecuritySourceType(true)

                                where rootSourceType != null

                                let mainPair = mainResult.GetValueOrDefault(rootSourceType)

                                where mainPair != null

                                select domainType.ToKeyValuePair(mainPair);


        return mainResult.Concat(dependencyRequest);
    }

    public IEnumerable<PropertyInfo> GetDomainTypeProperties(Type domainType, DTOFileType fileType)
    {
        if (domainType == null) throw new ArgumentNullException(nameof(domainType));
        if (fileType == null) throw new ArgumentNullException(nameof(fileType));

        return this.domainTypePropertiesCache.GetValue(domainType, fileType);
    }

    public virtual IEnumerable<PropertyInfo> GetDomainTypeProperties(Type domainType, DTOFileType fileType, bool isWritable)
    {
        if (domainType == null) throw new ArgumentNullException(nameof(domainType));
        if (fileType == null) throw new ArgumentNullException(nameof(fileType));

        var baseProperties = this.GetDomainTypeProperties(domainType, fileType);

        if (isWritable)
        {
            var withoutIdentity = baseProperties.Where(prop => !this.IsIdentityProperty(prop));

            if (fileType == FileType.StrictDTO || fileType == FileType.UpdateDTO)
            {
                return withoutIdentity;
            }
            else
            {
                return from property in withoutIdentity

                       where property.IsWritable(fileType.Role, false)

                       select property;
            }
        }
        else
        {
            return baseProperties;
        }
    }


    public virtual IEnumerable<GenerateTypeMap> GetTypeMaps()
    {
        yield return this.GetTypeMap(this.Environment.DomainObjectBaseType, FileType.BaseAbstractDTO);

        yield return this.GetTypeMap(this.Environment.PersistentDomainObjectBaseType, FileType.BasePersistentDTO);

        yield return this.GetTypeMap(this.Environment.AuditPersistentDomainObjectBaseType, FileType.BaseAuditPersistentDTO);

        foreach (var domainType in this.DomainTypes)
        {
            if (domainType.IsProjection())
            {
                yield return this.GetTypeMap(domainType, FileType.ProjectionDTO);
            }
            else
            {
                if (this.IsPersistentObject(domainType))
                {
                    yield return this.GetTypeMap(domainType, FileType.IdentityDTO);
                }

                if (domainType.HasVisualIdentityProperties())
                {
                    yield return this.GetTypeMap(domainType, FileType.VisualDTO);
                }

                yield return this.GetTypeMap(domainType, FileType.SimpleDTO);

                yield return this.GetTypeMap(domainType, FileType.FullDTO);

                yield return this.GetTypeMap(domainType, FileType.RichDTO);

                yield return this.GetTypeMap(domainType, FileType.StrictDTO);

                yield return this.GetTypeMap(domainType, FileType.UpdateDTO);
            }
        }
    }


    public GenerateTypeMap GetTypeMap(Type domainType, DTOFileType fileType)
    {
        if (domainType == null) throw new ArgumentNullException(nameof(domainType));
        if (fileType == null) throw new ArgumentNullException(nameof(fileType));

        return new GenerateTypeMap(domainType, fileType, this.GetDomainTypeProperties(domainType, fileType).Select(property => this.GetPropertyMap(domainType, fileType, property)));
    }

    private GeneratePropertyMap GetPropertyMap(Type domainType, DTOFileType fileType, PropertyInfo property)
    {
        if (domainType == null) throw new ArgumentNullException(nameof(domainType));
        if (fileType == null) throw new ArgumentNullException(nameof(fileType));

        var collectionElementType = property.PropertyType.GetCollectionOrArrayElementType();

        var nullableElementType = property.PropertyType.GetNullableElementType();

        var isCollection = collectionElementType != null;

        var isNullable = nullableElementType != null;

        var elementType = collectionElementType ?? nullableElementType ?? property.PropertyType;

        var isDetail = isCollection ? !property.IsNotDetail() : property.IsDetail();

        var baseElementFileType = this.GetLayerCodeTypeReferenceService(fileType).Maybe(service => service.GetFileType(property));

        var elementFileType = baseElementFileType ?? this.DefaultCodeTypeReferenceService.GetFileType(property);

        return new GeneratePropertyMap(property, elementType, elementFileType, isCollection, isNullable, isDetail);
    }

    public virtual ILayerCodeTypeReferenceService GetLayerCodeTypeReferenceService(DTOFileType fileType)
    {
        if (fileType == null) throw new ArgumentNullException(nameof(fileType));

        if (fileType == FileType.ProjectionDTO)
        {
            return new ProjectionCodeTypeReferenceService<IGeneratorConfigurationBase<IGenerationEnvironmentBase>>(this);
        }
        else if (fileType == FileType.StrictDTO)
        {
            return new StrictCodeTypeReferenceService<IGeneratorConfigurationBase<IGenerationEnvironmentBase>>(this);
        }
        else if (fileType == FileType.UpdateDTO)
        {
            return new UpdateCodeTypeReferenceService<IGeneratorConfigurationBase<IGenerationEnvironmentBase>>(this);
        }
        else if (fileType is MainDTOFileType)
        {
            return new MainCodeTypeReferenceService<IGeneratorConfigurationBase<IGenerationEnvironmentBase>>(this);
        }
        else
        {
            return null;
        }
    }

    public CodeExpression GetDefaultClientDTOMappingServiceExpression()
    {
        return this.GetCodeTypeReference(null, FileType.ClientPrimitiveDTOMappingService).ToTypeReferenceExpression().ToPropertyReference("Default");
    }

    public virtual CodeExpression GetCreateUpdateDTOExpression(
            Type domainType,
            CodeExpression currentStrictSource,
            CodeExpression baseStrictSource,
            CodeExpression mappingService)
    {
        if (baseStrictSource == null)
        {
            return this.GetCodeTypeReference(domainType, DTOType.UpdateDTO).ToObjectCreateExpression(currentStrictSource, mappingService);
        }
        else
        {
            return this.GetCodeTypeReference(domainType, DTOType.UpdateDTO).ToObjectCreateExpression(currentStrictSource, baseStrictSource, mappingService);
        }
    }

    protected virtual IGeneratePolicy<RoleFileType> CreateGeneratePolicy()
    {
        return new DTORoleGeneratePolicy(DTORole.All);
    }

    protected virtual IEnumerable<PropertyInfo> GetInternalDomainTypeProperties(Type domainType, DTOFileType fileType)
    {
        if (domainType == null) throw new ArgumentNullException(nameof(domainType));
        if (fileType == null) throw new ArgumentNullException(nameof(fileType));

        return this.GetInternalDomainTypeProperties(domainType, fileType, fileType.Role);
    }

    /// <summary>
    /// Возвращает список описателей свойств доменного типа.
    /// </summary>
    /// <param name="domainType">Тип доменного типа.</param>
    /// <param name="fileType">Тип генерируемого файла.</param>
    /// <param name="role">Роль DTO.</param>
    /// <returns>Экземпляр <see cref="IEnumerable{PropertyInfo}"/>.</returns>
    /// <exception cref="ArgumentNullException">Аргумент
    /// <paramref name="domainType"/>
    /// или
    /// <paramref name="fileType"/> равен null.
    /// </exception>
    protected IEnumerable<PropertyInfo> GetInternalDomainTypeProperties(Type domainType, DTOFileType fileType, DTORole role)
    {
        if (domainType == null) throw new ArgumentNullException(nameof(domainType));
        if (fileType == null) throw new ArgumentNullException(nameof(fileType));

        var serializationProperties = domainType.GetSerializationProperties(role);


        if (fileType == FileType.BaseAbstractDTO)
        {
            return serializationProperties.Where(this.Environment.IsDomainObjectBaseProperty);
        }
        else if (fileType == FileType.BasePersistentDTO || fileType == FileType.IdentityDTO)
        {
            return serializationProperties.Where(this.Environment.IsPersistentDomainObjectBaseProperty);
        }
        else if (fileType == FileType.BaseAuditPersistentDTO)
        {
            return serializationProperties.Where(this.Environment.IsAuditPersistentDomainObjectBaseProperty);
        }
        else if (fileType == FileType.VisualDTO)
        {
            return from property in this.GetDomainTypeProperties(domainType, FileType.SimpleDTO)

                   where property.IsVisualIdentity() && property.PropertyType == typeof(string)

                   select property;
        }
        else if (fileType == FileType.SimpleDTO)
        {
            return from property in serializationProperties

                   let type = property.PropertyType

                   where !this.Environment.IsDomainObjectBaseProperty(property) && !this.Environment.IsPersistentDomainObjectBaseProperty(property) && !this.Environment.IsAuditPersistentDomainObjectBaseProperty(property)

                         && !type.IsCollection()
                         && !type.IsArray
                         && !this.Environment.DomainObjectBaseType.IsAssignableFrom(type)

                   select property;
        }
        else if (fileType == FileType.FullDTO)
        {
            return from property in serializationProperties

                   where !property.IsDetail()

                   let type = property.PropertyType

                   where !type.IsAbstract && this.Environment.DomainObjectBaseType.IsAssignableFrom(type)

                   select property;
        }
        else if (fileType == FileType.RichDTO)
        {
            return from property in serializationProperties

                   let type = property.PropertyType

                   let elementType = type.GetCollectionOrArrayElementType()

                   where property.IsDetail() || (elementType != null && (!this.Environment.DomainObjectBaseType.IsAssignableFrom(type) || !elementType.IsAbstractDTO()))

                   select property;
        }
        else if (fileType == FileType.StrictDTO || fileType == FileType.UpdateDTO)
        {
            return from property in serializationProperties

                   where property.IsWritable(role, true) || this.IsIdentityOrVersionProperty(property)

                   select property;
        }
        else if (fileType == FileType.ProjectionDTO)
        {
            return from property in serializationProperties

                   let type = property.PropertyType

                   where !this.Environment.IsDomainObjectBaseProperty(property) && !this.Environment.IsPersistentDomainObjectBaseProperty(property)

                   select property;
        }
        else
        {
            throw new ArgumentOutOfRangeException(nameof(fileType));
        }
    }

    /// <summary>
    /// Возвращает список доменных типов.
    /// </summary>
    /// <returns>Экземпляр <see cref="IEnumerable{Type}"/></returns>
    protected override IEnumerable<Type> GetDomainTypes()
    {
        return this.Environment.GetDefaultDomainTypes(false).Concat(this.ProjectionTypes);
    }

    protected override IEnumerable<ICodeFileFactoryHeader<FileType>> GetFileFactoryHeaders()
    {
        yield return this.BaseAbstractDTOFileFactoryHeader;
        yield return this.BasePersistentDTOFileFactoryHeader;
        yield return this.BaseAuditPersistentDTOFileFactoryHeader;

        yield return this.SimpleDTOFileFactoryHeader;
        yield return this.FullDTOFileFactoryHeader;
        yield return this.RichDTOFileFactoryHeader;

        yield return this.VisualDTOFileFactoryHeader;

        yield return this.IdentityDTOFileFactoryHeader;
        yield return this.StrictDTOFileFactoryHeader;
        yield return this.UpdateDTOFileFactoryHeader;

        yield return this.ProjectionDTOFileFactoryHeader;

        yield return this.DomainObjectSecurityOperationCodeFileFactoryHeader;

        yield return this.ClientDTOMappingServiceInterfaceFileFactoryHeader;

        yield return this.ClientPrimitiveDTOMappingServiceBaseFactoryHeader;
        yield return this.ClientPrimitiveDTOMappingServiceFactoryHeader;
    }

    protected virtual IEnumerable<Type> GetProjectionTypes()
    {
        return this.Environment.ProjectionEnvironments
                   .SelectMany(projectionEnvironment => projectionEnvironment.Assembly.GetTypes())
                   .Where(type => type.HasAttribute<ProjectionAttribute>(attr => attr.Role == ProjectionRole.Default));
    }
}
