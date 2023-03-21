using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

using Framework.Core;
using Framework.DomainDriven.Attributes;
using Framework.DomainDriven.Common;
using Framework.DomainDriven.DAL.Sql;
using Framework.DomainDriven.Metadata;
using Framework.Persistent.Mapping;
using Framework.Restriction;

using JetBrains.Annotations;

using Microsoft.SqlServer.Management.Smo;

namespace Framework.DomainDriven.NHibernate.DALGenerator;

public class MappingGenerator : IMappingGenerator
{
    private const string NHibernateNamespace = "nhibernate-mapping-2.2";
    private const string NHibernateMappingRootName = "hibernate-mapping";
    private const string AssemblyName = "assembly";
    private const string SchemaName = "schema";
    private const string ClassName = "class";

    private static readonly XNamespace RootNameNamespace = "urn:" + NHibernateNamespace;

    private readonly IGrouping<IAssemblyInfo, DomainTypeMetadata> assemblyGroup;
    private readonly DatabaseName schema;

    private readonly bool useSmartUpdate;

    public MappingGenerator([NotNull] IGrouping<IAssemblyInfo, DomainTypeMetadata> assemblyGroup, [NotNull] DatabaseName schema, bool useSmartUpdate)
    {
        this.assemblyGroup = assemblyGroup ?? throw new ArgumentNullException(nameof(assemblyGroup));
        this.schema = schema ?? throw new ArgumentNullException(nameof(schema));
        this.useSmartUpdate = useSmartUpdate;
    }

    public IAssemblyInfo Assembly => this.assemblyGroup.Key;

    public XDocument Generate()
    {
        var root = this.CreateRoot();

        var document = new XDocument(root);

        this.assemblyGroup
            .Where(z => !z.DomainType.GetCustomAttributes<IgnoreMappingAttribute>().Any())
            .Where(z => !z.DomainType.GetCustomAttributes<IgnoreHbmMappingAttribute>().Any())
            .Concat(this.assemblyGroup.SelectMany(z=>z.NotAbstractChildrenDomainTypes.GetAllElements(q=>q.NotAbstractChildrenDomainTypes)).Where(z=>z.IsInlineBaseType))
            .OrderBy(meta => meta.DomainType.FullName)
            .Foreach(z => this.GenerateClassMapping(z, root, null));

        return document.OverrideChildrenNamespace();
    }

    protected virtual string GetColumnNameBy(DomainTypeMetadata domainTypeMetadata, FieldMetadata field)
    {
        var result = field.ToColumnName();
        return result;
    }

    protected virtual void GenerateClassMapping(DomainTypeMetadata domainTypeMetadata, XElement root, string externalTableName)
    {
        var classTag = this.CreateClassElement(domainTypeMetadata, root);

        this.CreateIdMapping(domainTypeMetadata, domainTypeMetadata.GetIdentityField(), classTag);

        this.GenerateMappingForTypeAttributes(domainTypeMetadata, classTag, externalTableName);
        this.GenerateMappingForChildrenTypes(domainTypeMetadata, classTag);
        this.GenerateMappingForExternalTableColumns(domainTypeMetadata, classTag);
    }

    protected virtual void GenerateMappingForTypeAttributes(DomainTypeMetadata domainTypeMetadata, XElement classTag, string externalTableName)
    {
        this.GenerateMappingForVersionField(domainTypeMetadata, classTag, externalTableName);
        this.GenerateMappingForPrimitiveFields(domainTypeMetadata, classTag, externalTableName);
        this.GenerateMappingForReferenceFields(domainTypeMetadata, classTag, externalTableName);
        this.GenerateMappingForDetailsCollectionFields(domainTypeMetadata, classTag, externalTableName);
        this.GenerateMappingInlineFields(domainTypeMetadata, classTag, externalTableName);
    }

    protected virtual void GenerateMappingForDetailsCollectionFields(DomainTypeMetadata domainTypeMetadata, XElement classTag, string externalTableName)
    {
        var listTypeFieldMetadatas = domainTypeMetadata.ListFields;

        if (domainTypeMetadata.IsInlineBaseType)
        {
            listTypeFieldMetadatas = domainTypeMetadata.GetAllElements(z => z.Parent).SelectMany(z => z.ListFields);
        }

        listTypeFieldMetadatas.OrderBy(x => x.Name).Where(v => v.ExternalTableName == externalTableName).Foreach(z => this.GenerateOneToManyPropertyMapping(z, classTag));
    }

