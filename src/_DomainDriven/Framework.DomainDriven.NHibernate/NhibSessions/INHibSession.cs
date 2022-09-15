﻿using Framework.DomainDriven.DAL.Revisions;

using NHibernate;
using NHibernate.Envers.Patch;

namespace Framework.DomainDriven.NHibernate;

public interface INHibSession : IDBSession
{
    IAuditReaderPatched AuditReader { get; }

    ISession NativeSession { get; }

    void RegisterModified<TDomainObject>(TDomainObject domainObject, ModificationType modificationType);
}
