using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Serialization;

using Framework.Core;
using Framework.DomainDriven.Serialization;
using Framework.Persistent;
using Framework.Restriction;
using Framework.Security;

namespace Framework.DomainDriven.SerializeMetadata
{
    public class SystemMetadataTypeBuilder<TPersistentDomainObjectBase> : ISystemMetadataTypeBuilder
    {
        private SystemMetadataTypeBuilder(Dictionary<TypeMetadata, Type> dict)
        {
            if (dict == null) throw new ArgumentNullException(nameof(dict));

            this.TypeResolver = TypeResolverHelper.Create(dict.ToDictionary(pair => pair.Key.Type, pair => pair.Value));

            this.SystemMetadata = new SystemMetadata(dict.Keys);

            this.AnonymousTypeBuilder = new AnonTypeBuilder(new AnonymousTypeBuilderStorage("systemMetadata")).WithCompressName()
                                                                                                              .WithGenerateNamePosfix()
                                                                                                              .OverrideInput((DomainTypeSubsetMetadata metaData) => new SubTypeMap(this, metaData))
                                                                                                              .WithCache()
                                                                                                              .WithLock();
        }

        public SystemMetadataTypeBuilder(DTORole dtoRole, IEnumerable<Assembly> assemblies)
            : this(GetTypeMetadatas(dtoRole, assemblies))
        {

        }


        public SystemMetadataTypeBuilder(DTORole dtoRole, params Assembly[] assemblies)
            : this(dtoRole, (IEnumerable<Assembly>)assemblies)
        {

        }

        public SystemMetadata SystemMetadata { get; }

        public virtual ITypeResolver<TypeHeader> TypeResolver { get; }

        public IAnonymousTypeBuilder<DomainTypeSubsetMetadata> AnonymousTypeBuilder { get; }

        private Type CreateType(TypeMetadata typeMetadata)
        {
            if (typeMetadata == null) throw new ArgumentNullException(nameof(typeMetadata));

            if (typeMetadata is DomainTypeSubsetMetadata)
            {
                return this.AnonymousTypeBuilder.GetAnonymousType(typeMetadata as DomainTypeSubsetMetadata);
            }
            else
            {
                return this.TypeResolver.Resolve(typeMetadata.Type, true);
            }
        }


        private static Dictionary<TypeMetadata, Type> GetTypeMetadatas(DTORole dtoRole, IEnumerable<Assembly> assemblies)
        {
            if (assemblies == null) throw new ArgumentNullException(nameof(assemblies));

            return assemblies.SelectMany(assembly => assembly.GetTypes())
                .Where(type => !type.IsAbstract && typeof(TPersistentDomainObjectBase).IsAssignableFrom(type))
                .GetReferencedTypes()
                .ToDictionary(type => GetTypeMetadata(dtoRole, type));
        }

        private static TypeMetadata GetTypeMetadata(DTORole dtoRole, Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            var header = new TypeHeader(type);

            if (type.IsEnum)
            {
                return new EnumMetadata(header,
                                        type.HasAttribute<FlagsAttribute>(),
                                        new TypeHeader(Enum.GetUnderlyingType(type)),
                                        EnumHelper.GetValues(type)
                                            .ToDictionary(value => Enum.GetName(type, value),
                                                          value => Convert.ChangeType(value, Enum.GetUnderlyingType(type)).ToString()));
            }

            if (type.Assembly == typeof(string).Assembly)
            {
                return new PrimitiveTypeMetadata(header);
            }


            var role = typeof(TPersistentDomainObjectBase).IsAssignableFrom(type) ? TypeRole.Domain : TypeRole.Other;

            return new DomainTypeMetadata(
                header,
                role,
                type.IsHierarchical(),

                from property in GetTypeProperties(dtoRole, type)

                let isVirtual = !property.HasPrivateField()

                let isSecurity = property.IsSecurity()

                let basePropType = property.PropertyType

                let propType = basePropType.GetCollectionOrArrayElementType()
                               ?? basePropType.GetNullableElementType()
                               ?? basePropType

                let isVisualIdentity = property.IsVisualIdentity()

                select new PropertyMetadata(property.Name, new TypeHeader(propType), basePropType.GetCollectionOrArrayElementType() != null
                                            , basePropType.IsNullable() || (basePropType.IsClass && (!property.HasAttribute<RequiredAttribute>())), isVirtual, isSecurity, isVisualIdentity));
        }