    protected virtual void GenerateMappingInlineFields(DomainTypeMetadata domainTypeMetadata, XElement classTag, string externalTableName)
    {
        var inlineTypeFieldMetadatas = domainTypeMetadata.InlineFields;

        if (domainTypeMetadata.IsInlineBaseType)
        {
            inlineTypeFieldMetadatas = domainTypeMetadata.GetAllElements(z => z.Parent).SelectMany(z => z.InlineFields);
        }

        inlineTypeFieldMetadatas.OrderBy(x => x.Name).Where(v => v.ExternalTableName == externalTableName).Foreach(z =>
        {
            this.GenerateInlineFieldMapping(z, classTag);
        });
    }

    protected virtual void GenerateMappingForChildrenTypes(DomainTypeMetadata domainTypeMetadata, XElement rootElement)
    {
        domainTypeMetadata.NotAbstractChildrenDomainTypes
                          .Where(z=>!z.IsInlineBaseType)
                          .OrderBy(x => x.DomainType.FullName)
                          .Foreach(z => this.GenerateMappingForChildType(z, rootElement));
    }

    protected virtual void GenerateMappingForChildType(DomainTypeMetadata domainTypeMetadata, XElement rootElement)
    {
        var joinedSubclassElement = rootElement
                                    .CreateElementWithRootNamespaceHandled("joined-subclass")
                                    .WithLowNameAttribute(domainTypeMetadata.DomainType.FullName)
                                    .WithTableElement(domainTypeMetadata);

        this.TryInitDynamicUpdateForOptimisticDeclareType(domainTypeMetadata, joinedSubclassElement);

        joinedSubclassElement.CreateElementWithRootNamespaceHandled("key")
                             .WithColumnAttribute(this.GetColumnNameBy(domainTypeMetadata, domainTypeMetadata.GetIdentityField()));
        this.GenerateMappingForTypeAttributes(domainTypeMetadata, joinedSubclassElement, null);
    }

    protected virtual void GenerateMappingForExternalTableColumns(DomainTypeMetadata domainTypeMetadata, XElement rootElement)
    {
        domainTypeMetadata.Fields.GroupBy(field => field.ExternalTableName).Where(g => g.Key != null).Foreach(g =>
        {
            var joinedSubclassElement = rootElement
                                        .CreateElementWithRootNamespaceHandled("join")
                                        .WithAttribute("table", g.Key);

            joinedSubclassElement.CreateElement("key").WithAttribute("column", "id");

            this.GenerateMappingForTypeAttributes(domainTypeMetadata, joinedSubclassElement, g.Key);
        });
    }

    protected virtual void GenerateMappingForReferenceFields(DomainTypeMetadata domainTypeMetadata, XElement classTag, string externalTableName)
    {
        var referenceTypeFieldMetadatas = domainTypeMetadata.ReferenceFields;

        if (domainTypeMetadata.IsInlineBaseType)
        {
            referenceTypeFieldMetadatas = domainTypeMetadata.GetAllElements(z => z.Parent).SelectMany(z => z.ReferenceFields);
        }


        referenceTypeFieldMetadatas.Where(v => v.ExternalTableName == externalTableName).OrderBy(x => x.Name).Foreach(z =>
        {
            if (z.IsOneToOneReference)
            {
                this.GenerateOneToOnePropertyMapping(z, classTag);
            }
            else
            {
                this.GenerateManyToOnePropertyMapping(z, classTag);
            }
        });
    }

    protected virtual void GenerateOneToOnePropertyMapping(ReferenceTypeFieldMetadata fieldMetadata, XElement rootElement)
    {
        var toDomainTypeMetadata = fieldMetadata.DomainTypeMetadata.AssemblyMetadata.DomainTypes.Single(
         z => z.DomainType == fieldMetadata.ToType);
        var reverseRefProperty = toDomainTypeMetadata.ReferenceFields.Single(z => z.ToType == fieldMetadata.FromType);

        var refTag = rootElement.CreateOneToOneElementWithRootNamespace()
                                .WithLowNameAttribute(fieldMetadata.Name.ToPropertyName())
                                .WithClassAttribute(fieldMetadata.ToType.FullName)
                                .WithTryUniqueAttribute(fieldMetadata)
                                .WithPropertyRefAttribute(reverseRefProperty.Name.ToPropertyName());

        this.TryApplyCascadeMode(fieldMetadata, refTag);
    }

