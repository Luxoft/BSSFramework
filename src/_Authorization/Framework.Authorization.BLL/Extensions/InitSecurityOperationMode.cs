using System;

namespace Framework.Authorization.BLL;

[Flags]
public enum InitSecurityOperationMode
{
    Add = 1,

    Remove = 2
}
