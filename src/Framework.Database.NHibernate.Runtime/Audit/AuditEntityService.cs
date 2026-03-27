using NHibernate.Envers.Configuration.Attributes;
using NHibernate.Mapping;

namespace Framework.Database.NHibernate.Audit;

public class AuditEntityService : IAuditAttributeService
{
    private readonly Dictionary<Type, bool> isAuditedDict = new();

    private readonly Dictionary<Type, Dictionary<Property, bool>> isAuditedPropertyDict = new();

    private readonly Dictionary<Type, string> typeToAuditTableSchemaDictionary = new();

    public void Register(Type type, bool isAudited)
    {
        this.isAuditedDict.Add(type, isAudited);
    }

    public void Register(Type type, string auditTableSchema)
    {
        this.typeToAuditTableSchemaDictionary.Add(type, auditTableSchema);
    }

    public void Register(Property property, bool isAudited)
    {
        var mappedClass = property.PersistentClass.MappedClass;

        Dictionary<Property, bool> propertyDict;

        if (!this.isAuditedPropertyDict.TryGetValue(mappedClass, out propertyDict))
        {
            propertyDict = new Dictionary<Property, bool>();
            this.isAuditedPropertyDict.Add(mappedClass, propertyDict);
        }

        propertyDict.Add(property, isAudited);
    }

    public RelationTargetAuditMode GetAttributeFor(Type type)
    {
        bool result;
        if (!this.isAuditedDict.TryGetValue(type, out result))
        {
            result = true;
        }

        return result ? RelationTargetAuditMode.Audited : RelationTargetAuditMode.NotAudited;
    }

    public RelationTargetAuditMode GetAttributeFor(Type type, Property property)
    {
        Dictionary<Property, bool> propertyDict;
        var result = true;

        if (this.isAuditedPropertyDict.TryGetValue(type, out propertyDict))
        {
            if (!propertyDict.TryGetValue(property, out result))
            {
                result = true; // ident property
            }
        }

        return result ? RelationTargetAuditMode.Audited : RelationTargetAuditMode.NotAudited;
    }

    public string GetAuditTableSchemaOrDefault(Type type)
    {
        string result;
        this.typeToAuditTableSchemaDictionary.TryGetValue(type, out result);
        return result;
    }
}
