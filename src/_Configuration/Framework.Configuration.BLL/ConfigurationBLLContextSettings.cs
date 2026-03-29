using Framework.BLL;
using Framework.Configuration.Domain;
using Framework.Core.Serialization;
using Framework.Core.TypeResolving;

namespace Framework.Configuration.BLL;

public class ConfigurationBLLContextSettings : BLLContextSettings<PersistentDomainObjectBase>
{
    public ISerializerFactory<string> SystemConstantSerializerFactory { get; init; } = SerializerFactory.Default;

    public ITypeResolver<string> SystemConstantTypeResolver { get; init; } = TypeResolverHelper.Base;
}
