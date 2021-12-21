using System;

using Framework.DomainDriven.DTOGenerator.Client;

namespace Framework.DomainDriven.FacadeServiceProxyGenerator
{
    public interface IGenerationEnvironmentBase : IClientGenerationEnvironmentBase
    {
        IClientGeneratorConfigurationBase<IClientGenerationEnvironmentBase> ClientDTO { get; }
    }
}
