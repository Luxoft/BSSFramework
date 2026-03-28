using Framework.Database.Mapping;
using Framework.Database.Metadata;

namespace Framework.Database.SqlMapper;

public class PrimitiveTypeFieldMapper : Mapper<PrimitiveTypeFieldMetadata>
{
    protected override IEnumerable<SqlFieldMappingInfo> GetMapping(PrimitiveTypeFieldMetadata field) => this.GetMapping(field, field.ToSqlColumnName() ?? field.Name);

    public IEnumerable<SqlFieldMappingInfo> GetMapping(PrimitiveTypeFieldMetadata field, string overrideColumnName)
    {
        var isPrimaryKey = field.IsIdentity;

        var fieldName = overrideColumnName;



        var isNullable = !(field.IsVersion && new[] { typeof(int), typeof(long) }.Contains(field.Type) || isPrimaryKey);

        string defaultConstraint = field.IsVersion && new[] { typeof(int), typeof(long) }.Contains(field.Type) ? "0" : null;

        yield return new SqlFieldMappingInfo(
            field.Type.ToDataType(field.Attributes),
            fieldName,
            isNullable,
            isPrimaryKey,
            field,
            defaultConstraint,
            field.Attributes.OfType<MappingAttribute>().Any(z => z.IsUnique));

    }
}