        private static IEnumerable<PropertyInfo> GetTypeProperties(DTORole dtoRole, Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            if (type.HasAttribute<DataContractAttribute>())
            {
                return from property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                       where property.HasAttribute<DataMemberAttribute>() && !property.GetIndexParameters().Any()
                       select property;
            }
            else
            {
                return type.GetSerializationProperties(dtoRole);
            }
        }



        private class AnonTypeBuilder : AnonymousTypeByPropertyWithSerializeBuilder<SubTypeMap, SubTypeMapProperty>
        {
            public AnonTypeBuilder(IAnonymousTypeBuilderStorage storage) : base(storage)
            {

            }

            protected override TypeBuilder DefineType(SubTypeMap typeMap)
            {
                var typeBuilder = base.DefineType(typeMap);

                typeBuilder.SetCustomAttribute(new CustomAttributeBuilder(typeof(SourceTypeNameAttribute).GetConstructors().Single(), new object[] { typeMap.BaseName }));

                return typeBuilder;
            }

            protected override PropertyBuilder ImplementMember(TypeBuilder typeBuilder, SubTypeMapProperty member)
            {
                var propertyBuilder = base.ImplementMember(typeBuilder, member);

                propertyBuilder.SetCustomAttribute(new CustomAttributeBuilder(typeof(SourcePropertyNameAttribute).GetConstructors().Single(), new object[] { member.BaseProperty.Name }));

                return propertyBuilder;
            }
        }

        private class SubTypeMap : ITypeMap<SubTypeMapProperty>, ISwitchNameObject<SubTypeMap>
        {
            private readonly ReadOnlyCollection<SubTypeMapProperty> _properties;

            private SubTypeMap(string name, string baseName, ReadOnlyCollection<SubTypeMapProperty> properties)
            {
                if (name == null) throw new ArgumentNullException(nameof(name));
                if (baseName == null) throw new ArgumentNullException(nameof(baseName));
                if (properties == null) throw new ArgumentNullException(nameof(properties));

                this.Name = name;
                this.BaseName = baseName;
                this._properties = properties;
            }

            public SubTypeMap(SystemMetadataTypeBuilder<TPersistentDomainObjectBase> builder, DomainTypeSubsetMetadata domainType)
            {
                if (builder == null) throw new ArgumentNullException(nameof(builder));
                if (domainType == null) throw new ArgumentNullException(nameof(domainType));

                this.BaseName = this.Name = domainType.Type.GenerateName;

                this._properties = domainType.Properties.ToReadOnlyCollection(prop =>
                {
                    var baseProp = builder.TypeResolver.Resolve(domainType.Type, true).GetProperty(prop.Name, true);

                    var baseType = builder.CreateType(prop.Type);

                    var resType = prop.IsCollection ? baseType.MakeArrayType()
                                : prop.AllowNull && baseType.IsValueType ? typeof(Nullable<>).MakeGenericType(baseType)
                                                                         : baseType;


                    var resTypeWithSec = baseProp.IsSecurity() ? typeof(Maybe<>).MakeGenericType(resType) : resType;

                    return new SubTypeMapProperty(prop.Alias ?? prop.Name, resTypeWithSec, baseProp);
                });
            }


            public string BaseName
            {
                get;
                private set;
            }

            public string Name
            {
                get; private set;
            }

            public IEnumerable<SubTypeMapProperty> Members
            {
                get { return this._properties; }
            }

            public SubTypeMap SwitchName(string newName)
            {
                return new SubTypeMap(newName, this.BaseName, this._properties);
            }
        }

        private class SubTypeMapProperty : TypeMapMemberBase
        {
            public SubTypeMapProperty(string name, Type type, PropertyInfo baseProperty)
                : base(name, type)
            {
                if (baseProperty == null) throw new ArgumentNullException(nameof(baseProperty));

                this.BaseProperty = baseProperty;
            }


            public PropertyInfo BaseProperty { get; private set; }
        }
    }
}
