using System.Reflection;
using System.Runtime.CompilerServices;

using CommonFramework;

using Framework.Core;

namespace Framework.DomainDriven.Metadata;

public class InlineTypeFieldMetadata : FieldMetadata
{
    private readonly InlineTypeFieldMetadata _parent;

    private readonly IList<PrimitiveTypeFieldMetadata> _primitiveMetadataCollection;
    private readonly List<InlineTypeFieldMetadata> _children;

    private readonly List<ReferenceTypeFieldMetadata> referenceTypes;

    public InlineTypeFieldMetadata(string name, Type type, IEnumerable<Attribute> attributes, DomainTypeMetadata domainTypeMetadata, InlineTypeFieldMetadata parent = null)
            : base(name, type, attributes, domainTypeMetadata)
    {
        this._parent = parent;


        var currentType = type;
        if (currentType.IsNullable())
        {
            currentType = currentType.GetNullableElementType();
        }

        var fields = currentType.ExpandFields();



        var partialResult = fields.Partial(z => z.FieldType.IsPrimitiveType(),
                                           (primitive, composite) => new { primitive, composite });
        var refTypeAndComposite = partialResult
                                  .composite
                                  .Partial(
                                           z => z.FieldType.IsDomainType(
                                                                         domainTypeMetadata.AssemblyMetadata
                                                                                 .PersistentDomainObjectBaseType),
                                           (refType, composite) => new { refType, composite });

        this._primitiveMetadataCollection = partialResult.primitive
                                                         .Select(z => new PrimitiveTypeFieldMetadata(z.Name, z.FieldType, z.GetAttributes(type), domainTypeMetadata, z.Name.ToLower() == "id"))
                                                         .ToList();

        this.referenceTypes = refTypeAndComposite
                              .refType
                              .Select(
                                      z => new ReferenceTypeFieldMetadata(
                                                                          z.Name,
                                                                          z.FieldType,
                                                                          z.GetAttributes(type),
                                                                          domainTypeMetadata))
                              .ToList();

        this._children =
                refTypeAndComposite
                        .composite
                        .Select(
                                z =>
                                        new InlineTypeFieldMetadata(z.Name, z.FieldType, z.GetAttributes(z.FieldType),
                                                                    domainTypeMetadata, this)).ToList();
    }

    public IEnumerable<PrimitiveTypeFieldMetadata> GetPrimitiveTypeFieldMetadata (Type declarationType)
    {
        var parentPrefix = this.Parent.Maybe(z => z.Name, string.Empty);
        return this.Type.GetInstanseFieldsDeep()
                   .Select(z => new PrimitiveTypeFieldMetadata(parentPrefix+z.Name, z.FieldType, z.GetAttributes(declarationType)
                                                                       .Concat(z.GetAttributes(z.DeclaringType)), this.DomainTypeMetadata, z.Name.ToLower() == "id"));
    }

    public bool IsComposite
    {
        get { return this._children.Any(); }
    }

    public InlineTypeFieldMetadata Parent
    {
        get { return this._parent; }
    }

    public IList<PrimitiveTypeFieldMetadata> PrimitiveMetadataCollection
    {
        get { return this._primitiveMetadataCollection; }
    }

    public List<InlineTypeFieldMetadata> Children
    {
        get { return this._children; }
    }

    public List<ReferenceTypeFieldMetadata> ReferenceTypes => this.referenceTypes;
}

public static class FieldInfoExtension
{
    public static IEnumerable<Attribute> GetCollectionAttributes (this FieldInfo field, Type declarationType)
    {
        var fieldType = field.FieldType;

        var property = declarationType.GetProperties(BindingFlags.Public | BindingFlags.Instance).FirstOrDefault(
         property_ => string.Equals (property_.Name, field.Name, StringComparison.CurrentCultureIgnoreCase));

        if (null == property)
        {
            property = declarationType
                       .GetProperties()
                       .Where(z => (z.GetGetMethod(true) ?? z.GetSetMethod(true))!.IsDefined(typeof(CompilerGeneratedAttribute), false))
                       .Select(z => new { GeneratedName = $"<{z.Name}>k__BackingField", Property = z })
                       .FirstOrDefault(z => string.Equals(z.GeneratedName, field.Name, StringComparison.InvariantCultureIgnoreCase))
                       .Maybe(z=>z.Property);
        }

        if (property != null && fieldType.IsGenericType)
        {
            var propertyGeneric = property.PropertyType.GetGenericTypeDefinition();

            var fieldElement = fieldType.GetGenericArguments().Single();
            var propertyElement = property.PropertyType.GetGenericArguments().Single();

            if (fieldElement.IsAssignableFrom(propertyElement) && typeof(IEnumerable<object>).IsAssignableFrom(propertyGeneric.MakeGenericType(typeof(object))))
            {
                return property.GetCustomAttributes<Attribute>();
            }
        }

        return Enumerable.Empty<Attribute> ();
    }

    public static IEnumerable<Attribute> GetAttributes(this FieldInfo field, Type declarationType)
    {

        var property = declarationType.GetPropertyInfoBy (field);

        return property
               .Maybe (z => z.GetCustomAttributes<Attribute>()).EmptyIfNull ();
    }

    private static PropertyInfo GetPropertyInfoBy(this Type type, FieldInfo fieldInfo)
    {
        var result = type.GetProperties().FirstOrDefault(
                                                         z => fieldInfo.FieldType == z.PropertyType
                                                              && string.Equals (z.Name, fieldInfo.Name, StringComparison.CurrentCultureIgnoreCase));

        if (null == result)
        {
            if (type.BaseType == typeof(object))
            {
                return null;
            }
            return type.BaseType.GetPropertyInfoBy (fieldInfo);
        }
        return result;
    }
}
