using Framework.Configuration.Domain;
using Framework.Core.Serialization;
using Framework.DomainDriven;

namespace Framework.Configuration.BLL;

public class ConfigurationBLLContextSettings : BLLContextSettings<PersistentDomainObjectBase>
{
    public ISerializerFactory<string> SystemConstantSerializerFactory { get; init; } = SerializerFactory.Default;
}
