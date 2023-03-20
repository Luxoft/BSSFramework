using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Linq;

using Framework.Persistent;

using JetBrains.Annotations;

using NHibernate.Cfg;

namespace Framework.DomainDriven.NHibernate;

public class MappingSettings<TPersistentDomainObjectBase> : MappingSettings
{
    public MappingSettings([NotNull] Assembly mappingAssembly, IEnumerable<Type> types = null)
            : this(mappingAssembly, new DatabaseName(typeof(TPersistentDomainObjectBase).GetTargetSystemName()), types)
    {
    }

    public MappingSettings([NotNull] Assembly mappingAssembly, DatabaseName databaseName, IEnumerable<Type> types = null)
            : this(mappingAssembly, databaseName, null, types)
    {
    }

    public MappingSettings([NotNull] Assembly mappingAssembly, DatabaseName databaseName, AuditDatabaseName auditDatabaseName, IEnumerable<Type> types = null)
            : base(typeof(TPersistentDomainObjectBase), mappingAssembly, databaseName, auditDatabaseName, types)
    {
    }

    public MappingSettings([NotNull] IEnumerable<XDocument> mappingXmls, DatabaseName databaseName, bool auditDatabaseName, IEnumerable<Type> types = null)
            : this(mappingXmls, databaseName, auditDatabaseName ? databaseName.ToDefaultAudit() : null, types)
    {
    }

    public MappingSettings([NotNull] IEnumerable<XDocument> mappingXmls, DatabaseName databaseName, AuditDatabaseName auditDatabaseName, IEnumerable<Type> types = null)
            : base(typeof(TPersistentDomainObjectBase), mappingXmls, databaseName, auditDatabaseName, types)
    {
    }

    public MappingSettings([NotNull] IEnumerable<XDocument> mappingXmls, bool isAudit)
            : this(mappingXmls, new DatabaseName(typeof(TPersistentDomainObjectBase).GetTargetSystemName()),
                   isAudit ? new DatabaseName(typeof(TPersistentDomainObjectBase).GetTargetSystemName()).ToDefaultAudit() : null)
    {
    }

    public MappingSettings([NotNull] Action<Configuration> initMappingAction, DatabaseName databaseName, bool isAudit, IEnumerable<Type> types = null)
            : this(initMappingAction, databaseName, isAudit ? databaseName.ToDefaultAudit() : null, types)
    {
    }

    public MappingSettings([NotNull] Action<Configuration> initMappingAction, DatabaseName databaseName, AuditDatabaseName auditDatabaseName, IEnumerable<Type> types = null)
            : base(typeof(TPersistentDomainObjectBase), initMappingAction, databaseName, auditDatabaseName, types)
    {
    }
}
