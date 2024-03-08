using System.CodeDom;
using System.Collections.ObjectModel;
using System.Reflection;

using Framework.CodeDom;
using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.Generation.Domain;
using Framework.DomainDriven.Serialization;
using Framework.Events;
using Framework.Persistent;
using Framework.Projection;

namespace Framework.DomainDriven.DTOGenerator.Server;

public abstract class ServerGeneratorConfigurationBase<TEnvironment> : GeneratorConfigurationBase<TEnvironment>, IServerGeneratorConfigurationBase<TEnvironment>
        where TEnvironment : class, IServerGenerationEnvironmentBase
{
    private readonly Lazy<ReadOnlyCollection<DTOFileType>> _lazyLambdaConvertTypes;

    private readonly IDictionaryCache<Tuple<Type, DTOFileType, bool>, ReadOnlyCollection<Type>> _domainTypeDetailsCache;

    private readonly IDictionaryCache<Tuple<Type, DTOFileType, bool>, ReadOnlyCollection<Type>> _domainTypeMastersCache;


    protected ServerGeneratorConfigurationBase(TEnvironment environment)
            : base(environment)
    {
        this._lazyLambdaConvertTypes = LazyHelper.Create(() => this.GetLambdaConvertTypes().ToReadOnlyCollection());


        this.PropertyAssignerConfigurator = LazyInterfaceImplementHelper.CreateProxy(this.GetPropertyAssignerConfigurator);


        this._domainTypeDetailsCache = new DictionaryCache<Tuple<Type, DTOFileType, bool>, ReadOnlyCollection<Type>>(t => t.Pipe((domainType, fileType, isWritable) =>
        {
            var request = from property in this.GetDomainTypeProperties(domainType, fileType, isWritable)

                          where this.IsCollectionProperty(property) || (property.IsDetail() && this.IsReferenceProperty(property))

                          let elementType = property.PropertyType.GetCollectionElementType() ?? property.PropertyType

                          where this.IsPersistentObject(elementType)

                          select elementType;

            return request.ToReadOnlyCollection();
        })).WithLock();

        this._domainTypeMastersCache = new DictionaryCache<Tuple<Type, DTOFileType, bool>, ReadOnlyCollection<Type>>(t => t.Pipe((domainType, fileType, isWritable) =>
        {
            var request = from masterDomainType in this.DomainTypes

                          where !masterDomainType.IsProjection()

                          where this.GetDomainTypeDetails(masterDomainType, fileType, isWritable).Contains(domainType)

                          select masterDomainType;

            return request.ToReadOnlyCollection<Type>();
        })).WithLock();


        this.ServerDTOMappingServiceInterfaceFileFactoryHeader = ServerFileType.ServerDTOMappingServiceInterface.ToHeader("", _ => "I" + this.Environment.TargetSystemName + "DTOMappingService");

        this.ServerPrimitiveDTOMappingServiceFileFactoryHeader = ServerFileType.ServerPrimitiveDTOMappingService.ToHeader("", _ => this.Environment.TargetSystemName + ServerFileType.ServerPrimitiveDTOMappingService);

        this.ServerPrimitiveDTOMappingServiceBaseFileFactoryHeader = ServerFileType.ServerPrimitiveDTOMappingServiceBase.ToHeader("", _ => this.Environment.TargetSystemName + ServerFileType.ServerPrimitiveDTOMappingServiceBase);


        this.VersionType = this.Environment.AuditPersistentDomainObjectBaseType.GetInterfaceImplementationArgument(typeof(IVersionObject<>)) ?? typeof(Ignore);

        this.VersionProperty = this.Environment.AuditPersistentDomainObjectBaseType.GetProperties().SingleOrDefault(prop => prop.HasAttribute<VersionAttribute>());
    }


    public IPropertyAssignerConfigurator PropertyAssignerConfigurator { get; }

    public virtual IEventOperationSource EventOperationSource { get; } = new EventOperationSource();

    public sealed override Type CollectionType { get; } = typeof(List<>);

    public virtual ClientDTORole MapToDomainRole { get; } = ClientDTORole.Strict | ClientDTORole.Update;

    public virtual string ToDomainObjectMethodName { get; } = "ToDomainObject";

    public virtual string MapToDomainObjectMethodName { get; } = "MapToDomainObject";

    public virtual string EventDataContractNamespace { get; } = "";

    public virtual string IntegrationDataContractNamespace { get; } = "";

    public virtual bool CheckVersion { get; } = true;

    public virtual Type VersionType { get; }

    public PropertyInfo VersionProperty { get; }


    protected virtual ICodeFileFactoryHeader<FileType> LambdaHelperFileFactoryHeader { get; } = ServerFileType.LambdaHelper.ToHeader(@"Helpers\", _ => ServerFileType.LambdaHelper.Name);

    protected virtual ICodeFileFactoryHeader<FileType> ServerDTOMappingServiceInterfaceFileFactoryHeader { get; }

    protected virtual ICodeFileFactoryHeader<FileType> ServerPrimitiveDTOMappingServiceFileFactoryHeader { get; }

    protected virtual ICodeFileFactoryHeader<FileType> ServerPrimitiveDTOMappingServiceBaseFileFactoryHeader { get; }

    protected virtual ICodeFileFactoryHeader<RoleFileType> BaseEventDTOFileFactoryHeader { get; } = ServerFileType.BaseEventDTO.ToHeader("", _ => "EventDTOBase");

    protected virtual ICodeFileFactoryHeader<DTOFileType> SimpleEventDTOFileFactoryHeader { get; } = ServerFileType.SimpleEventDTO.ToHeader(@"SimpleEventDTO\", domainType => domainType.Name + "EventSimpleDTO");

    protected virtual ICodeFileFactoryHeader<FileType> RichEventDTOFileFactoryHeader { get; } = ServerFileType.RichEventDTO.ToHeader(@"RichEventDTO\", domainType => domainType.Name + "EventRichDTO");

    protected virtual ICodeFileFactoryHeader<FileType> SimpleIntegrationDTOFileFactoryHeader { get; } = ServerFileType.SimpleIntegrationDTO.ToHeader(@"SimpleIntegrationDTO\", domainType => domainType.Name + "IntegrationSimpleDTO");

    protected virtual ICodeFileFactoryHeader<FileType> RichIntegrationDTOFileFactoryHeader { get; } = ServerFileType.RichIntegrationDTO.ToHeader(@"RichIntegrationDTO\", domainType => domainType.Name + "IntegrationRichDTO");


    //public virtual DTORole GeneratingRole { get; } = DTORole.All;


    public CodeTypeReference DTOMappingServiceInterfaceTypeReference => this.GetCodeTypeReference(null, ServerFileType.ServerDTOMappingServiceInterface);

    public CodeTypeReference BLLContextTypeReference => this.Environment.BLLCore.BLLContextInterfaceTypeReference;

    public ReadOnlyCollection<DTOFileType> LambdaConvertTypes => this._lazyLambdaConvertTypes.Value;




    public override IEnumerable<GenerateTypeMap> GetTypeMaps()
    {
        foreach (var baseTypeMap in base.GetTypeMaps())
        {
            yield return baseTypeMap;
        }

        yield return this.GetTypeMap(this.Environment.PersistentDomainObjectBaseType, ServerFileType.BaseEventDTO);

        foreach (var domainType in this.DomainTypes)
        {
            if (!domainType.IsProjection())
            {
                foreach (var eventOperation in this.EventOperationSource.GetEventOperations(domainType))
                {
                    yield return this.GetTypeMap(domainType, new DomainOperationEventDTOFileType(eventOperation));
                }

                yield return this.GetTypeMap(domainType, ServerFileType.SimpleEventDTO);
                yield return this.GetTypeMap(domainType, ServerFileType.RichEventDTO);

                yield return this.GetTypeMap(domainType, ServerFileType.SimpleIntegrationDTO);
                yield return this.GetTypeMap(domainType, ServerFileType.RichIntegrationDTO);
            }
        }
    }

    public override ILayerCodeTypeReferenceService GetLayerCodeTypeReferenceService(DTOFileType fileType)
    {
        if (fileType == null) throw new ArgumentNullException(nameof(fileType));

        if (fileType == ServerFileType.SimpleIntegrationDTO || fileType == ServerFileType.RichIntegrationDTO)
        {
            return new CryptCodeTypeReferenceService<ServerGeneratorConfigurationBase<TEnvironment>>(this, ServerFileType.SimpleIntegrationDTO, ServerFileType.RichIntegrationDTO);
        }
        else if (fileType == ServerFileType.SimpleEventDTO || fileType == ServerFileType.RichEventDTO)
        {
            return new CryptCodeTypeReferenceService<ServerGeneratorConfigurationBase<TEnvironment>>(this, ServerFileType.SimpleEventDTO, ServerFileType.RichEventDTO);
        }
        else
        {
            return base.GetLayerCodeTypeReferenceService(fileType);
        }
    }

    public CodeMethodReferenceExpression GetConvertToDTOMethod(Type domainType, FileType fileType)
    {
        if (domainType == null) throw new ArgumentNullException(nameof(domainType));
        if (fileType == null) throw new ArgumentNullException(nameof(fileType));

        return new CodeMethodReferenceExpression(this.GetCodeTypeReference(null, ServerFileType.LambdaHelper).ToTypeReferenceExpression(), "To" + fileType.Name);
    }

    public CodeMethodReferenceExpression GetConvertToDTOListMethod(Type domainType, FileType fileType)
    {
        if (domainType == null) throw new ArgumentNullException(nameof(domainType));
        if (fileType == null) throw new ArgumentNullException(nameof(fileType));

        return new CodeMethodReferenceExpression(this.GetCodeTypeReference(null, ServerFileType.LambdaHelper).ToTypeReferenceExpression(), "To" + fileType.Name + "List");
    }

    public bool CanCreateDomainObject(PropertyInfo property, Type elementType, DTOFileType fileType)
    {
        if (property == null) throw new ArgumentNullException(nameof(property));
        if (elementType == null) throw new ArgumentNullException(nameof(elementType));
        if (fileType == null) throw new ArgumentNullException(nameof(fileType));

        var createRoleAttribute = property.GetCustomAttribute<CreateRoleAttribute>() ?? property.DeclaringType.GetCustomAttribute<CreateRoleAttribute>();

        if (createRoleAttribute != null)
        {
            return createRoleAttribute.Value;
        }
        else
        {
            return this.GetAllowCreateAttributeType(fileType).Maybe(allowCreateAttr =>

                                                                            elementType.GetCustomAttributes(allowCreateAttr, true)
                                                                                       .OfType<IAllowCreateAttribute>().Any(attr => attr.AllowCreate));
        }
    }

    public Type GetAllowCreateAttributeType(DTOFileType fileType)
    {
        if (fileType == null) throw new ArgumentNullException(nameof(fileType));

        if (fileType.Role == DTORole.Integration)
        {
            return typeof(BLLIntegrationSaveRoleAttribute);
        }
        else if (fileType == FileType.StrictDTO || fileType == FileType.UpdateDTO || fileType is MainDTOFileType)
        {
            return typeof(BLLSaveRoleAttribute);
        }
        else
        {
            return null;
        }
    }

    public virtual CodeAttributeDeclaration GetDTOFileAttribute(Type domainType, RoleFileType fileType)
    {
        var baseAttr = new CodeAttributeDeclaration(
                                                    typeof(DTOFileTypeAttribute).ToTypeReference(),
                                                    domainType.ToTypeOfExpression().ToAttributeArgument(),
                                                    fileType.Name.ToPrimitiveExpression().ToAttributeArgument(),
                                                    fileType.Role.ToPrimitiveExpression().ToAttributeArgument());

        this.GetDTOFileAttributeExternalData(domainType, fileType)
            .MaybeString(externalData => baseAttr.Arguments.Add(new CodeAttributeArgument(nameof(DTOFileTypeAttribute.ExternalData), externalData.ToPrimitiveExpression())));

        return baseAttr;
    }

    protected virtual string GetDTOFileAttributeExternalData(Type domainType, RoleFileType fileType)
    {
        switch (fileType)
        {
            case DomainOperationEventDTOFileType eventFileType:
                return $"Operation = {eventFileType.EventOperation.Name}";

            default:
                return null;
        }
    }

    public override ICodeFileFactoryHeader GetFileFactoryHeader(FileType fileType, bool raiseIfNotFound = true)
    {
        if (fileType == null) throw new ArgumentNullException(nameof(fileType));

        if (fileType is DomainOperationEventDTOFileType)
        {
            var eventFileType = fileType as DomainOperationEventDTOFileType;

            return new CodeFileFactoryHeader<DomainOperationEventDTOFileType>(eventFileType, "EventOperation", domainType => $"{domainType.Name}{eventFileType.EventOperation.Name}{DTORole.Event}DTO");
        }
        else
        {
            return base.GetFileFactoryHeader(fileType, raiseIfNotFound);
        }
    }

    protected override IEnumerable<PropertyInfo> GetInternalDomainTypeProperties(Type domainType, DTOFileType baseFileType)
    {
        if (domainType == null) throw new ArgumentNullException(nameof(domainType));
        if (baseFileType == null) throw new ArgumentNullException(nameof(baseFileType));

        if (baseFileType is DomainOperationEventDTOFileType || baseFileType == ServerFileType.BaseEventDTO)
        {
            return new PropertyInfo[0];
        }

        var clientFileType = baseFileType == ServerFileType.SimpleEventDTO || baseFileType == ServerFileType.SimpleIntegrationDTO
                                     ? FileType.SimpleDTO
                                     : baseFileType == ServerFileType.RichEventDTO || baseFileType == ServerFileType.RichIntegrationDTO
                                             ? FileType.RichDTO
                                             : null;

        if (clientFileType != null)
        {
            return new[] { clientFileType }.Concat(clientFileType.GetBaseTypes(false))
                                           .SelectMany(fileType => base.GetInternalDomainTypeProperties(domainType, fileType, baseFileType.Role))
                                           .Distinct();
        }

        return base.GetInternalDomainTypeProperties(domainType, baseFileType);
    }

    protected override IEnumerable<ICodeFileFactoryHeader<FileType>> GetFileFactoryHeaders()
    {
        return base.GetFileFactoryHeaders().Concat(new[]
                                                   {
                                                           this.BaseEventDTOFileFactoryHeader,

                                                           this.SimpleEventDTOFileFactoryHeader,
                                                           this.RichEventDTOFileFactoryHeader,

                                                           this.SimpleIntegrationDTOFileFactoryHeader,
                                                           this.RichIntegrationDTOFileFactoryHeader,

                                                           this.LambdaHelperFileFactoryHeader,

                                                           this.ServerDTOMappingServiceInterfaceFileFactoryHeader,

                                                           this.ServerPrimitiveDTOMappingServiceBaseFileFactoryHeader,
                                                           this.ServerPrimitiveDTOMappingServiceFileFactoryHeader,
                                                   });
    }

    protected virtual IPropertyAssignerConfigurator GetPropertyAssignerConfigurator()
    {
        return new PropertyAssignerConfigurator<ServerGeneratorConfigurationBase<TEnvironment>>(this);
    }

    /// <summary>
    /// Возвращает типы преобразователей DTO.
    /// </summary>
    /// <returns><see cref="IEnumerable{DTOFileType}"/>.</returns>
    protected virtual IEnumerable<DTOFileType> GetLambdaConvertTypes()
    {
        return new[]
               {
                       FileType.IdentityDTO,
                       FileType.VisualDTO,
                       FileType.SimpleDTO,
                       FileType.FullDTO,
                       FileType.RichDTO,

                       FileType.ProjectionDTO,

                       ServerFileType.SimpleEventDTO,
                       ServerFileType.RichEventDTO,
                       ServerFileType.SimpleIntegrationDTO,
                       ServerFileType.RichIntegrationDTO
               };
    }


    public IEnumerable<Type> GetDomainTypeMasters(Type domainType, DTOFileType fileType, bool isWritable)
    {
        if (domainType == null) throw new ArgumentNullException(nameof(domainType));
        if (fileType == null) throw new ArgumentNullException(nameof(fileType));

        return this._domainTypeMastersCache.GetValue(domainType, fileType, isWritable);
    }


    private IEnumerable<Type> GetDomainTypeDetails(Type domainType, DTOFileType fileType, bool isWritable)
    {
        if (domainType == null) throw new ArgumentNullException(nameof(domainType));
        if (fileType == null) throw new ArgumentNullException(nameof(fileType));

        return this._domainTypeDetailsCache.GetValue(domainType, fileType, isWritable);
    }
}
