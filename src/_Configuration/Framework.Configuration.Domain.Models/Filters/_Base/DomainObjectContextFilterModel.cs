using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Configuration.Domain;

public abstract class DomainObjectContextFilterModel<TDomainObject> : DomainObjectBase
        where TDomainObject : PersistentDomainObjectBase
{
}