    protected virtual void GenerateMappingForPrimitiveFields(DomainTypeMetadata domainTypeMetadata, XElement classTag, string externalTableName)
    {
        var generatedPrimitiveFields = domainTypeMetadata.PrimitiveFields;

        if (domainTypeMetadata.IsInlineBaseType)
        {
            generatedPrimitiveFields = domainTypeMetadata.GetAllElements(z => z.Parent).SelectMany(z => z.PrimitiveFields);
        }

        this.GenerateMappingForPrimitiveFields(
                                               generatedPrimitiveFields.OrderBy(x => x.Name).Where(v => v.ExternalTableName == externalTableName),
                                               classTag);
    }

    protected virtual void GenerateMappingForPrimitiveFields(IEnumerable<PrimitiveTypeFieldMetadata> primitiveFields, XElement xmlTag)
    {
        primitiveFields
                .Where(z => !z.IsVersion)
                .Where(z => !z.Name.ToLowerInvariant().Equals("id"))
                .Foreach(z => this.GeneratePrimitivePropertyMapping(z, xmlTag));
    }

    protected virtual void GenerateMappingForVersionField(DomainTypeMetadata domainTypeMetadata, XElement root, string externalTableName)
    {
        var primitiveFields = domainTypeMetadata.PrimitiveFields.Where(v => v.ExternalTableName == externalTableName);

        var versionField = primitiveFields.SingleOrDefault(
                                                           z => z.IsVersion,
                                                           _ => new Exception($"Type:{domainTypeMetadata.DomainType.Name} must have one property with version attribute"));

        if (null == versionField)
        {
            return;
        }

        var possibleValues = new[]
                             {
                                     new
                                     {
                                             SQLType = DataType.Timestamp,
                                             CLRTypeName = "BinaryBlob",
                                             CLRType = typeof(byte[]),
                                             GeneratedValue = "always"
                                     },
                                     new
                                     {
                                             SQLType = DataType.Int,
                                             CLRTypeName = "int",
                                             CLRType = typeof(int),
                                             GeneratedValue = "never"
                                     },
                                     new
                                     {
                                             SQLType = DataType.BigInt,
                                             CLRTypeName = typeof(long).Name,
                                             CLRType = typeof(long),
                                             GeneratedValue = "never"
                                     },
                                     new
                                     {
                                             SQLType = DataType.DateTime,
                                             CLRTypeName = typeof(DateTime).Name,
                                             CLRType = typeof(DateTime),
                                             GeneratedValue = "always"
                                     }
                             };

        var configuration = possibleValues.Single(z => z.CLRType == versionField.Type, () => new Exception(
                                                                                        $"Unsupported versioned field type. Supported types are:{possibleValues.Select(z => z.CLRType.FullName).Join(",")} "));

        var versionElement = root.CreateVersionElement()
                                 .WithLowNameAttribute(versionField.Name.ToPropertyName())
                                 .WithAttribute("generated", configuration.GeneratedValue)
                                 .WithAttribute("type", configuration.CLRTypeName);

        versionElement
                .CreateColumnElement()
                .WithLowNameAttribute(versionField.Name.ToPropertyName())
                .WithAttribute("not-null", "false")
                .WithAttribute("sql-type", configuration.SQLType.Name);
    }

