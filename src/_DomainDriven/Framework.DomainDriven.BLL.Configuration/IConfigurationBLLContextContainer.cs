namespace Framework.DomainDriven.BLL.Configuration
{
    public interface IConfigurationBLLContextContainer<out TConfigurationBLLContext>
    {
        TConfigurationBLLContext Configuration { get; }
    }
}