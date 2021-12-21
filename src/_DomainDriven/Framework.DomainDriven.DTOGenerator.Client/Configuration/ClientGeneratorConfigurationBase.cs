using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

using Framework.Core;
using Framework.DomainDriven.Generation.Domain;
using Framework.DomainDriven.Serialization;

using JetBrains.Annotations;

namespace Framework.DomainDriven.DTOGenerator.Client
{
    public abstract class ClientGeneratorConfigurationBase<TEnvironment> : GeneratorConfigurationBase<TEnvironment>, IClientGeneratorConfigurationBase<TEnvironment>

        where TEnvironment : class, IClientGenerationEnvironmentBase
    {
        private readonly Lazy<ReadOnlyCollection<Assembly>> _reuseTypesAssembliesLazy;

        private readonly Lazy<ReadOnlyCollection<Type>> _enumTypesLazy;

        private readonly Lazy<ReadOnlyCollection<Type>> _classTypesLazy;

        private readonly Lazy<ReadOnlyCollection<Type>> _structTypesLazy;

        private readonly Lazy<ReadOnlyCollection<Type>> _referencedTypesLazy;


        private readonly IReadOnlyDictionary<MainDTOFileType, MainDTOInterfaceFileType> _baseInterfaceFileTypeDict;


        protected ClientGeneratorConfigurationBase(TEnvironment environment)
            : base(environment)
        {
            this._reuseTypesAssembliesLazy = new Lazy<ReadOnlyCollection<Assembly>>(() =>
                this.GetReuseTypesAssemblies().ToReadOnlyCollection());

            this._referencedTypesLazy = LazyHelper.Create(() =>
                this.DomainTypes.GetReferencedTypes(property => !property.IsIgnored(DTORole.Client)).ToReadOnlyCollection());

            this._enumTypesLazy = LazyHelper.Create(() =>
            {
                var enumTypesRequest = from referencedType in this.ReferencedTypes

                                       where referencedType.IsEnum && !this.IsReused(referencedType)

                                       select referencedType;

                return enumTypesRequest.ToReadOnlyCollection();
            });

            this._classTypesLazy = LazyHelper.Create(() =>
            {
                var classTypesRequest = from referencedType in this.ReferencedTypes

                                        where referencedType.IsClass
                                           && !referencedType.IsGenericTypeDefinition
                                           && referencedType != typeof(string)
                                           && referencedType != typeof(object)
                                           && !referencedType.IsArray
                                           && !this.Environment.DomainObjectBaseType.IsAssignableFrom(referencedType)
                                           && !this.IsReused(referencedType)

                                        select referencedType;

                return classTypesRequest.Distinct().ToReadOnlyCollection();
            });

            this._structTypesLazy = LazyHelper.Create(() =>
            {
                var structTypesRequest = from referencedType in this.ReferencedTypes

                                         where referencedType.IsValueType
                                            && !referencedType.IsPrimitiveType()
                                            && !referencedType.IsEnum
                                            && !this.IsReused(referencedType)

                                         select referencedType;

                return structTypesRequest.ToReadOnlyCollection();
            });


            this._baseInterfaceFileTypeDict = new[]
            {
                ClientFileType.BaseAbstractInterfaceDTO,
                ClientFileType.BasePersistentInterfaceDTO,
                ClientFileType.SimpleInterfaceDTO,
                ClientFileType.FullInterfaceDTO,
                ClientFileType.RichInterfaceDTO
            }.ToDictionary(f => f.MainType);

            this.DefaultCodeTypeReferenceService = new ClientConfigurationCodeTypeReferenceService<ClientGeneratorConfigurationBase<TEnvironment>>(this);
        }


        public virtual bool ContainsPropertyChange { get; } = true;

        public override Type ClientEditCollectionType { get; } = typeof(ObservableCollection<>);

        public override ICodeTypeReferenceService DefaultCodeTypeReferenceService { get; }


        public ReadOnlyCollection<Assembly> ReuseTypesAssemblies => this._reuseTypesAssembliesLazy.Value;


        public ReadOnlyCollection<Type> ReferencedTypes => this._referencedTypesLazy.Value;


        public ReadOnlyCollection<Type> EnumTypes => this._enumTypesLazy.Value;

        public ReadOnlyCollection<Type> ClassTypes => this._classTypesLazy.Value;

        public ReadOnlyCollection<Type> StructTypes => this._structTypesLazy.Value;


        protected virtual ICodeFileFactoryHeader<FileType> EnumFileFactoryHeader { get; } = ClientFileType.Enum.ToHeader(@"Enums\", @enum => @enum.Name);

        protected virtual ICodeFileFactoryHeader<FileType> ClassFileFactoryHeader { get; } = ClientFileType.Struct.ToHeader(@"Structs\", @struct => @struct.Name);

        protected virtual ICodeFileFactoryHeader<FileType> StructFileFactoryHeader { get; } = ClientFileType.Class.ToHeader(@"Classes\", @class => @class.Name);



        protected virtual ICodeFileFactoryHeader<FileType> BaseAbstractInterfaceDTOFileFactoryHeader => ClientFileType.BaseAbstractInterfaceDTO.ToHeader(@"Interfaces\", type => $"I{this.BaseAbstractDTOFileFactoryHeader.GetName(type)}");

        protected virtual ICodeFileFactoryHeader<FileType> BasePersistentInterfaceDTOFileFactoryHeader => ClientFileType.BasePersistentInterfaceDTO.ToHeader(@"Interfaces\", (Func<Type, string>)(type => $"I{this.BasePersistentDTOFileFactoryHeader.GetName(type)}"));

        protected virtual ICodeFileFactoryHeader<FileType> BaseAuditPersistentInterfaceDTOFileFactoryHeader => ClientFileType.BaseAuditPersistentInterfaceDTO.ToHeader(@"Interfaces\", (Func<Type, string>)(type => $"I{this.BaseAuditPersistentDTOFileFactoryHeader.GetName(type)}"));


        protected virtual ICodeFileFactoryHeader<FileType> SimpleInterfaceDTOFileFactoryHeader => ClientFileType.SimpleInterfaceDTO.ToHeader(@"Interfaces\", type => $"I{this.SimpleDTOFileFactoryHeader.GetName(type)}");

        protected virtual ICodeFileFactoryHeader<FileType> FullInterfaceDTOFileFactoryHeader => ClientFileType.FullInterfaceDTO.ToHeader(@"Interfaces\", type => $"I{this.FullDTOFileFactoryHeader.GetName(type)}");

        protected virtual ICodeFileFactoryHeader<FileType> RichInterfaceDTOFileFactoryHeader => ClientFileType.RichInterfaceDTO.ToHeader(@"Interfaces\", type => $"I{this.RichDTOFileFactoryHeader.GetName(type)}");


        public override IEnumerable<GenerateTypeMap> GetTypeMaps()
        {
            foreach (var baseTypeMap in base.GetTypeMaps())
            {
                yield return baseTypeMap;
            }

            yield return this.GetTypeMap(this.Environment.DomainObjectBaseType, ClientFileType.BaseAbstractInterfaceDTO);

            yield return this.GetTypeMap(this.Environment.PersistentDomainObjectBaseType, ClientFileType.BasePersistentInterfaceDTO);

            yield return this.GetTypeMap(this.Environment.AuditPersistentDomainObjectBaseType, ClientFileType.BaseAuditPersistentInterfaceDTO);

            foreach (var type in this.DomainTypes)
            {
                yield return this.GetTypeMap(type, ClientFileType.SimpleInterfaceDTO);

                yield return this.GetTypeMap(type, ClientFileType.FullInterfaceDTO);

                yield return this.GetTypeMap(type, ClientFileType.RichInterfaceDTO);
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

        public virtual FileType GetBaseInterfaceType(MainDTOFileType fileType, bool raiseIfNull = false)
        {
            if (fileType == null) throw new ArgumentNullException(nameof(fileType));

            if (raiseIfNull)
            {
                return this._baseInterfaceFileTypeDict.GetValue(fileType, () => new System.ArgumentException(@"invalid fileType", nameof(fileType)));
            }
            else
            {
                return this._baseInterfaceFileTypeDict.GetValueOrDefault(fileType);
            }
        }

        public virtual IEnumerable<CodeTypeMember> GetFileFactoryExtendedMembers([NotNull] ICodeFileFactory<FileType> fileFactory)
        {
            if (fileFactory == null) throw new ArgumentNullException(nameof(fileFactory));

            yield break;
        }

        public override CodeTypeReference GetCodeTypeReference([NotNull] Type domainType, [NotNull] FileType fileType)
        {
            if (fileType == null) throw new ArgumentNullException(nameof(fileType));

            return domainType != null && this.IsReused(domainType)
                ? new CodeTypeReference(domainType)
                : base.GetCodeTypeReference(domainType, fileType);
        }

        protected override IEnumerable<PropertyInfo> GetInternalDomainTypeProperties([NotNull] Type domainType, [NotNull] DTOFileType fileType)
        {
            if (domainType == null) throw new ArgumentNullException(nameof(domainType));
            if (fileType == null) throw new ArgumentNullException(nameof(fileType));

            if (fileType == ClientFileType.Struct)
            {
                return domainType.GetSerializationProperties(fileType.Role);
            }
            else if (fileType == ClientFileType.Class)
            {
                return from property in domainType.GetSerializationProperties(fileType.Role)
                       where property.DeclaringType == domainType
                       select property;
            }
            else if (fileType is MainDTOInterfaceFileType)
            {
                var interfaceFileType = fileType as MainDTOInterfaceFileType;

                return this.GetDomainTypeProperties(domainType, interfaceFileType.MainType);
            }
            else
            {
                return base.GetInternalDomainTypeProperties(domainType, fileType);
            }
        }

        protected virtual IEnumerable<Assembly> GetReuseTypesAssemblies()
        {
            yield return typeof(Period).Assembly;
        }

        protected override IEnumerable<ICodeFileFactoryHeader<FileType>> GetFileFactoryHeaders()
        {
            foreach (var header in base.GetFileFactoryHeaders())
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
        }
    }
}
