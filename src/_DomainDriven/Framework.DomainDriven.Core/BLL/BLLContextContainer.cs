using JetBrains.Annotations;

namespace Framework.DomainDriven.BLL;

public class BLLContextContainer<TBLLContext> : IBLLContextContainer<TBLLContext>
        where TBLLContext : class
{
    public BLLContextContainer([NotNull] TBLLContext context)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));

        this.Context = context;
    }

    public TBLLContext Context { get; private set; }
}
