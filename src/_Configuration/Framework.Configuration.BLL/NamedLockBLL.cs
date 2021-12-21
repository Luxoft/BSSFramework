using System;
using System.Linq;
using Framework.Core;
using Framework.DomainDriven;
using Framework.Configuration.Domain;

namespace Framework.Configuration.BLL
{
    public partial class NamedLockBLL
    {
        public void CheckInit()
        {
            // TODO move generate enums into generate db script
            MutexHelper.GlobalLock(this.GetType().FullName + "_InitLock", () =>
                {
                    var actualValues = Enum.GetValues(typeof (NamedLockOperation)).Cast<NamedLockOperation>();
                    var expectedValues = this.GetFullList();

                    var mergeResult = expectedValues.GetMergeResult(actualValues, z => (int) z.LockOperation, z => (int) z);

                    mergeResult.AddingItems.Select(z => new NamedLock() {LockOperation = z}).Foreach(this.Save);
                });
        }

        public void Lock(NamedLockOperation lockOperation, LockRole lockRole)
        {
            var namedLock = this.GetObjectBy(z => z.LockOperation == lockOperation);

            base.Lock(namedLock, lockRole);
        }
    }
}
