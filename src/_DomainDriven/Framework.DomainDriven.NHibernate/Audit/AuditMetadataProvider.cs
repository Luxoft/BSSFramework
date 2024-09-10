using System.Linq.Expressions;

using Framework.DomainDriven.DAL.Revisions;
using Framework.Persistent.Mapping;

using NHibernate;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Envers;
using NHibernate.Envers.Configuration.Attributes;
using NHibernate.Envers.Configuration.Fluent;
using NHibernate.Envers.Configuration.Store;
using NHibernate.Envers.Tools.Reflection;
using NHibernate.Mapping;
using NHibernate.Mapping.ByCode;

namespace Framework.DomainDriven.NHibernate.Audit;

/// <summary>
/// Knows how to create configuration data
/// </summary>
internal class AuditMetadataProvider<TAuditRevisionEntity> : IMetaDataProvider
        where TAuditRevisionEntity : AuditRevisionEntity
{
    private IList<Type> _filledTypes;

    private readonly string _auditTableSuffix;

    private readonly IEntityTrackingRevisionListener _entityTrackingRevisionListener;

    private readonly IList<IMappingSettings> mappingSettings;

    private readonly string auditEntityRevisionSchema;


    /// <summary>
    /// Initializes a new instance of the <see cref="AuditMetadataProvider{TAuditRevisionEntity}"/> class.
    /// </summary>
    /// <param name="monitorableTypes">The monitorable types.</param>
    /// <param name="auditDatabaseName">The audit schema.</param>
    /// <param name="auditTableSuffix">The audit table suffix.</param>
    /// <param name="entityTrackingRevisionListener">The entity tracking revision listener.</param>
    public AuditMetadataProvider(
            IList<IMappingSettings> mappingSettings,
            string auditEntityRevisionSchema,
            string auditTableSuffix,
            IEntityTrackingRevisionListener entityTrackingRevisionListener)
    {
        this.mappingSettings = mappingSettings;
        this.auditEntityRevisionSchema = auditEntityRevisionSchema;


        this._auditTableSuffix = auditTableSuffix;
        this._entityTrackingRevisionListener = entityTrackingRevisionListener;

    }

    /// <summary>
    /// Creates the meta data.
    /// </summary>
    /// <param name="nhConfiguration">The NH Configuration.</param>
    /// <returns>
    /// A dictionary of <see cref="IEntityMeta"/>, keyed by entity type
    /// </returns>
    public IDictionary<Type, IEntityMeta> CreateMetaData(Configuration nhConfiguration)
    {
        this._filledTypes = new List<Type>();

        var ret = new Dictionary<Type, IEntityMeta>();

        nhConfiguration.AddMapping(this.CreateRevisionInfoMappingDocument());

        var auditAttributeService = this.mappingSettings.GetAuditAttributeService(nhConfiguration.ClassMappings);

        var auditedTypes = this.mappingSettings
                               .SelectMany(
                                           z =>
                                           {
                                               var filter = z.GetAuditTypeFilter();
                                               return z.Types.Where(filter.IsAuditedType);
                                           });

        var filteredAuditedClassMappings = auditedTypes
                                           .Join(
                                                 nhConfiguration.ClassMappings,
                                                 z => z.FullName,
                                                 z => z.EntityName,
                                                 (type, persistentClass) => new { Type = type, PersistentClass = persistentClass })
                                           .ToList();

        foreach (var pair in filteredAuditedClassMappings)
        {
            var persistentClass = pair.PersistentClass;

            var propertyIterator = persistentClass.PropertyIterator;

            this.AddForEntity(persistentClass, ret, auditAttributeService);
            this.AddForComponent(propertyIterator, ret, auditAttributeService);
        }

        var auditRevisionType = typeof(TAuditRevisionEntity);

        ret[auditRevisionType] = new EntityMeta();


        var entityMeta = ((EntityMeta)ret[auditRevisionType]);

        entityMeta.AddClassMeta(new RevisionEntityAttribute { Listener = this._entityTrackingRevisionListener });


        Expression<Func<TAuditRevisionEntity, long>> revisionNumber = z => z.Id;
        Expression<Func<TAuditRevisionEntity, DateTime>> revisionTimestamp = z => z.RevisionDate;


        entityMeta.AddMemberMeta(revisionNumber.MethodInfo(), new RevisionNumberAttribute());
        entityMeta.AddMemberMeta(revisionTimestamp.MethodInfo(), new RevisionTimestampAttribute());

        this.ThrowIfUsingNonMappedRevisionEntity(nhConfiguration);

        return ret;
    }

    private void ThrowIfUsingNonMappedRevisionEntity(Configuration nhConfiguration)
    {
        if (null == nhConfiguration.GetClassMapping(typeof(TAuditRevisionEntity)))
        {
            throw new MappingException("Custom revision entity " + typeof(TAuditRevisionEntity).Name + " must be mapped!");
        }
    }

    private HbmMapping CreateRevisionInfoMappingDocument()
    {
        var mapper = new ModelMapper();

        mapper.Class<TAuditRevisionEntity>(mapping =>
                                           {
                                               mapping.Id(z => z.Id, idMapper => idMapper.Generator(Generators.Native, z => z.Params(new KeyValuePair<string, string>("sequence", "generateidsequence"))));

                                               mapping.Property(z => z.RevisionDate, z => { z.Insert(true); z.Update(false); });
                                               mapping.Property(z => z.Author, z => { z.Insert(true); z.Update(false); });
                                               mapping.Table(typeof(AuditRevisionEntity).Name);
                                               mapping.Schema(this.auditEntityRevisionSchema);
                                           });

        var result = mapper.CompileMappingForAllExplicitlyAddedEntities();

        return result;
    }

    private void AddForComponent(IEnumerable<Property> propertyIterator, Dictionary<Type, IEntityMeta> dicToFill, IAuditAttributeService auditService)
    {
        foreach (var property in propertyIterator)
        {
            var propAsComponent = property.Value as Component;
            if (propAsComponent == null || propAsComponent.IsDynamic)
            {
                continue;
            }

            this.FillClass(propAsComponent.ComponentClass, dicToFill, auditService);
            this.FillMembers(propAsComponent.ComponentClass, propAsComponent.PropertyIterator, dicToFill, auditService);
        }
    }

    private void AddForEntity(PersistentClass persistentClass, Dictionary<Type, IEntityMeta> dicToFill, IAuditAttributeService auditService)
    {
        var typ = persistentClass.MappedClass;

        this.FillClass(typ, dicToFill, auditService);

        var props = new List<Property>();

        props.AddRange(persistentClass.PropertyIterator);

        if (persistentClass.IdentifierProperty != null && !persistentClass.IdentifierProperty.IsComposite)
        {
            props.Add(persistentClass.IdentifierProperty);
        }

        this.FillMembers(typ, props, dicToFill, auditService);
    }

    private void FillMembers(Type type, IEnumerable<Property> properties, Dictionary<Type, IEntityMeta> dicToFill, IAuditAttributeService auditService)
    {
        foreach (var propInfo in PropertyAndMemberInfo.PersistentInfo(type, properties))
        {
            if (!dicToFill.ContainsKey(type))
            {
                dicToFill[type] = new EntityMeta();
            }

            var entityMeta = (EntityMeta)dicToFill[type];

            var mode = auditService.GetAttributeFor(type, propInfo.Property);
            if (mode == RelationTargetAuditMode.NotAudited)
            {
                entityMeta.AddMemberMeta(propInfo.Member, new NotAuditedAttribute());
            }
            else
            {
                entityMeta.AddMemberMeta(propInfo.Member, new AuditedAttribute());
            }
        }
    }

    private void FillClass(Type type, Dictionary<Type, IEntityMeta> dicToFill, IAuditAttributeService auditService)
    {
        if (!this._filledTypes.Contains(type) && !type.IsAbstract)
        {
            if (!dicToFill.ContainsKey(type))
            {
                dicToFill[type] = new EntityMeta();
            }

            var auditMode = auditService.GetAttributeFor(type);

            var classAttributeToAdd = new AuditedAttribute() { TargetAuditMode = auditMode }; //ClassAttribute(attr, type);

            ((EntityMeta)dicToFill[type]).AddClassMeta(classAttributeToAdd);

            var tableName = type.GetTableName(string.Empty).tableName;

            var auditTableAttribute = new AuditTableAttribute(tableName + this._auditTableSuffix)
                                      {
                                              Schema = auditService.GetAuditTableSchemaOrDefault(type) ?? this.auditEntityRevisionSchema
                                      };

            ((EntityMeta)dicToFill[type]).AddClassMeta(auditTableAttribute);
        }

        var baseType = type.BaseType;
        if (!type.IsInterface && baseType != typeof(object))
        {
            this.FillClass(baseType, dicToFill, auditService);
        }
    }
}
