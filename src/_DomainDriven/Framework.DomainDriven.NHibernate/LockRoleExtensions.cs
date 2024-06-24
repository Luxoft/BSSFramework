using Framework.DomainDriven.Lock;

using NHibernate;

namespace Framework.DomainDriven.NHibernate;

public static class LockRoleExtensions
{
    public static LockMode ToLockMode(this LockRole source)
    {
        switch (source)
        {
            case LockRole.None: return LockMode.None;
            case LockRole.Update: return LockMode.Upgrade;
            case LockRole.Read: return LockMode.Read;
            case LockRole.NoWait: return LockMode.UpgradeNoWait;
            default: throw new ArgumentException($"LockRole:{source} can't be converted to LockMode");
        }
    }
}
