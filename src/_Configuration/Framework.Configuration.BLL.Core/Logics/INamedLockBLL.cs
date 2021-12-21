using Framework.DomainDriven;
using Framework.Configuration.Domain;

namespace Framework.Configuration.BLL
{
    public partial interface INamedLockBLL
    {
        void CheckInit();

        void Lock(NamedLockOperation regularJobLock, LockRole lockRole);
    }
}