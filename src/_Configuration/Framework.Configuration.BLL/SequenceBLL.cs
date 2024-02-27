using Framework.Configuration.Domain;
using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.Lock;

namespace Framework.Configuration.BLL;

public partial class SequenceBLL
{
    public long GetNextNumber(string name)
    {
        this.LockSequence();

        var sequence = this.GetByName(name);

        if (null == sequence)
        {
            sequence = this.GetByName(name) ?? new Sequence { Name = name, Number = 0 }.Self(this.Save);
        }

        this.Lock(sequence, LockRole.Update);

        sequence.Number++;

        this.Save(sequence);

        return sequence.Number;
    }

    protected virtual void LockSequence() => this.Context.NamedLockService.LockAsync(ConfigurationNamedLock.UpdateSequence, LockRole.Update).GetAwaiter().GetResult();

    public Sequence Create(SequenceCreateModel createModel) => new();
}
