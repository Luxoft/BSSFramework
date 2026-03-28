using System.IO.Compression;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Framework.Infrastructure.Integration;

[Obsolete("Use IEventXsdExporter2")]
public class EventXsdExporter(string xsdNamespace, IEnumerable<Type> types)
{
    public Stream Export()
    {
        var schemas = this.ExportTypes();

        return this.WriteXsd(schemas);
    }

    private XmlSchemaSet ExportTypes()
    {
        var schemas = new XmlSchemas();

        var exporter = new XmlSchemaExporter(schemas);

        var importer = new XmlReflectionImporter(xsdNamespace);

        foreach (var type in types)
        {
            var mapping = importer.ImportTypeMapping(type, xsdNamespace);

            exporter.ExportTypeMapping(mapping);
        }

        schemas.Compile(null, false);

        return this.ToSet(schemas);
    }

    private XmlSchemaSet ToSet(XmlSchemas schemas)
    {
        var set = new XmlSchemaSet();

        foreach (XmlSchema schema in schemas)
        {
            set.Add(schema);
        }

        set.Compile();

        return set;
    }

    private string ToFileName(string targetNamespace, int index) =>
            $"{targetNamespace.Replace("http://", "").Replace("/", "_").Replace(".", "_")}_{index}.xsd";

    private Stream WriteXsd(XmlSchemaSet schemasSet)
    {
        var zipStream = new MemoryStream();

        using (var zip = new ZipArchive(zipStream, ZipArchiveMode.Create, true))
        {
            var index = 0;

            foreach (XmlSchema schema in schemasSet.Schemas())
            {
                var entry = zip.CreateEntry(this.ToFileName(schema.TargetNamespace, index));

                using (var entryStream = entry.Open())
                {
                    schema.Write(entryStream);
                }

                index++;
            }
        }

        zipStream.Position = 0;

        return zipStream;
    }
}