    protected virtual void CreateIdMapping(DomainTypeMetadata domainTypeMetadata, FieldMetadata field, XElement classTag)
    {
        var mappingAttribute = field.Attributes.OfType<MappingAttribute>().FirstOrDefault();
        var identityFieldInDB = mappingAttribute.Maybe(z => z.ColumnName, this.GetColumnNameBy(domainTypeMetadata, domainTypeMetadata.GetIdentityField()));
        var typeName = field.Type.Name;
        var generatorAttribute = string.Empty;
        if (field.Type == typeof(Guid))
        {
            generatorAttribute = "guid.comb";
        }

        if (field.Type == typeof(int) || field.Type == typeof(long))
        {
            generatorAttribute = "increment";
        }

        if (string.IsNullOrWhiteSpace(generatorAttribute))
        {
            throw new ArgumentException(
                                        $"Identity field:{field.Name} for class:{domainTypeMetadata.DomainType.Name} has type{field.Type.Name} which unknown for generate mapping");
        }

        classTag.CreateElementWithRootNamespaceHandled("id")
                .WithLowNameAttribute("Id")
                .WithColumnAttribute(identityFieldInDB)
                .WithAttribute("type", typeName)
                .WithPrivateAccessAttribute(field.Name)
                .CreateElementWithRootNamespaceHandled("generator")
                .WithAttribute(ClassName, generatorAttribute);
    }

    private XElement CreateClassElement(DomainTypeMetadata domainTypeMetadata, XElement root)
    {
        var classElement = root.CreateElementWithRootNamespaceHandled(ClassName)
                               .WithLowNameAttribute(domainTypeMetadata.DomainType.FullName)
                               .WithTableElement(domainTypeMetadata)
                               .WithSchemaAttribute(domainTypeMetadata);

        if (domainTypeMetadata.IsView)
        {
            classElement.WithImmutableAttribute();
            classElement.WithSchemeActionNoneAttribute();
        }

        if (this.useSmartUpdate || domainTypeMetadata.UseSmartUpdate)
        {
            classElement.WithDynamicUpdateElement(domainTypeMetadata);
        }

        this.TryInitOptimisticLockAttributes(domainTypeMetadata, classElement);

        return classElement;
    }

    protected virtual void TryInitOptimisticLockAttributes(DomainTypeMetadata domainTypeMetadata, XElement element)
    {
        if (!domainTypeMetadata.DomainType.HasAttribute<OptimisticLockAttribute>())
        {
            return;
        }

        var optimisticLockAttribute = domainTypeMetadata.DomainType.GetCustomAttribute<OptimisticLockAttribute>();
        if (domainTypeMetadata.Fields.Any(z => z.IsVersion))
        {
            throw new Exception(
                                $"Unsupported {typeof(OptimisticLockAttribute).Name} with versioned property. Type:{domainTypeMetadata.DomainType.Name}");
        }

        element
                .WithAttribute("optimistic-lock", optimisticLockAttribute.LockType.ToString().ToLowerInvariant())
                .WithAttribute("dynamic-update", "true");
    }

    protected virtual void TryInitDynamicUpdateForOptimisticDeclareType(DomainTypeMetadata domainTypeMetadata, XElement element)
    {
        if (domainTypeMetadata.DomainType.HasAttribute<OptimisticLockAttribute>())
        {
            element.WithAttribute("dynamic-update", "true");
        }
    }

    private XElement CreateRoot()
    {
        var result = new XElement(RootNameNamespace + MappingGenerator.NHibernateMappingRootName)
                     .WithAttribute(AssemblyName, this.Assembly.Name)
                     .WithAttribute("auto-import", false)
                     .MaybeWithAttribute(SchemaName, this.schema);

        return result;
    }

    protected virtual void GeneratePrimitivePropertyMapping(FieldMetadata fieldMetadata, XElement rootElement)
    {
        rootElement.CreatePropertyElementWithRootNamespace()
                   .WithLowNameAttribute(fieldMetadata.Name.ToPropertyName())
                   .WithColumnAttribute(fieldMetadata.ToColumnName())
                   .WithTryUniqueAttribute(fieldMetadata)
                   .Pipe(el =>
                         {
                             this.ProcessPrimitiveType(el, fieldMetadata.Type, fieldMetadata.GetReferencedProperty().GetCustomAttributes().ToList());

                             this.TryProcessMappingPropertyAttribute(fieldMetadata, el);

                             return el;
                         })

                   .WithPrivateAccessAttribute(fieldMetadata.Name);
    }

