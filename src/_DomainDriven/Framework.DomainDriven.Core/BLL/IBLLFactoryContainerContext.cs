namespace Framework.DomainDriven.BLL
{
    public interface IBLLFactoryContainerContext<out TBLLFactoryContainer>
    {
        TBLLFactoryContainer Logics { get; }
    }
}