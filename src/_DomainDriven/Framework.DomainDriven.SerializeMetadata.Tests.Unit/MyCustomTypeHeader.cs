using System;

namespace Framework.DomainDriven.SerializeMetadata.Tests.Unit
{
    public class MyCustomTypeHeader : TypeHeader
    {
        private readonly string uniqueKey;

        public MyCustomTypeHeader(string name, string uniqueKey)
            : base(name)
        {
            this.uniqueKey = uniqueKey ?? throw new ArgumentNullException(nameof(uniqueKey));
        }

        public override string GenerateName => $"{base.GenerateName}{this.uniqueKey}";

        public override bool Equals(TypeHeader other)
        {
            return this.Equals(other as MyCustomTypeHeader);
        }

        public bool Equals(MyCustomTypeHeader other)
        {
            return base.Equals(other)
                   && other.uniqueKey == this.uniqueKey;
        }
    }
}