    private void ProcessPrimitiveType(XElement el, Type fieldType, IList<Attribute> attributes)
    {
        if (fieldType == typeof(DateTime) || fieldType == typeof(DateTime?))
        {
            el.WithColumnType("timestamp");
        }

        var maxLengthAttribute = attributes.OfType<MaxLengthAttribute>().FirstOrDefault();

        if (null == maxLengthAttribute)
        {
            if (fieldType == typeof(byte[]))
            {
                el.WithLengthAttribute(int.MaxValue);
            }
        }
        else
        {
            el.WithLengthAttribute(maxLengthAttribute.Value);
        }

        if (fieldType == typeof(decimal) || fieldType == typeof(decimal?))
        {
            var attribute = attributes.OfType<LengthAndPrecisionAttribute>().Concat(new[] { LengthAndPrecisionAttribute.Default }).First();

            el.WithAttribute("precision", attribute.Length).WithAttribute("scale", attribute.Precision);
        }
    }

    protected virtual void TryProcessMappingPropertyAttribute(FieldMetadata fieldMetadata, XElement el)
    {
        var mappingPropertyAttribute = fieldMetadata.Attributes.OfType<MappingPropertyAttribute>().FirstOrDefault();
        if (null != mappingPropertyAttribute)
        {
            if (!mappingPropertyAttribute.CanInsert)
            {
                el.WithAttribute("insert", false);
            }

            if (!mappingPropertyAttribute.CanUpdate)
            {
                el.WithAttribute("update", false);
            }
        }
    }

    protected virtual void GenerateManyToOnePropertyMapping(ReferenceTypeFieldMetadata reference, XElement rootElement)
    {
        var sqlReferenceColumnName = reference.GetSqlReferenceColumnName();

        var propertyName = reference.Name.ToPropertyName();

        var refTag = rootElement.CreateManyToOnePropertyElementWithRootNamespace()
                                .WithLowNameAttribute(propertyName)
                                .WithColumnAttribute(sqlReferenceColumnName)
                                .WithClassAttribute(reference.ToType.FullName)
                                .WithTryUniqueAttribute(reference)
                                .WithPrivateAccessAttribute(reference.Name);

        this.TryApplyCascadeMode(reference, refTag);

        this.TryProcessMappingPropertyAttribute(reference, refTag);
    }

    protected virtual void TryApplyCascadeMode(ReferenceTypeFieldMetadata reference, XElement rootElement)
    {
        if (reference.CascadeMode == CascadeMode.Enabled
            || (reference.CascadeMode == CascadeMode.Auto && reference.DomainTypeMetadata.DomainType != reference.FromType))
        {
            rootElement.WithAllDeleteOrphanCascadeAttribute();
        }
    }

    protected virtual void GenerateInlineFieldMapping(InlineTypeFieldMetadata reference, XElement classTag)
    {
        var tryUniqueKey = reference.GetUniqueKeys();
        var componentElement = classTag.CreateElementWithRootNamespaceHandled("component")
                                       .WithLowNameAttribute(reference.Name.ToPropertyName())
                                       .WithClassAttribute(reference.Type.FullName + ", " + reference.Type.Assembly.GetName().Name)
                                       .WithPrivateAccessAttribute(reference.Name);

        var compositeAttributes = reference.Attributes.OfType<CompositeFieldAttribute>().ToList();

        var mappingAttr = reference.Attributes.OfType<MappingAttribute>().SingleOrDefault();

        var primitiveNamePrefix = mappingAttr == null ? reference.GetAllElements(z => z.Parent).Reverse().Concat(z => z.Name)
                                          : mappingAttr.ColumnName;

        Func<SqlFieldMappingInfo, string> getColumnNameFunc = sqlFieldMappingInfo =>
                                                              {
                                                                  var defaultColumnName = primitiveNamePrefix + sqlFieldMappingInfo.Name;

                                                                  var referenceName = reference.Name;

                                                                  var compositeAttr = compositeAttributes.FirstOrDefault(q =>
                                                                          string.Equals(referenceName + q.ClassFieldName, defaultColumnName, StringComparison.InvariantCultureIgnoreCase));
                                                                  return compositeAttr.Maybe(z => z.ColumnName).IfDefault(defaultColumnName);
                                                              };

        foreach (var pair in reference.PrimitiveMetadataCollection.OrderBy(v => v.Name).Select(z => new { Mapping = MapperFactory.GetMapping(z).Single(), FieldMetadata = z }))
        {
            var sqlMapping = pair.Mapping;
            var element = componentElement.CreatePropertyElementWithRootNamespace()
                                          .WithLowNameAttribute(sqlMapping.Source.Name.ToPropertyName())
                                          .WithColumnAttribute(getColumnNameFunc(sqlMapping))
                                          .WithPrivateAccessAttribute(reference.Name);

            if (!string.IsNullOrEmpty(tryUniqueKey))
            {
                element.WithUniqueKey(tryUniqueKey);
            }

            this.ProcessPrimitiveType(element, pair.FieldMetadata.Type, sqlMapping.Source.Attributes.ToList());
        }

        foreach (var pair in reference.ReferenceTypes.OrderBy(v => v.Name).Select(z => new { Mapping = MapperFactory.GetMapping(z).Single(), FieldMetadata = z }))
        {
            this.GenerateManyToOnePropertyMapping(pair.FieldMetadata, classTag);
        }

        foreach (var subInlineFieldMapping in reference.Children.OrderBy(v => v.Name))
        {
            this.GenerateInlineFieldMapping(subInlineFieldMapping, componentElement);
        }
    }

