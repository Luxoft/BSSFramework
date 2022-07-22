using System;

namespace Framework.DomainDriven.ServiceModel.Service
{
    public interface IDebugModeManager
    {
        bool IsDebugMode { get; }
    }
}
