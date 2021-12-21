using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.DomainDriven.BLL.Security.Lock
{
    public interface INamedLock<out TNamedLockOperation>
        where TNamedLockOperation : struct, Enum
    {
        TNamedLockOperation LockOperation { get; }
    }
}
