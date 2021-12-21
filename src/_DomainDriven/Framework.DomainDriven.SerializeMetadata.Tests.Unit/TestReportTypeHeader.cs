using System;

namespace Framework.DomainDriven.SerializeMetadata.Tests.Unit
{
    public class TestReportTypeHeader : TypeHeader, IEquatable<TestReportTypeHeader>
    {
        private readonly string reportName;

        public TestReportTypeHeader(string name, string reportName)
            : base(name)
        {
            this.reportName = reportName;
        }

        public override string GenerateName => $"{base.Name}|{this.reportName}";

        public override bool Equals(TypeHeader other)
        {
            return this.Equals(other as TestReportTypeHeader);
        }

        public bool Equals(TestReportTypeHeader other)
        {
            return base.Equals(other)
                   && other.reportName == this.reportName;
        }
    }
}
