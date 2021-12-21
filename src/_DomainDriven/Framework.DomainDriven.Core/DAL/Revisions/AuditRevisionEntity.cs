using System;

namespace Framework.DomainDriven.DAL.Revisions
{
    public class AuditRevisionEntity
    {
        public virtual string Author { get; set; }
        public virtual long Id { get; protected internal set; }
        public virtual DateTime RevisionDate { get; protected internal set; }
    }
}