using Framework.Core;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.Configuration.Domain;

namespace Framework.Configuration.BLL;

public partial class SequenceBLL
{
    public long GetNextNumber(string name)
    {
        var sequence = this.GetByName(name);

        if (null == sequence)
        {
            this.Context.Logics.NamedLock.Lock(NamedLockOperation.UpdateSequence, LockRole.Update);

            sequence = this.GetByName(name) ?? (new Sequence { Name = name, Number = 0 }.Self(this.Save));
        }

        this.Lock(sequence, LockRole.Update);

        sequence.Number++;

        this.Save(sequence);

        return sequence.Number;
    }

    public Sequence Create(SequenceCreateModel createModel)
    {
        return new Sequence();
    }
}
