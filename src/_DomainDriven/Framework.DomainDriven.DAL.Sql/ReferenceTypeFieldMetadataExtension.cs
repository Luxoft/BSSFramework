using System;
using System.Linq;
using Framework.Persistent.Mapping;
using Framework.Persistent.Mapping;
using Framework.DomainDriven.Metadata;
using Framework.Core;
using Framework.Persistent.Mapping;

namespace Framework.DomainDriven.DAL.Sql
{
    public static class FieldMetadataExtension
    {
        public static string ToColumnName<TFieldMetadata> (this TFieldMetadata source, Func<FieldMetadata, string> getDefaultColumnNameFunc)
            where TFieldMetadata : FieldMetadata
        {
            var mappingAttribute =
                source
                .Attributes
                .OfType<MappingAttribute> ()
                .SingleOrDefault (() => new Exception (
                                            $"Field {source.Name} of {source.DomainTypeMetadata.DomainType.Name} has no one {typeof(MappingAttribute).Name}"));

            return mappingAttribute.Maybe (z => z.ColumnName).IfDefaultString(getDefaultColumnNameFunc (source));
        }

        public static string ToColumnName<TFieldMetadata> (this TFieldMetadata source)
            where TFieldMetadata : FieldMetadata
        {
            return source.ToColumnName (z =>
                z.Name);
        }
    }
    public static class ReferenceTypeFieldMetadataExtension
    {
        public static string GetGroupIndexName(this ReferenceTypeFieldMetadata reference)
        {
            var sqlMapping = MapperFactory.GetMapping(reference).First();
            return
                $"IX_{reference.DomainTypeMetadata.DomainType.Name}_{sqlMapping.Name}_{reference.Type.Name}_{reference.DomainTypeMetadata.PrimitiveFields.First(z => z.IsIdentity).Name}";
        }

        public static string GetForeignKeyName(this ReferenceTypeFieldMetadata reference)
        {
            var sqlMapping = MapperFactory.GetMapping(reference).First();
            return $"FK_{reference.DomainTypeMetadata.DomainType.Name}_{sqlMapping.Name}_{reference.Type.Name}";
        }
        public static string GetForeignKeyName(this ListTypeFieldMetadata source)
        {
            var assemblyMetadata = source.DomainTypeMetadata.AssemblyMetadata;
            var allReferancies = assemblyMetadata.DomainTypes.SelectMany(z => z.ReferenceFields);
            var reference = allReferancies.Single(z => z.FromType == source.ElementType && z.ToType == source.DomainTypeMetadata.DomainType);
            return reference.GetForeignKeyName();
        }
        public static string GetSqlReferenceColumnName(this ReferenceTypeFieldMetadata source)
        {
            var fi = (FieldMetadata)source;
            return fi.ToColumnName (z => z.Name + "Id");
        }
        public static string ToSqlColumnName(this FieldMetadata source)
        {
            return  source.GetMappingName() ?? source.Name;
        }
        private static string GetMappingName(this FieldMetadata source)
        {
            return source.Attributes.OfType<MappingAttribute>().SingleOrDefault().Maybe(z => z.ColumnName);
        }
    }
}