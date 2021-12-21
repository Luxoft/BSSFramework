using System;
using System.Linq;

using Framework.Core;

using SampleSystem.Domain;

namespace SampleSystem.BLL
{
    public partial class NamedLockBLL
    {
        public void CheckInit()
        {
            var actualValues = Enum.GetValues(typeof(NamedLockOperation)).Cast<NamedLockOperation>();
            var expectedValues = this.GetFullList();

            var mergeResult = expectedValues.GetMergeResult(actualValues, z => (int)z.LockOperation, z => (int)z);

            mergeResult.AddingItems.Select(z => new NamedLock() { LockOperation = z }).Foreach(this.Save);
            mergeResult.RemovingItems.Foreach(this.Remove);

        }
    }
}