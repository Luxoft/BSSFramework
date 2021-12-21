using System;

using Framework.DomainDriven.DTOGenerator.TypeScript.Facade;

namespace Framework.DomainDriven.FacadeServiceProxyGenerator
{
    public interface IFacadeConfiguration
    {
        Type BaseContract { get; }

        ITypeScriptMethodPolicy Policy { get; }
    }
}
