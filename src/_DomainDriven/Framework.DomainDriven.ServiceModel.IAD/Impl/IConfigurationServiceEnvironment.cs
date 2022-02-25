using Framework.DomainDriven.ServiceModel.Service;
using Framework.Configuration.BLL;

namespace Framework.DomainDriven.ServiceModel.IAD
{
    public interface IConfigurationServiceEnvironment : IServiceEnvironment<IConfigurationBLLContext>
    {
        IObjectStorage ObjectStorage { get; }
    }
}
