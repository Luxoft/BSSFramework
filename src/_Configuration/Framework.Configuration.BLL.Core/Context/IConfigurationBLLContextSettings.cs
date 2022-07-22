using Framework.Core;
using Framework.Core.Serialization;

namespace Framework.Configuration.BLL;

public interface IConfigurationBLLContextSettings : ITypeResolverContainer<string>
{
    ISerializerFactory<string> SystemConstantSerializerFactory { get; }
}
