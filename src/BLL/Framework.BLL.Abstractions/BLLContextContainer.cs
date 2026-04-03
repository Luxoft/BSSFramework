namespace Framework.BLL;

public class BLLContextContainer<TBLLContext>(TBLLContext context) : IBLLContextContainer<TBLLContext>
    where TBLLContext : class
{
    public TBLLContext Context { get; } = context;
}
