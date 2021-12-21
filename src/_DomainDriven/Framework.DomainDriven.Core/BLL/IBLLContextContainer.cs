namespace Framework.DomainDriven.BLL
{
    public interface IBLLContextContainer<out TBLLContext>
    {
        TBLLContext Context { get; }
    }
}