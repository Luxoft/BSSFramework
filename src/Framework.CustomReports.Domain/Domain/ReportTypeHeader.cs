using System;

using Framework.DomainDriven.SerializeMetadata;

namespace Framework.CustomReports.Domain
{
    public class ReportTypeHeader : TypeHeader, IEquatable<ReportTypeHeader>
    {
        private readonly Guid reportIdent;

        private readonly long reportVersion;

        public ReportTypeHeader(string name, Guid reportIdent, long reportVersion)
            : base(name)
        {
            this.reportIdent = reportIdent;
            this.reportVersion = reportVersion;
        }

        public override string GenerateName => $"{this.Name}|{this.reportIdent}|{this.reportVersion}";

        public override bool Equals(TypeHeader other)
        {
            return this.Equals(other as ReportTypeHeader);
        }

        public bool Equals(ReportTypeHeader other)
        {
            return base.Equals(other)
                   && other.reportVersion == this.reportVersion
                   && other.reportIdent == this.reportIdent;
        }
    }
}
