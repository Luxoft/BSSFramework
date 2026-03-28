using System.CodeDom;
using System.Collections.ObjectModel;
using System.Reflection;

using CommonFramework;
using CommonFramework.DictionaryCache;

using Framework.BLL.Domain.DTO;
using Framework.BLL.Domain.Extensions;
using Framework.BLL.Domain.Serialization;
using Framework.BLL.Domain.Serialization.Extensions;
using Framework.CodeDom.Extensions;
using Framework.CodeGeneration.Configuration;
using Framework.CodeGeneration.DomainMetadata;
using Framework.CodeGeneration.DTOGenerator.CodeTypeReferenceService;
using Framework.CodeGeneration.DTOGenerator.CodeTypeReferenceService.Base;
using Framework.CodeGeneration.DTOGenerator.Extensions;
using Framework.CodeGeneration.DTOGenerator.FileTypes;
using Framework.CodeGeneration.DTOGenerator.GeneratePolicy;
using Framework.CodeGeneration.DTOGenerator.Map;
using Framework.CodeGeneration.FileFactory;
using Framework.CodeGeneration.GeneratePolicy;
using Framework.Core;
using Framework.Projection;
using Framework.Relations;

namespace Framework.CodeGeneration.DTOGenerator.Configuration;

public abstract class GeneratorConfigurationBase<TEnvironment> : GeneratorConfiguration<TEnvironment, BaseFileType>, IGeneratorConfigurationBase<TEnvironment>
        where TEnvironment : class, IGenerationEnvironmentBase
{
    private readonly IDictionaryCache<Tuple<Type, DTOFileType>, ReadOnlyCollection<PropertyInfo>> domainTypePropertiesCache;

    protected GeneratorConfigurationBase(TEnvironment environment)
            : base(environment)
    {
        this.domainTypePropertiesCache = new DictionaryCache<Tuple<Type, DTOFileType>, ReadOnlyCollection<PropertyInfo>>(t =>

                t.Pipe(this.GetInternalDomainTypeProperties).OrderBy(prop => prop.Name).ToReadOnlyCollection()).WithLock();

        this.DefaultCodeTypeReferenceService = new ConfigurationCodeTypeReferenceService<GeneratorConfigurationBase<TEnvironment>>(this);

        this.ProjectionTypes = LazyInterfaceImplementHelper.CreateProxy(() => this.GetProjectionTypes().ToReadOnlyCollectionI());

        this.GeneratePolicy = LazyInterfaceImplementHelper.CreateProxy(this.CreateGeneratePolicy);


        this.ClientDTOMappingServiceInterfaceFileFactoryHeader = BaseFileType.ClientDTOMappingServiceInterface.ToHeader("", _ => $"I{this.Environment.TargetSystemName}ClientDTOMappingService");

        this.ClientPrimitiveDTOMappingServiceFactoryHeader = BaseFileType.ClientPrimitiveDTOMappingService.ToHeader("", _ => this.Environment.TargetSystemName + BaseFileType.ClientPrimitiveDTOMappingService);

        this.ClientPrimitiveDTOMappingServiceBaseFactoryHeader = BaseFileType.ClientPrimitiveDTOMappingServiceBase.ToHeader("", _ => this.Environment.TargetSystemName + BaseFileType.ClientPrimitiveDTOMappingServiceBase);
    }

    public IGeneratePolicy<RoleFileType> GeneratePolicy { get; }

    public IReadOnlyCollection<Type> ProjectionTypes { get; }

    public virtual bool ExpandStrictMaybeToDefault { get; } = false;

    public virtual bool IdentityIsReference { get; } = false;

    public virtual ICodeTypeReferenceService DefaultCodeTypeReferenceService { get; }

    public virtual Type CollectionType { get; } = typeof(List<>);

    public virtual Type ClientEditCollectionType => this.CollectionType;


    public virtual string DTOIdentityPropertyName { get; } = "Identity";

    public virtual string DTOEmptyPropertyName { get; } = "Empty";

    public virtual string DataContractNamespace { get; } = string.Empty;


    protected override string NamespacePostfix { get; } = "Generated.DTO";


    protected virtual ICodeFileFactoryHeader<DTOFileType> IdentityDTOFileFactoryHeader { get; } = BaseFileType.IdentityDTO.ToHeader();

    protected virtual ICodeFileFactoryHeader<DTOFileType> StrictDTOFileFactoryHeader { get; } = BaseFileType.StrictDTO.ToHeader();

    protected virtual ICodeFileFactoryHeader<DTOFileType> UpdateDTOFileFactoryHeader { get; } = BaseFileType.UpdateDTO.ToHeader();

    protected virtual ICodeFileFactoryHeader<DTOFileType> ProjectionDTOFileFactoryHeader { get; } =

        BaseFileType.ProjectionDTO.ToHeader($@"{BaseFileType.ProjectionDTO}\", domainType => domainType.Name.SkipLast("Projection", false) + BaseFileType.ProjectionDTO);


    protected virtual ICodeFileFactoryHeader<MainDTOFileType> BaseAbstractDTOFileFactoryHeader { get; } = BaseFileType.BaseAbstractDTO.ToHeader(string.Empty, _ => BaseFileType.BaseAbstractDTO.Name);

    protected virtual ICodeFileFactoryHeader<MainDTOFileType> BasePersistentDTOFileFactoryHeader { get; } = BaseFileType.BasePersistentDTO.ToHeader(string.Empty, _ => BaseFileType.BasePersistentDTO.Name);

    protected virtual ICodeFileFactoryHeader<MainDTOFileType> BaseAuditPersistentDTOFileFactoryHeader { get; } = BaseFileType.BaseAuditPersistentDTO.ToHeader(string.Empty, _ => BaseFileType.BaseAuditPersistentDTO.Name);


    protected virtual ICodeFileFactoryHeader<MainDTOFileType> VisualDTOFileFactoryHeader { get; } = BaseFileType.VisualDTO.ToHeader();


    protected virtual ICodeFileFactoryHeader<MainDTOFileType> SimpleDTOFileFactoryHeader { get; } = BaseFileType.SimpleDTO.ToHeader();

    protected virtual ICodeFileFactoryHeader<MainDTOFileType> FullDTOFileFactoryHeader { get; } = BaseFileType.FullDTO.ToHeader();

    protected virtual ICodeFileFactoryHeader<MainDTOFileType> RichDTOFileFactoryHeader { get; } = BaseFileType.RichDTO.ToHeader();

    protected virtual ICodeFileFactoryHeader<BaseFileType> ClientDTOMappingServiceInterfaceFileFactoryHeader { get; }

    protected virtual ICodeFileFactoryHeader<BaseFileType> ClientPrimitiveDTOMappingServiceBaseFactoryHeader { get; }

    protected virtual ICodeFileFactoryHeader<BaseFileType> ClientPrimitiveDTOMappingServiceFactoryHeader { get; }




    //public static IEnumerable<MainDTOFileType> GetNestedTypes(this MainDTOFileType fileType, ImmutableArray<MainDTOFileType> mainDtoTypes)
    //{
    //    if (fileType == null) throw new ArgumentNullException(nameof(fileType));

    //    return fileType.GetAllElements(ft => ft.NestedType, true);
    //}

    public IEnumerable<MainDTOFileType> GetNestedTypes(MainDTOFileType fileType)
    {
        throw new NotImplementedException();
    }

    public virtual bool ForceGenerateProperties(Type domainType, DTOFileType fileType)
    {
        if (domainType == null) throw new ArgumentNullException(nameof(domainType));
        if (fileType == null) throw new ArgumentNullException(nameof(fileType));

        return true;
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

            if (fileType == BaseFileType.StrictDTO || fileType == BaseFileType.UpdateDTO)
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
        yield return this.GetTypeMap(this.Environment.DomainObjectBaseType, BaseFileType.BaseAbstractDTO);

        yield return this.GetTypeMap(this.Environment.PersistentDomainObjectBaseType, BaseFileType.BasePersistentDTO);

        yield return this.GetTypeMap(this.Environment.AuditPersistentDomainObjectBaseType, BaseFileType.BaseAuditPersistentDTO);

        foreach (var domainType in this.DomainTypes)
        {
            if (domainType.IsProjection())
            {
                yield return this.GetTypeMap(domainType, BaseFileType.ProjectionDTO);
            }
            else
            {
                if (this.IsPersistentObject(domainType))
                {
                    yield return this.GetTypeMap(domainType, BaseFileType.IdentityDTO);
                }

                if (domainType.HasVisualIdentityProperties())
                {
                    yield return this.GetTypeMap(domainType, BaseFileType.VisualDTO);
                }

                yield return this.GetTypeMap(domainType, BaseFileType.SimpleDTO);

                yield return this.GetTypeMap(domainType, BaseFileType.FullDTO);

                yield return this.GetTypeMap(domainType, BaseFileType.RichDTO);

                yield return this.GetTypeMap(domainType, BaseFileType.StrictDTO);

                yield return this.GetTypeMap(domainType, BaseFileType.UpdateDTO);
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

    public virtual ILayerCodeTypeReferenceService? GetLayerCodeTypeReferenceService(DTOFileType fileType)
    {
        if (fileType == null) throw new ArgumentNullException(nameof(fileType));

        if (fileType == BaseFileType.ProjectionDTO)
        {
            return new ProjectionCodeTypeReferenceService<IGeneratorConfigurationBase<IGenerationEnvironmentBase>>(this);
        }
        else if (fileType == BaseFileType.StrictDTO)
        {
            return new StrictCodeTypeReferenceService<IGeneratorConfigurationBase<IGenerationEnvironmentBase>>(this);
        }
        else if (fileType == BaseFileType.UpdateDTO)
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
        return this.GetCodeTypeReference(null, BaseFileType.ClientPrimitiveDTOMappingService).ToTypeReferenceExpression().ToPropertyReference("Default");
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


        if (fileType == BaseFileType.BaseAbstractDTO)
        {
            return serializationProperties.Where(this.Environment.IsDomainObjectBaseProperty);
        }
        else if (fileType == BaseFileType.BasePersistentDTO || fileType == BaseFileType.IdentityDTO)
        {
            return serializationProperties.Where(this.Environment.IsPersistentDomainObjectBaseProperty);
        }
        else if (fileType == BaseFileType.BaseAuditPersistentDTO)
        {
            return serializationProperties.Where(this.Environment.IsAuditPersistentDomainObjectBaseProperty);
        }
        else if (fileType == BaseFileType.VisualDTO)
        {
            return from property in this.GetDomainTypeProperties(domainType, BaseFileType.SimpleDTO)

                   where property.IsVisualIdentity() && property.PropertyType == typeof(string)

                   select property;
        }
        else if (fileType == BaseFileType.SimpleDTO)
        {
            return from property in serializationProperties

                   let type = property.PropertyType

                   where !this.Environment.IsDomainObjectBaseProperty(property) && !this.Environment.IsPersistentDomainObjectBaseProperty(property) && !this.Environment.IsAuditPersistentDomainObjectBaseProperty(property)

                         && !type.IsCollection()
                         && !type.IsArray
                         && !this.Environment.DomainObjectBaseType.IsAssignableFrom(type)

                   select property;
        }
        else if (fileType == BaseFileType.FullDTO)
        {
            return from property in serializationProperties

                   where !property.IsDetail()

                   let type = property.PropertyType

                   where !type.IsAbstract && this.Environment.DomainObjectBaseType.IsAssignableFrom(type)

                   select property;
        }
        else if (fileType == BaseFileType.RichDTO)
        {
            return from property in serializationProperties

                   let type = property.PropertyType

                   let elementType = type.GetCollectionOrArrayElementType()

                   where property.IsDetail() || (elementType != null && (!this.Environment.DomainObjectBaseType.IsAssignableFrom(type) || !elementType.IsAbstractDTO()))

                   select property;
        }
        else if (fileType == BaseFileType.StrictDTO || fileType == BaseFileType.UpdateDTO)
        {
            return from property in serializationProperties

                   where property.IsWritable(role, true) || this.IsIdentityOrVersionProperty(property)

                   select property;
        }
        else if (fileType == BaseFileType.ProjectionDTO)
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

    protected override IEnumerable<ICodeFileFactoryHeader<BaseFileType>> GetFileFactoryHeaders()
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

        yield return this.ClientDTOMappingServiceInterfaceFileFactoryHeader;

        yield return this.ClientPrimitiveDTOMappingServiceBaseFactoryHeader;
        yield return this.ClientPrimitiveDTOMappingServiceFactoryHeader;
    }

    protected virtual IEnumerable<Type> GetProjectionTypes()
    {
        return this.Environment.ProjectionEnvironments
                   .SelectMany(projectionEnvironment => projectionEnvironment.Assembly.Types)
                   .Where(type => type.HasAttribute<ProjectionAttribute>(attr => attr.Role == ProjectionRole.Default));
    }
}
