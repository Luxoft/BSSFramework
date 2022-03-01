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

using WorkflowSampleSystem.Domain;

using Environment = NHibernate.Cfg.Environment;

namespace WorkflowSampleSystem.Generated.DAL.NHibernate
{
    public class WorkflowSampleSystemMappingSettings : MappingSettings<PersistentDomainObjectBase>
    {
        private readonly string connectionString;

        public WorkflowSampleSystemMappingSettings(DatabaseName databaseName, string connectionString)
                : base(typeof(WorkflowSampleSystemMappingSettings).Assembly, databaseName, databaseName.ToDefaultAudit()) =>
                this.connectionString = connectionString;

        /// <summary>
        /// For DBGenerator
        /// </summary>
        public WorkflowSampleSystemMappingSettings(
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
                                                .Driver<MicrosoftDataSqlClientDriver>()
                                                .ConnectionString(this.connectionString))
                    .Mappings(
                              m =>
                              {
                                  m.FluentMappings.AddFromAssemblyOf<WorkflowSampleSystemMappingSettings>()
                                   .Conventions.AddFromAssemblyOf<EnumConvention>();
                              })
                    .ExposeConfiguration(
                                         c =>
                                         {
                                             c.Properties.Add(
                                                              Environment.LinqToHqlGeneratorsRegistry,
                                                              typeof(EnhancedLinqToHqlGeneratorsRegistry).AssemblyQualifiedName);
                                             c.Properties.Add(
                                                              Environment.SqlExceptionConverter,
                                                              typeof(SQLExceptionConverter).AssemblyQualifiedName);
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
