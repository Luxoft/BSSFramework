using System.Collections.Generic;

using Framework.Core;

namespace Framework.Configuration.Domain.Models.Filters
{
    public class UserActionObjectRootFilterModel : DomainObjectContextFilterModel<UserActionObject>
    {
        public int CountingEntities { get; set; }

        public Period Period { get; set; }

        public List<string> DomainTypeNames { get; set; }

        public List<string> ActionNames { get; set; }
    }
}