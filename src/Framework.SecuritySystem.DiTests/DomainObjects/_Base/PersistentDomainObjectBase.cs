using System;

using Framework.Persistent;

namespace Framework.SecuritySystem.DiTests
{
    public class PersistentDomainObjectBase : IIdentityObject<Guid>
    {
        public Guid Id { get; set; }
    }
}
