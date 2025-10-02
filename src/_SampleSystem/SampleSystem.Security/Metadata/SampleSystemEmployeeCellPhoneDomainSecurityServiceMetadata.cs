using SampleSystem.Domain;

using SecuritySystem.DependencyInjection.DomainSecurityServiceBuilder;

namespace SampleSystem.Security.Metadata;

public class SampleSystemEmployeeCellPhoneDomainSecurityServiceMetadata : IDomainSecurityServiceMetadata<EmployeeCellPhone>
{
    public static void Setup(IDomainSecurityServiceBuilder<EmployeeCellPhone> builder) =>
        builder.SetDependency(v => v.Employee);
}
