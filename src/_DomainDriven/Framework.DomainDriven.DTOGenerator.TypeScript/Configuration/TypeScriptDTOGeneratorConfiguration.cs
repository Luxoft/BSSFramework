using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

using Framework.CodeDom;
using Framework.Core;
using Framework.DomainDriven.DTOGenerator.TypeScript.CodeTypeReferenceService;
using Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory;
using Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Base.ByProperty;
using Framework.DomainDriven.Generation.Domain;
using Framework.DomainDriven.Serialization;
using Framework.Transfering;

using JetBrains.Annotations;

namespace Framework.DomainDriven.DTOGenerator.TypeScript.Configuration;

/// <summary>
///  Client generator configuration base
/// </summary>
/// <typeparam name="TEnvironment">The type of the environment.</typeparam>
public abstract class TypeScriptDTOGeneratorConfiguration<TEnvironment> : GeneratorConfigurationBase<TEnvironment>, ITypeScriptDTOGeneratorConfiguration<TEnvironment>
        where TEnvironment : class, ITypeScriptGenerationEnvironmentBase
{
    private readonly Lazy<ReadOnlyCollection<Assembly>> reuseTypesAssembliesLazy;

    private readonly Lazy<ReadOnlyCollection<Type>> enumTypesLazy;

    private readonly Lazy<ReadOnlyCollection<Type>> classTypesLazy;

    private readonly Lazy<ReadOnlyCollection<Type>> structTypesLazy;

    private readonly Lazy<ReadOnlyCollection<Type>> referencedTypesLazy;


    protected TypeScriptDTOGeneratorConfiguration(TEnvironment generationEnvironment)
            : base(generationEnvironment)
    {
        this.reuseTypesAssembliesLazy = new Lazy<ReadOnlyCollection<Assembly>>(() =>
                                                                                       this.GetReuseTypesAssemblies().ToReadOnlyCollection());

        this.referencedTypesLazy = LazyHelper.Create(() =>
                                                             this.DomainTypes.GetReferencedTypes(property => !property.IsIgnored(DTORole.Client)).ToReadOnlyCollection());

        this.enumTypesLazy = LazyHelper.Create(() =>
                                               {
                                                   var enumTypesRequest = from referencedType in this.ReferencedTypes
                                                                          where referencedType.IsEnum && !this.IsReused(referencedType)
                                                                          select referencedType;

                                                   var allEnums = this.Environment.DomainObjectAssemblies.SelectMany(x => x.GetTypes()).Where(x => x.IsEnum).Select(x => x).ToList();
                                                   allEnums.AddRange(enumTypesRequest);

                                                   return allEnums.Distinct().ToReadOnlyCollection();
                                               });

        this.classTypesLazy = LazyHelper.Create(() =>
                                                {
                                                    var classTypesRequest = from referencedType in this.ReferencedTypes
                                                                            where referencedType.IsClass
                                                                                  && referencedType != typeof(string)
                                                                                  && referencedType != typeof(object)
                                                                                  && !referencedType.IsArray
                                                                                  && !this.Environment.DomainObjectBaseType.IsAssignableFrom(referencedType)
                                                                                  && !this.IsReused(referencedType)
                                                                            select referencedType;

                                                    return classTypesRequest.Distinct().ToReadOnlyCollection();
                                                });

        this.structTypesLazy = LazyHelper.Create(() =>
                                                 {
                                                     var structTypesRequest = from referencedType in this.ReferencedTypes
                                                                              where referencedType.IsValueType
                                                                                    && !referencedType.IsPrimitiveType()
                                                                                    && !referencedType.IsEnum
                                                                                    && !this.IsReused(referencedType)
                                                                              select referencedType;

                                                     return structTypesRequest.ToReadOnlyCollection();
                                                 });

        this.DefaultCodeTypeReferenceService = new ClientCodeTypeReferenceService<TypeScriptDTOGeneratorConfiguration<TEnvironment>>(this);
    }


    public override string Namespace => string.Empty;

    protected override string NamespacePostfix => string.Empty;

    public override string DataContractNamespace => string.Empty;

    /// <inheritdoc />
    public virtual bool GenerateClientMappingService { get; } = false;

    public bool ContainsPropertyChange => false;

    public override Type CollectionType => typeof(Array);

    public override ICodeTypeReferenceService DefaultCodeTypeReferenceService { get; }

    public ReadOnlyCollection<Assembly> ReuseTypesAssemblies => this.reuseTypesAssembliesLazy.Value;

    public ReadOnlyCollection<Type> ReferencedTypes => this.referencedTypesLazy.Value;

    public ReadOnlyCollection<Type> EnumTypes => this.enumTypesLazy.Value;

    public ReadOnlyCollection<Type> ClassTypes => this.classTypesLazy.Value;

    public ReadOnlyCollection<Type> StructTypes => this.structTypesLazy.Value;

    protected virtual ICodeFileFactoryHeader<FileType> EnumFileFactoryHeader
    {
        get { return ClientFileType.Enum.ToHeader(@"Enums\", @enum => @enum.Name); }
    }

    protected virtual ICodeFileFactoryHeader<FileType> ClassFileFactoryHeader
    {
        get { return ClientFileType.Struct.ToHeader(@"Structs\", @struct => @struct.Name); }
    }

    protected virtual ICodeFileFactoryHeader<FileType> StructFileFactoryHeader
    {
        get { return ClientFileType.Class.ToHeader(@"Classes\", @class => @class.Name); }
    }

    protected virtual ICodeFileFactoryHeader<FileType> BaseAbstractInterfaceDTOFileFactoryHeader
    {
        get { return ClientFileType.BaseAbstractInterfaceDTO.ToHeader(@"Interfaces\", _ => "I" + FileType.BaseAbstractDTO); }
    }

    protected virtual ICodeFileFactoryHeader<FileType> BasePersistentInterfaceDTOFileFactoryHeader
    {
        get { return ClientFileType.BasePersistentInterfaceDTO.ToHeader(@"Interfaces\", type => "I" + this.BasePersistentDTOFileFactoryHeader.GetName(type)); }
    }

    protected virtual ICodeFileFactoryHeader<FileType> BaseAuditPersistentInterfaceDTOFileFactoryHeader
    {
        get { return ClientFileType.BaseAuditPersistentInterfaceDTO.ToHeader(@"Interfaces\", type => "I" + this.BaseAuditPersistentDTOFileFactoryHeader.GetName(type)); }
    }

    protected virtual ICodeFileFactoryHeader<FileType> SimpleInterfaceDTOFileFactoryHeader
    {
        get { return ClientFileType.SimpleInterfaceDTO.ToHeader(@"Interfaces\", type => "I" + this.SimpleDTOFileFactoryHeader.GetName(type)); }
    }

    protected virtual ICodeFileFactoryHeader<FileType> FullInterfaceDTOFileFactoryHeader
    {
        get { return ClientFileType.FullInterfaceDTO.ToHeader(@"Interfaces\", type => "I" + this.FullDTOFileFactoryHeader.GetName(type)); }
    }

    protected virtual ICodeFileFactoryHeader<FileType> RichInterfaceDTOFileFactoryHeader
    {
        get { return ClientFileType.RichInterfaceDTO.ToHeader(@"Interfaces\", type => "I" + this.RichDTOFileFactoryHeader.GetName(type)); }
    }

    protected override IEnumerable<Type> GetProjectionTypes()
    {
        foreach (var baseType in base.GetProjectionTypes())
        {
            if (baseType is BaseTypeImpl genType)
            {
                yield return genType.TryGetRealType() ?? baseType;
            }
            else
            {
                yield return baseType;
            }
        }
    }

    public virtual IEnumerable<RequireJsModule> GetModules()
    {
        var data = new List<RequireJsModule>
                   {
                           new RequireJsModule("{ Guid, Convert }", "luxite/system", string.Empty),
                           new RequireJsModule("* as Framework", "luxite/framework/framework", string.Empty),
                           new RequireJsModule("{ observable, observableArray, unwrap }", "knockout", string.Empty),
                           new RequireJsModule("{ Core }", "luxite/framework/framework")
                   };
        return data;
    }

    /* End Facade generation usage */

    public virtual IEnumerable<string> GetNamespaces()
    {
        var result = new List<string>();

        // adding typescript interfaces referecne
        result.AddRange(
                        this.GetModules()
                            .Where(x => !string.IsNullOrEmpty(x.InterfacePath)).Select(x => $"/// <reference path=\"{x.InterfacePath}\"/>"));

        // adding require js modules reference
        result.AddRange(
                        this.GetModules()
                            .Select(x => $"import {x.Name} from '{x.ReferencePath}';"));
        return result;
    }

    public virtual MainDTOInterfaceFileType GetBaseInterfaceType(MainDTOFileType fileType)
    {
        if (fileType == null)
        {
            throw new ArgumentNullException(nameof(fileType));
        }

        if (fileType == FileType.BaseAbstractDTO || fileType == ObservableFileType.BaseObservableAbstractDTO)
        {
            return ClientFileType.BaseAbstractInterfaceDTO;
        }

        if (fileType == FileType.BasePersistentDTO || fileType == ObservableFileType.BaseObservablePersistentDTO)
        {
            return ClientFileType.BasePersistentInterfaceDTO;
        }

        if (fileType == FileType.BaseAuditPersistentDTO || fileType == ObservableFileType.BaseObservableAuditPersistentDTO)
        {
            return ClientFileType.BaseAuditPersistentInterfaceDTO;
        }

        if (fileType == FileType.SimpleDTO || fileType == ObservableFileType.ObservableSimpleDTO)
        {
            return ClientFileType.SimpleInterfaceDTO;
        }

        if (fileType == FileType.FullDTO || fileType == ObservableFileType.ObservableFullDTO)
        {
            return ClientFileType.FullInterfaceDTO;
        }

        if (fileType == FileType.RichDTO || fileType == ObservableFileType.ObservableRichDTO)
        {
            return ClientFileType.RichInterfaceDTO;
        }

        if (fileType == ObservableFileType.ObservableVisualDTO || fileType == FileType.VisualDTO)
        {
            return ClientFileType.BasePersistentInterfaceDTO;
        }

        if (fileType == ObservableFileType.ObservableProjectionDTO || fileType == FileType.ProjectionDTO)
        {
            return ClientFileType.BasePersistentInterfaceDTO;
        }

        if (fileType == FileType.BasePersistentDTO || fileType == ObservableFileType.BaseObservablePersistentDTO)
        {
            return ClientFileType.BasePersistentInterfaceDTO;
        }

        if (fileType == ObservableFileType.BaseObservablePersistentDTO)
        {
            return ObservableFileType.BaseObservablePersistentInterfaceDTO;
        }

        if (fileType == ObservableFileType.BaseObservableAuditPersistentDTO)
        {
            return ObservableFileType.BaseObservableAuditPersistentInterfaceDTO;
        }

        if (fileType == ObservableFileType.ObservableSimpleDTO)
        {
            return ObservableFileType.ObservableSimpleInterfaceDTO;
        }

        if (fileType == ObservableFileType.ObservableFullDTO)
        {
            return ObservableFileType.ObservableFullInterfaceDTO;
        }

        if (fileType == ObservableFileType.ObservableRichDTO)
        {
            return ObservableFileType.ObservableRichInterfaceDTO;
        }

        throw new ArgumentException(@"invalid fileType", nameof(fileType));
    }

    public virtual MainDTOFileType GetImplementType(MainDTOInterfaceFileType fileType)
    {
        if (fileType == null)
        {
            throw new ArgumentNullException(nameof(fileType));
        }

        if (fileType == ClientFileType.BaseAbstractInterfaceDTO)
        {
            return FileType.BaseAbstractDTO;
        }

        if (fileType == ClientFileType.BasePersistentInterfaceDTO)
        {
            return FileType.BasePersistentDTO;
        }

        if (fileType == ClientFileType.BaseAuditPersistentInterfaceDTO)
        {
            return FileType.BaseAuditPersistentDTO;
        }

        if (fileType == ClientFileType.SimpleInterfaceDTO)
        {
            return FileType.SimpleDTO;
        }

        if (fileType == ClientFileType.FullInterfaceDTO)
        {
            return FileType.FullDTO;
        }

        if (fileType == ClientFileType.RichInterfaceDTO)
        {
            return FileType.RichDTO;
        }

        if (fileType == ObservableFileType.ObservableVisualDTO)
        {
            return ObservableFileType.ObservableVisualDTO;
        }

        //if (fileType == ObservableFileType.ObservableProjectionDTO)
        //{
        //    return ObservableFileType.ObservableProjectionDTO;
        //}

        if (fileType == ObservableFileType.BaseObservableAuditPersistentInterfaceDTO)
        {
            return ObservableFileType.BaseObservableAuditPersistentDTO;
        }

        if (fileType == ObservableFileType.BaseObservablePersistentInterfaceDTO)
        {
            return ObservableFileType.BaseObservablePersistentDTO;
        }

        if (fileType == ObservableFileType.ObservableSimpleInterfaceDTO)
        {
            return ObservableFileType.ObservableSimpleDTO;
        }

        if (fileType == ObservableFileType.ObservableFullInterfaceDTO)
        {
            return ObservableFileType.ObservableFullDTO;
        }

        if (fileType == ObservableFileType.ObservableRichInterfaceDTO)
        {
            return ObservableFileType.ObservableRichDTO;
        }

        //if (fileType == ObservableFileType.ObservableIdentityDTO)
        //{
        //    return ObservableFileType.ObservableIdentityDTO;
        //}

        throw new ArgumentException(@"invalid fileType", nameof(fileType));
    }

    protected override IEnumerable<PropertyInfo> GetInternalDomainTypeProperties([NotNull] Type domainType, [NotNull] DTOFileType fileType)
    {
        if (domainType == null)
        {
            throw new ArgumentNullException(nameof(domainType));
        }

        if (fileType == null)
        {
            throw new ArgumentNullException(nameof(fileType));
        }

        if (fileType == ClientFileType.Struct)
        {
            return domainType.GetSerializationProperties();
        }

        if (fileType == ClientFileType.Class)
        {
            return from property in domainType.GetSerializationProperties()

                   where property.DeclaringType == domainType

                   select property;
        }

        if (fileType == ObservableFileType.BaseObservableAbstractDTO)
        {
            return this.GetDomainTypeProperties(domainType, FileType.BaseAbstractDTO);
        }

        if (fileType == ObservableFileType.ObservableIdentityDTO)
        {
            return this.GetDomainTypeProperties(domainType, FileType.IdentityDTO);
        }

        if (fileType == ObservableFileType.ObservableVisualDTO)
        {
            return this.GetDomainTypeProperties(domainType, FileType.VisualDTO);
        }

        if (fileType == ObservableFileType.ObservableProjectionDTO)
        {
            return this.GetDomainTypeProperties(domainType, FileType.ProjectionDTO);
        }

        if (fileType == ObservableFileType.BaseObservablePersistentDTO)
        {
            return this.GetDomainTypeProperties(domainType, FileType.BasePersistentDTO);
        }

        if (fileType == ObservableFileType.BaseObservableAuditPersistentDTO)
        {
            return this.GetDomainTypeProperties(domainType, FileType.BaseAuditPersistentDTO);
        }

        if (fileType == ObservableFileType.ObservableSimpleDTO)
        {
            return this.GetDomainTypeProperties(domainType, FileType.SimpleDTO);
        }

        if (fileType == ObservableFileType.ObservableFullDTO)
        {
            return this.GetDomainTypeProperties(domainType, FileType.FullDTO);
        }

        if (fileType == ObservableFileType.ObservableRichDTO)
        {
            return this.GetDomainTypeProperties(domainType, FileType.RichDTO);
        }

        var internalProperties = base.GetInternalDomainTypeProperties(domainType, fileType);

        return internalProperties.Distinct();
    }

    protected virtual IEnumerable<Assembly> GetReuseTypesAssemblies()
    {
        yield return typeof(Period).Assembly;
    }

    protected override IEnumerable<ICodeFileFactoryHeader<FileType>> GetFileFactoryHeaders()
    {
        foreach (var header in base.GetFileFactoryHeaders().Where(x => x.Type != FileType.ProjectionDTO))
        {
            yield return header;
        }

        yield return this.EnumFileFactoryHeader;
        yield return this.StructFileFactoryHeader;
        yield return this.ClassFileFactoryHeader;

        yield return this.BaseAbstractInterfaceDTOFileFactoryHeader;
        yield return this.BasePersistentInterfaceDTOFileFactoryHeader;
        yield return this.BaseAuditPersistentInterfaceDTOFileFactoryHeader;
        yield return this.SimpleInterfaceDTOFileFactoryHeader;
        yield return this.FullInterfaceDTOFileFactoryHeader;
        yield return this.RichInterfaceDTOFileFactoryHeader;

        yield return ClientFileType.ProjectionDTO.ToHeader();


        yield return this.ObservablePersistentInterfaceDTOFileFactoryHeader;
        yield return this.ObservableAuditPersistentInterfaceDTOFileFactoryHeader;
        yield return this.ObservableSimpleInterfaceDTOFileFactoryHeader;
        yield return this.ObservableFullInterfaceDTOFileFactoryHeader;
        yield return this.ObservableRichInterfaceDTOFileFactoryHeader;

        yield return ObservableFileType.BaseObservableAbstractDTO.ToHeader(string.Empty, _ => ObservableFileType.BaseObservableAbstractDTO.Name);
        yield return ObservableFileType.ObservableIdentityDTO.ToHeader();
        yield return ObservableFileType.ObservableVisualDTO.ToHeader();
        yield return ObservableFileType.BaseObservablePersistentDTO.ToHeader(string.Empty, _ => ObservableFileType.BaseObservablePersistentDTO.Name);
        yield return ObservableFileType.BaseObservableAuditPersistentDTO.ToHeader(string.Empty, _ => ObservableFileType.BaseObservableAuditPersistentDTO.Name);
        yield return ObservableFileType.ObservableSimpleDTO.ToHeader();
        yield return ObservableFileType.ObservableFullDTO.ToHeader();
        yield return ObservableFileType.ObservableRichDTO.ToHeader();

        yield return ObservableFileType.ObservableProjectionDTO.ToHeader(ObservableFileType.ObservableProjectionDTO + @"\", domainType => domainType.Name.SkipLast("Projection", false) + ObservableFileType.ObservableProjectionDTO);
    }







    public override CodeTypeReference GetCodeTypeReference(Type domainType, FileType fileType)
    {
        var preResult = domainType != null && this.IsReused(domainType) ? new CodeTypeReference(domainType) : base.GetCodeTypeReference(domainType, fileType);

        preResult.BaseType = preResult.BaseType.TrimStart(new char[] { '.' });

        return preResult;
    }




    protected virtual ICodeFileFactoryHeader<FileType> ObservableVisualDTOFileFactoryHeader => ObservableFileType.ObservableVisualDTO.ToHeader();

    protected virtual ICodeFileFactoryHeader<FileType> ObservableSimpleDTOFileFactoryHeader => ObservableFileType.ObservableSimpleDTO.ToHeader();

    protected virtual ICodeFileFactoryHeader<FileType> ObservableFullDTOFileFactoryHeader => ObservableFileType.ObservableFullDTO.ToHeader();

    protected virtual ICodeFileFactoryHeader<FileType> ObservableRichDTOFileFactoryHeader => ObservableFileType.ObservableRichDTO.ToHeader();

    protected virtual ICodeFileFactoryHeader<FileType> ObservableIdentityDTOFileFactoryHeader => ObservableFileType.ObservableIdentityDTO.ToHeader();

    protected virtual ICodeFileFactoryHeader<FileType> ObservableSimpleInterfaceDTOFileFactoryHeader
    {
        get { return ObservableFileType.ObservableSimpleInterfaceDTO.ToHeader(@"Interfaces\", type => "I" + this.ObservableSimpleDTOFileFactoryHeader.GetName(type)); }
    }

    protected virtual ICodeFileFactoryHeader<FileType> ObservableFullInterfaceDTOFileFactoryHeader
    {
        get { return ObservableFileType.ObservableFullInterfaceDTO.ToHeader(@"Interfaces\", type => "I" + this.ObservableFullDTOFileFactoryHeader.GetName(type)); }
    }

    protected virtual ICodeFileFactoryHeader<FileType> ObservableRichInterfaceDTOFileFactoryHeader
    {
        get { return ObservableFileType.ObservableRichInterfaceDTO.ToHeader(@"Interfaces\", type => "I" + this.ObservableRichDTOFileFactoryHeader.GetName(type)); }
    }

    protected virtual ICodeFileFactoryHeader<FileType> ObservablePersistentInterfaceDTOFileFactoryHeader
    {
        get { return ObservableFileType.BaseObservablePersistentInterfaceDTO.ToHeader(@"Interfaces\", _ => "I" + ObservableFileType.BaseObservablePersistentDTO.Name); }
    }

    protected virtual ICodeFileFactoryHeader<FileType> ObservableAuditPersistentInterfaceDTOFileFactoryHeader
    {
        get { return ObservableFileType.BaseObservableAuditPersistentInterfaceDTO.ToHeader(@"Interfaces\", _ => "I" + ObservableFileType.BaseObservableAuditPersistentDTO.Name); }
    }

    public virtual IEnumerable<CodeTypeMember> GetFileFactoryExtendedMembers(ICodeFileFactory<FileType> fileFactory)
    {
        if (fileFactory == null)
        {
            throw new ArgumentNullException(nameof(fileFactory));
        }

        if (fileFactory is IPropertyFileFactory || fileFactory is ITypeScriptIdentityDTOFileFactory)
        {
            yield return new CodeMemberField
                         {
                                 Name = "__type",
                                 Type = typeof(string).ToTypeReference(),
                                 Attributes = MemberAttributes.Public,
                                 InitExpression = new CodePrimitiveExpression(fileFactory.Name)
                         };

            yield return new CodeMemberField
                         {
                                 Name = Constants.GenerateTypeIdenity(fileFactory.Name),
                                 Type = typeof(string).ToTypeReference(),
                                 Attributes = MemberAttributes.Private,
                         };
        }
    }

    public override ILayerCodeTypeReferenceService GetLayerCodeTypeReferenceService(DTOFileType fileType)
    {
        if (fileType is ObservableMainDTOFileType)
        {
            return new ObservableCodeTypeReferenceService<TypeScriptDTOGeneratorConfiguration<TEnvironment>>(this);
        }

        return base.GetLayerCodeTypeReferenceService(fileType);
    }

    public override IEnumerable<GenerateTypeMap> GetTypeMaps()
    {
        foreach (var baseTypeMap in base.GetTypeMaps())
        {
            yield return baseTypeMap;

            var observableFileType = baseTypeMap.FileType.AsObservableFileType();

            if (observableFileType != null)
            {
                yield return this.GetTypeMap(baseTypeMap.DomainType, observableFileType);
            }
        }

        foreach (var type in this.ClassTypes)
        {
            yield return this.GetTypeMap(type, ClientFileType.Class);
        }

        foreach (var type in this.StructTypes)
        {
            yield return this.GetTypeMap(type, ClientFileType.Struct);
        }
    }

    public override CodeExpression GetCreateUpdateDTOExpression(
            Type domainType,
            CodeExpression currentStrictSource,
            CodeExpression baseStrictSource,
            CodeExpression mappingService)
    {
        var updateTypeRef = this.GetCodeTypeReference(domainType, DTOType.UpdateDTO);

        return updateTypeRef.ToTypeReferenceExpression()
                            .ToMethodInvokeExpression(
                                                      "fromStrict",
                                                      currentStrictSource,
                                                      baseStrictSource ?? new CodePrimitiveExpression(null),
                                                      mappingService);
    }
}
