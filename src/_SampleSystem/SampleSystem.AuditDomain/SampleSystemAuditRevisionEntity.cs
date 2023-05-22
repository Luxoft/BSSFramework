using System;

namespace SampleSystem.AuditDomain
{
    public class SampleSystemAuditRevisionEntity : SystemAuditRevisionPersistentDomainObjectBase
    {
        private string author;

        private DateTime revisionDate;

        public virtual string Author => this.author;

        public virtual DateTime RevisionDate => this.revisionDate;
    }
}
