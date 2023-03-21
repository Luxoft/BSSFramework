using System;

namespace Framework.DomainDriven.DTOGenerator.Server;

public interface IPropertyAssignerConfigurator
{
    IPropertyAssigner GetStrictSecurityToDomainObjectPropertyAssigner(IPropertyAssigner innerAssigner);

    IPropertyAssigner GetUpdateSecurityToDomainObjectPropertyAssigner(IPropertyAssigner innerAssigner);

    IPropertyAssigner GetDomainObjectToSecurityPropertyAssigner(IPropertyAssigner innerAssigner);
}
