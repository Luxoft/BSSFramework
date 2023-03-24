using System.Collections.Generic;
using System.Linq;

using Framework.Core;
using Framework.DomainDriven.Metadata;

namespace Framework.DomainDriven.DAL.Sql;

public class InlineTypeFieldMapper : IMapper
{
    public IEnumerable<SqlFieldMappingInfo> GetMapping(FieldMetadata field) => GetMappingWithPrimitives(field).Concat(GetMappingWithRefType(field));

    private static IEnumerable<SqlFieldMappingInfo> GetMappingWithPrimitives(FieldMetadata field)
    {
        var inlineTypeFieldMetadata = field as InlineTypeFieldMetadata;

        var pairs = inlineTypeFieldMetadata
                    .GetAllElements(z => z.Children)
                    .Select(
                            z => new
                                 {
                                         Name = string.Join(
                                                            string.Empty,
                                                            z.GetAllElements(q => q.Parent)
                                                             .Select(q => q.Name)
                                                             .Reverse()),
                                         PrimitiveCollection = z.PrimitiveMetadataCollection,
                                 })
                    .ToList();

        var mapper = new PrimitiveTypeFieldMapper();

        return pairs.SelectMany(z => z.PrimitiveCollection.SelectMany(q => mapper.GetMapping(q, z.Name + q.Name)));
    }

    private static IEnumerable<SqlFieldMappingInfo> GetMappingWithRefType(FieldMetadata field)
    {
        var inlineTypeFieldMetadata = field as InlineTypeFieldMetadata;

        var pairs = inlineTypeFieldMetadata
                    .GetAllElements(z => z.Children)
                    .Select(
                            z => new
                                 {
                                         Name = string.Join(
                                                            string.Empty,
                                                            z.GetAllElements(q => q.Parent)
                                                             .Select(q => q.Name)
                                                             .Reverse()),
                                         z.ReferenceTypes,
                                 })
                    .ToList();

        var mapper = new ReferenceTypeFieldMapper();

        return pairs.SelectMany(z => z.ReferenceTypes.SelectMany(q => mapper.GetMapping(q, z.Name)));
    }
}
