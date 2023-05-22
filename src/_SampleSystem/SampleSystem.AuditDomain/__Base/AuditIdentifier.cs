using System;

namespace SampleSystem.AuditDomain
{
    public struct AuditIdentifier
    {
        private long revNumber;

        private Guid id;


        public long RevNumber => this.revNumber;

        public Guid Id => this.id;
    }
}
