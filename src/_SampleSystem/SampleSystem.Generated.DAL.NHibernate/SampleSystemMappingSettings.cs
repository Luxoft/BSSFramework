using System;
using System.Collections.Generic;
using System.Xml.Linq;

using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

using Framework.DomainDriven;
using Framework.DomainDriven.NHibernate;

using NHibernate.Cfg;
using NHibernate.Driver;

using SampleSystem.Domain;

using Environment = NHibernate.Cfg.Environment;

namespace SampleSystem.Generated.DAL.NHibernate
{
    public class SampleSystemMappingSettings : MappingSettings<PersistentDomainObjectBase>
    {
        private readonly string connectionString;

        public SampleSystemMappingSettings(DatabaseName databaseName, string connectionString)
                : base(typeof(SampleSystemMappingSettings).Assembly, databaseName, databaseName.ToDefaultAudit()) =>
                this.connectionString = connectionString;

        /// <summary>
        /// For DBGenerator
        /// </summary>
        public SampleSystemMappingSettings(
                IEnumerable<XDocument> mappingXmls,
                DatabaseName databaseName,
                AuditDatabaseName auditDatabaseName,
                string connectionString,
                IEnumerable<Type> types = null)
                : base(mappingXmls, databaseName, auditDatabaseName, types)
        {
            this.connectionString = connectionString;
        }

        public override void InitMapping(Configuration cfg)
        {
            base.InitMapping(cfg);

            Fluently
                    .Configure(cfg)
                    .Database(
                              MsSqlConfiguration.MsSql2012
                                                .Dialect<EnhancedMsSql2012Dialect>()
                                                .Driver<Fix2100SqlClientDriver>()
                                                .ConnectionString(this.connectionString))
                    .Mappings(
                              m =>
                              {
                                  m.FluentMappings.AddFromAssemblyOf<SampleSystemMappingSettings>()
                                   .Conventions.AddFromAssemblyOf<EnumConvention>();
                              })
                    .ExposeConfiguration(
                                         c =>
                                         {
                                             c.Properties.Add(Environment.LinqToHqlGeneratorsRegistry, typeof(EnhancedLinqToHqlGeneratorsRegistry).AssemblyQualifiedName);

                                             c.Properties.Add(Environment.SqlExceptionConverter, typeof(SQLExceptionConverter).AssemblyQualifiedName);

                                             c.Properties.Add(Environment.CommandTimeout, "1200");

                                             c.Properties.Add(Environment.SqlTypesKeepDateTime, "true");
                                         })
                    .BuildConfiguration();
        }

        private class EnumConvention : IUserTypeConvention
        {
            public void Accept(IAcceptanceCriteria<IPropertyInspector> criteria) =>
                    criteria.Expect(
                                    x => x.Property.PropertyType.IsEnum
                                         || (x.Property.PropertyType.IsGenericType
                                             && x.Property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)
                                             && x.Property.PropertyType.GetGenericArguments()[0].IsEnum));

            public void Apply(IPropertyInstance instance) => instance.CustomType(instance.Property.PropertyType);
        }
    }
}
