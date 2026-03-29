namespace Framework.BLL;

public interface IBLLContextContainer<out TBLLContext>
{
    TBLLContext Context { get; }
}
