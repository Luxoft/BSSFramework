using System;

namespace Framework.Configuration.Domain.Models.Create
{
    public class UserActionObjectModel : DomainObjectBase
    {
        public Guid ObjectIdentity { get; set; }

        public string Name { get; set; }
    }
}
