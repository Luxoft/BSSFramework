namespace Framework.BLL;

public interface IBLLFactoryContainerContext<out TBLLFactoryContainer>
{
    TBLLFactoryContainer Logics { get; }
}
