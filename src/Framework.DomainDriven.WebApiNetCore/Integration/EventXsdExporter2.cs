using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

using Framework.Core;

namespace Framework.DomainDriven.WebApiNetCore.Integration;

public class EventXsdExporter2 : IEventXsdExporter2
{
    private const string FileNameWithoutNamespace = "emptyNamespace.xsd";

    public Stream Export(string xsdNamespace, string localName, IReadOnlyCollection<Type> types)
    {
        var exportTypes = this.GetSchemaSet(
                                            xsdNamespace,
                                            localName,
                                            types);

        return WriteXsd(exportTypes);
    }

    public Stream Export<TBaseEventDto>()
            where TBaseEventDto : class
    {
        var baseEventDto = typeof(TBaseEventDto);
        var attribute = baseEventDto.GetCustomAttribute<DataContractAttribute>();
        if (attribute == null)
        {
            throw new NotSupportedException($"{baseEventDto} should contain DataContractAttribute");
        }

        var exportTypes = this.GetSchemaSet(
                                            attribute.Namespace,
                                            baseEventDto.Name,
                                            Assembly.GetAssembly(baseEventDto)!.GetTypes()
                                                    .Where(x => x.IsClass && !x.IsAbstract && baseEventDto.IsAssignableFrom(x))
                                                    .ToList());

        return WriteXsd(exportTypes);
    }

    private XmlSchemaSet GetSchemaSet(string xsdNamespace, string localName, IReadOnlyCollection<Type> types)
    {
        var schemas = new XmlSchemas();
        var exporter = new XmlSchemaExporter(schemas);
        var importer = new XmlReflectionImporter(this.GetOverrideAttributes(types), xsdNamespace);

        foreach (var mapping in types.Select(x => importer.ImportTypeMapping(x, xsdNamespace)))
        {
            exporter.ExportTypeMapping(mapping);
        }

        schemas.Compile(null, false);

        return ToSet(xsdNamespace, localName, schemas);
    }

    private static XmlSchemaSet ToSet(string xsdNamespace, string localName, XmlSchemas schemas)
    {
        var set = new XmlSchemaSet();

        var xmlSchema = schemas.First();
        OverrideDerivedClassProperties(xsdNamespace, localName, xmlSchema);

        AddReferences(xmlSchema, schemas.Except(new[] { xmlSchema }).ToList());

        foreach (XmlSchema schema in schemas)
        {
            set.Add(schema);
        }

        set.Compile();

        return set;
    }

    private static string ToFileName(string targetNamespace) =>
            targetNamespace == null
                    ? FileNameWithoutNamespace
                    : $"{targetNamespace.Replace("http://", string.Empty).Replace("/", "_").Replace(".", "_")}.xsd";

    private static Stream WriteXsd(XmlSchemaSet schemasSet)
    {
        var zipStream = new MemoryStream();
        using (var zip = new ZipArchive(zipStream, ZipArchiveMode.Create, true))
        {
            foreach (XmlSchema schema in schemasSet.Schemas())
            {
                var entry = zip.CreateEntry(ToFileName(schema.TargetNamespace));
                using var entryStream = entry.Open();
                schema.Write(entryStream);
            }
        }

        zipStream.Position = 0;

        return zipStream;
    }

    private static void AddReferences(XmlSchema schema, IEnumerable<XmlSchema> references)
    {
        schema.Includes.Clear();

        foreach (var schemaReference in references)
        {
            var xmlSchemaImport = new XmlSchemaImport
                                  {
                                          Namespace = schemaReference.TargetNamespace,
                                          SchemaLocation = $".\\{ToFileName(schemaReference.TargetNamespace)}"
                                  };

            schema.Includes.Add(xmlSchemaImport);
        }
    }

    protected virtual XmlAttributeOverrides GetOverrideAttributes(IEnumerable<Type> types)
    {
        var attributeOverrides = new XmlAttributeOverrides();
        ChangeNamespacePeriod(attributeOverrides);

        foreach (var type in types)
        {
            MakeNullableProps(attributeOverrides, type);
            MakeNullableFields(attributeOverrides, type);
        }

        return attributeOverrides;
    }

    private static void OverrideDerivedClassProperties(string xsdNamespace, string localName, XmlSchema xmlSchema)
    {
        var schemaType = xmlSchema.SchemaTypes[new XmlQualifiedName(localName, xsdNamespace)];
        if (schemaType is XmlSchemaComplexType type)
        {
            type.IsAbstract = false;
        }

        foreach (var schemaItem in xmlSchema.Items)
        {
            if (schemaItem is not XmlSchemaComplexType complexType)
            {
                continue;
            }

            if (complexType.Particle is not XmlSchemaSequence sequence)
            {
                continue;
            }

            foreach (var schemaElement in sequence.Items.Cast<XmlSchemaElement>())
            {
                schemaElement.MinOccurs = 0;
            }
        }
    }

    private static void ChangeNamespacePeriod(XmlAttributeOverrides attributeOverrides)
    {
        var @namespace = CustomAttributeProviderExtensions.GetCustomAttribute<DataContractAttribute>(typeof(Period))?.Namespace
                         ?? string.Empty;

        attributeOverrides.Add(
                               typeof(Period),
                               nameof(Period.StartDate),
                               new XmlAttributes
                               {
                                       XmlElements = { new XmlElementAttribute { Order = 2, Namespace = @namespace } }
                               });
        attributeOverrides.Add(
                               typeof(Period),
                               nameof(Period.EndDate),
                               new XmlAttributes
                               {
                                       XmlElements = { new XmlElementAttribute { Order = 1, Namespace = @namespace } }
                               });
    }

    private static void MakeNullableProps(XmlAttributeOverrides attributeOverrides, Type type)
    {
        foreach (var property in type.GetProperties())
        {
            if (attributeOverrides[type, property.Name] != null || IsNotNullable(property.PropertyType))
            {
                continue;
            }

            var attributes = new XmlAttributes();
            attributes.XmlElements.Add(new XmlElementAttribute { IsNullable = true });

            attributeOverrides.Add(type, property.Name, attributes);

            MakeNullableProps(attributeOverrides, property.PropertyType);
        }
    }

    private static void MakeNullableFields(XmlAttributeOverrides attributeOverrides, Type type)
    {
        foreach (var field in type.GetFields())
        {
            if (attributeOverrides[type, field.Name] != null || IsNotNullable(field.FieldType))
            {
                continue;
            }

            var attributes = new XmlAttributes();
            attributes.XmlElements.Add(new XmlElementAttribute { IsNullable = true });
            attributeOverrides.Add(type, field.Name, attributes);

            MakeNullableFields(attributeOverrides, field.FieldType);
        }
    }

    private static bool IsNotNullable(Type type) =>
            type.IsValueType || (type.IsGenericType && type.GetGenericTypeDefinition() != typeof(Nullable<>));
}
