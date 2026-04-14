using Framework.CodeGeneration.DTOGenerator.PropertyAssigner;

namespace Framework.CodeGeneration.DTOGenerator.Server.PropertyAssigner;

public interface IPropertyAssignerConfigurator
{
    IPropertyAssigner GetStrictSecurityToDomainObjectPropertyAssigner(IPropertyAssigner innerAssigner);

    IPropertyAssigner GetUpdateSecurityToDomainObjectPropertyAssigner(IPropertyAssigner innerAssigner);

    IPropertyAssigner GetDomainObjectToSecurityPropertyAssigner(IPropertyAssigner innerAssigner);
}
