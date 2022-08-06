using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using Framework.DomainDriven.DAL.Revisions;

using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Envers;
using NHibernate.Envers.Configuration.Attributes;
using NHibernate.Envers.Configuration.Fluent;
using NHibernate.Envers.Configuration.Store;
using NHibernate.Mapping.ByCode;

namespace Framework.DomainDriven.NHibernate.Audit
{
    /// <summary>
    /// Empty implement
    /// </summary>
    internal class EmptyAuditMetadataProvider : IMetaDataProvider
    {
        private readonly IRevisionListener _entityTrackingRevisionListener;


        public EmptyAuditMetadataProvider(IRevisionListener entityTrackingRevisionListener)
        {
            this._entityTrackingRevisionListener = entityTrackingRevisionListener;
        }


        public IDictionary<Type, IEntityMeta> CreateMetaData(Configuration nhConfiguration)
        {
            nhConfiguration.AddMapping(this.CreateRevisionInfoMappingDocument());

            var ret = new Dictionary<Type, IEntityMeta>();
            var auditRevisionType = typeof(AuditRevisionEntity);

            ret[auditRevisionType] = new EntityMeta();


            var entityMeta = ((EntityMeta)ret[auditRevisionType]);

            entityMeta.AddClassMeta(new RevisionEntityAttribute { Listener = this._entityTrackingRevisionListener });


            Expression<Func<AuditRevisionEntity, long>> revisionNumber = z => z.Id;
            Expression<Func<AuditRevisionEntity, DateTime>> revisionTimestamp = z => z.RevisionDate;


            entityMeta.AddMemberMeta(revisionNumber.MethodInfo(), new RevisionNumberAttribute());
            entityMeta.AddMemberMeta(revisionTimestamp.MethodInfo(), new RevisionTimestampAttribute());

            return ret;
        }


        private HbmMapping CreateRevisionInfoMappingDocument()
        {
            var mapper = new ModelMapper();

            mapper.Class<AuditRevisionEntity>(mapping =>
            {
                mapping.Id(z => z.Id, idMapper => idMapper.Generator(Generators.Native, z => z.Params(new KeyValuePair<string, string>("sequence", "generateidsequence"))));

                mapping.Property(z => z.RevisionDate, z => { z.Insert(true); z.Update(false); });
                mapping.Property(z => z.Author, z => { z.Insert(true); z.Update(false); });
            });

            var result = mapper.CompileMappingForAllExplicitlyAddedEntities();

            return result;
        }
    }
}
