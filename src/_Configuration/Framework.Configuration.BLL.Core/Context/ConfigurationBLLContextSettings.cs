using System;

using Framework.Core;

using Framework.Configuration.Domain;
using Framework.Core.Serialization;

namespace Framework.Configuration.BLL;

public class ConfigurationBLLContextSettings : IConfigurationBLLContextSettings
{
    public ITypeResolver<string> TypeResolver { get; } = TypeSource.FromSample<PersistentDomainObjectBase>().ToDefaultTypeResolver();

    public ISerializerFactory<string> SystemConstantSerializerFactory { get; } = SerializerFactory.Default;
}
