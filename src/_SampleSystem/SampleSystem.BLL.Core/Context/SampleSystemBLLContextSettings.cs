using System;

using Framework.Core;

using SampleSystem.Domain;

namespace SampleSystem.BLL;

public class SampleSystemBLLContextSettings : ISampleSystemBLLContextSettings
{
    public ITypeResolver<string> TypeResolver { get; init; } = TypeSource.FromSample<PersistentDomainObjectBase>().ToDefaultTypeResolver();
}
