using System;
using System.Collections.Generic;

using Framework.DomainDriven.BLL;

namespace Framework.Configuration.Domain.Models.Create
{
    [DBSessionMode(DBSessionMode.Write)]
    public class UserActionCreateModel : DomainObjectCreateModel<UserAction>
    {
        public IList<UserActionObjectModel> ObjectIdentities { get; set; }

        public string DomainType { get; set; }

        public string Name { get; set; }
    }
}