    protected virtual void GenerateOneToManyPropertyMapping(ListTypeFieldMetadata listTypeFieldMetadata, XElement root)
    {
        var oneToManyElement =
                root.CreateSetElementWithRootNamespace()
                    .WithLowNameAttribute(listTypeFieldMetadata.IsCompilerGenerated
                                                  ? listTypeFieldMetadata.Name
                                                  : listTypeFieldMetadata.Name.ToPropertyName())
                    .WithInverseAttribute(true);

        if (!listTypeFieldMetadata.IsCompilerGenerated)
        {
            oneToManyElement.WithPrivateAccessAttribute(listTypeFieldMetadata.Name);
        }

        if (listTypeFieldMetadata.Immutable)
        {
            oneToManyElement.WithCascadeAttribute("None");
        }
        else
        {
            if (listTypeFieldMetadata.CascadeMode == CascadeMode.Enabled
                || (listTypeFieldMetadata.CascadeMode == CascadeMode.Auto
                    && listTypeFieldMetadata.ElementType != listTypeFieldMetadata.DomainTypeMetadata.DomainType))
            {
                oneToManyElement.WithAllDeleteOrphanCascadeAttribute();
            }
            else
            {
                oneToManyElement.WithCascadeAttribute("None");
            }
        }

        var elementDomainTypeMetadata = listTypeFieldMetadata
                                        .DomainTypeMetadata
                                        .AssemblyMetadata
                                        .DomainTypes
                                        .SelectMany(z => z.NotAbstractChildrenDomainTypes.Concat(new[] { z }))
                                        .Single(z => z.DomainType == listTypeFieldMetadata.ElementType);

        var reverseReferenceArray =
                elementDomainTypeMetadata.ReferenceFields.Where(
                                                                z => z.ToType.IsAssignableFrom(listTypeFieldMetadata.DomainTypeMetadata.DomainType)).ToArray();

        var elementType = listTypeFieldMetadata.ElementType;

        while (0 == reverseReferenceArray.Length && elementType != typeof(object))
        {
            elementType = elementType.BaseType;
            var domainTypeMetadatas = listTypeFieldMetadata.DomainTypeMetadata.AssemblyMetadata.DomainTypes.SelectMany(z => z.NotAbstractChildrenDomainTypes.Concat(new[] { z })).ToList();

            var baseElementDomainTypeMetadata = domainTypeMetadatas
                                                .Where(z => !z.DomainType.IsAbstract)
                                                .SingleOrDefault(z => z.DomainType == elementType);

            if (baseElementDomainTypeMetadata == null)
            {
                continue;
            }

            reverseReferenceArray =
                    baseElementDomainTypeMetadata.ReferenceFields.Where(
                                                                        z => z.ToType.IsAssignableFrom(listTypeFieldMetadata.DomainTypeMetadata.DomainType)).ToArray();
        }

        var reverseReference = reverseReferenceArray.Length > 1
                                       ? reverseReferenceArray.Single(r => r.IsMasterReference)
                                       : reverseReferenceArray.Single();

        oneToManyElement
                .CreateKeyElement()
                .WithColumnAttribute(reverseReference.GetSqlReferenceColumnName());

        oneToManyElement
                .CreateOneToManyElementWithRootNamespace()
                .WithClassAttribute(listTypeFieldMetadata.ElementType.FullName);
    }
}
