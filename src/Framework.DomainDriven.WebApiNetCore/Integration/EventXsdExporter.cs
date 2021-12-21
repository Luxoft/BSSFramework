using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Framework.DomainDriven.WebApiNetCore.Integration
{
    public class EventXsdExporter
    {
        private readonly string xsdNamespace;

        private readonly IEnumerable<Type> types;

        public EventXsdExporter(string xsdNamespace, IEnumerable<Type> types)
        {
            this.xsdNamespace = xsdNamespace;
            this.types = types;
        }

        public Stream Export()
        {
            var schemas = this.ExportTypes();

            return this.WriteXsd(schemas);
        }

        private XmlSchemaSet ExportTypes()
        {
            var schemas = new XmlSchemas();

            var exporter = new XmlSchemaExporter(schemas);

            var importer = new XmlReflectionImporter(this.xsdNamespace);

            foreach (var type in this.types)
            {
                var mapping = importer.ImportTypeMapping(type, this.xsdNamespace);

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
}
