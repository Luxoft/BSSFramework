using System.Linq;
using System.Xml.Linq;
using Framework.Core;
using Framework.DomainDriven.Metadata;
using Framework.Persistent.Mapping;

namespace Framework.DomainDriven.NHibernate.DALGenerator;

internal static class XElementExtensions
{
    public static XElement WithPrivateAccessAttribute(this XElement source, string fieldName)
    {
        var defaultAccess = "field.camelcase";

        if (fieldName.StartsWith("_"))
        {
            defaultAccess += "-underscore";
        }

        return source.WithAttribute("access", defaultAccess);
    }

    public static XElement CreatePropertyElement(this XElement root)
    {
        return root.CreateElement("property");
    }

    private static string GetTableName(DomainTypeMetadata domainTypeMetadata)
    {
        var tableNameInfo = domainTypeMetadata.DomainType.GetTableName(string.Empty);

        return $"{tableNameInfo.tableName}";
    }

    public static XElement WithTableElement(this XElement root, DomainTypeMetadata domainTypeMetadata)
    {
        root.WithAttribute("table", GetTableName(domainTypeMetadata));
        return root;
    }

    public static XElement WithSchemaAttribute(this XElement root, DomainTypeMetadata domainTypeMetadata)
    {
        var name = domainTypeMetadata.DomainType.GetTableName(string.Empty);
        if (string.IsNullOrEmpty(name.schemaName))
        {
            return root;
        }
        root.WithAttribute("schema", $"{name.schemaName}");
        return root;
    }

    public static XElement WithDynamicUpdateElement(this XElement root, DomainTypeMetadata domainTypeMetadata)
    {
        root.WithAttribute("dynamic-update", "true");
        return root;
    }

    public static XElement CreatePropertyElementWithRootNamespace(this XElement root)
    {
        return root.CreateElementWithRootNamespaceHandled("property");
    }

    public static XElement CreateManyToOnePropertyElement(this XElement root)
    {
        return root.CreateElement("many-to-one");
    }

    public static XElement CreateManyToOnePropertyElementWithRootNamespace(this XElement root)
    {
        return root.CreateElement("many-to-one");
    }

    public static XElement CreateKeyElement(this XElement root)
    {
        return root.CreateElement("key");
    }

    public static XElement WithClassAttribute(this XElement source, string className)
    {
        return source.WithAttribute("class", className);
    }

    public static XElement WithImmutableAttribute(this XElement source)
    {
        source.WithAttribute("mutable", false);
        return source;
    }

    public static XElement WithSchemeActionNoneAttribute(this XElement source)
    {
        source.WithAttribute("schema-action", "none");
        return source;
    }

    public static XElement WithPropertyRefAttribute(this XElement source, string propertyName)
    {
        return source.WithAttribute("property-ref", propertyName);
    }

    public static XElement WithColumnAttribute(this XElement source, string columnName)
    {
        return source.WithAttribute("column", columnName);
    }

    public static XElement WithTryUniqueAttribute(this XElement source, FieldMetadata fieldMetadata)
    {
        var uniqueKeys = fieldMetadata.GetUniqueKeys();

        if (!string.IsNullOrWhiteSpace(uniqueKeys))
        {
            source.WithUniqueKey(uniqueKeys);
        }

        return source;
    }

    public static void WithUniqueKey(this XElement source, string uniqueKeys)
    {
        source.WithAttribute("unique-key", uniqueKeys);
    }

    public static string GetUniqueKeys(this FieldMetadata fieldMetadata)
    {
        var uniqueKeys =
                fieldMetadata.DomainTypeMetadata.UniqueIndexes.Where(
                                                                     z =>
                                                                             z.Fields.Select(q => q.DomainTypeMetadata)
                                                                              .All(refDomainType => refDomainType == fieldMetadata.DomainTypeMetadata))
                             .Where(z => z.Fields.Contains(fieldMetadata))
                             .Select(z => z.FriendlyName)
                             .Distinct()
                             .Join(",");
        return uniqueKeys;
    }

    public static XElement WithColumnType(this XElement source, string columnType)
    {
        return source.WithAttribute("type", columnType);
    }

    public static XElement CreateOneToManyElement(this XElement root)
    {
        return root.CreateElement("one-to-many");
    }

    public static XElement CreateOneToManyElementWithRootNamespace(this XElement root)
    {
        return root.CreateElementWithRootNamespaceHandled("one-to-many");
    }

    public static XElement CreateOneToOneElementWithRootNamespace(this XElement root)
    {
        return root.CreateElementWithRootNamespaceHandled("one-to-one");
    }

    public static XElement CreateBagElement(this XElement root)
    {
        return root.CreateElement("bag");
    }

    public static XElement CreateVersionElement(this XElement root)
    {
        return root.CreateElement("version");
    }

    public static XElement CreateColumnElement(this XElement root)
    {
        return root.CreateElement("column");
    }

    public static XElement CreateBagElementWithRootNamespace(this XElement root)
    {
        return root.CreateElementWithRootNamespaceHandled("bag");
    }

    public static XElement CreateSetElementWithRootNamespace(this XElement root)
    {
        return root.CreateElementWithRootNamespaceHandled("set");
    }

    public static XElement WithAllDeleteOrphanCascadeAttribute(this XElement source)
    {
        return source.WithCascadeAttribute("all-delete-orphan");
    }

    public static XElement WithCascadeAttribute(this XElement source, string cascadeValue)
    {
        return source.WithAttribute("cascade", cascadeValue);
    }

    public static XElement WithInverseAttribute(this XElement source, bool inverse)
    {
        return source.WithAttribute("inverse", inverse);
    }

    internal static XElement CreateElementWithRootNamespaceHandled(this XElement source, string name)
    {
        return source.CreateElement(name);
    }
}
