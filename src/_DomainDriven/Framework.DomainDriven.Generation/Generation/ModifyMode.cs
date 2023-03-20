using System;

namespace Framework.DomainDriven;

[Flags]
public enum ModifyMode
{
    None = 1,
    RemoveNotExistsTable = 2,
    RemoveNotExistsColumns = 4,
}
