using System;

using Framework.Core;

using SampleSystem.Domain;

namespace SampleSystem.BLL;

public class SampleSystemBLLContextSettings : ISampleSystemBLLContextSettings
{
    public ITypeResolver<string> TypeResolver { get; } = TypeSource.FromSample<PersistentDomainObjectBase>().ToDefaultTypeResolver();
}
