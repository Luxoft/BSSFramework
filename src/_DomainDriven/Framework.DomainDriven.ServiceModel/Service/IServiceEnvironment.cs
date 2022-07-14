using System;

using Framework.DomainDriven.BLL;

namespace Framework.DomainDriven.ServiceModel.Service
{
    public interface IServiceEnvironment
    {
        bool IsDebugMode { get; }
    }
}
