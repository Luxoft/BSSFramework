using System;

namespace Framework.DomainDriven.ServiceModel.Service
{
    public interface IServiceEnvironment
    {
        bool IsDebugMode { get; }
    }
}
