using System;
using Framework.Persistent;

namespace Framework.Configuration.BLL.Core.Tests.Unit.SubscriptionSystemService3
{
    public class IdentityObject : IIdentityObject<Guid>
    {
        public IdentityObject(Guid id)
        {
            this.Id = id;
        }

        public Guid Id { get; }

        public override string ToString()
        {
            return this.Id.ToString();
        }
    }
}
