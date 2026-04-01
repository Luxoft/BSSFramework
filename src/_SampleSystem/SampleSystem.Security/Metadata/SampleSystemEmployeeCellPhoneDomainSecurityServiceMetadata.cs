using SampleSystem.Domain;

using SecuritySystem.DependencyInjection.Domain;

namespace SampleSystem.Security.Metadata;

public class SampleSystemEmployeeCellPhoneDomainSecurityServiceMetadata : IDomainSecurityServiceMetadata<EmployeeCellPhone>
{
    public static void Setup(IDomainSecurityServiceSetup<EmployeeCellPhone> setup) =>
        setup.SetDependency(v => v.Employee);
}
