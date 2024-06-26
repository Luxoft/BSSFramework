using Framework.SecuritySystem.DependencyInjection.DomainSecurityServiceBuilder;

using SampleSystem.Domain;

namespace SampleSystem.Security.Metadata;

public class SampleSystemEmployeeCellPhoneDomainSecurityServiceMetadata : IDomainSecurityServiceMetadata<EmployeeCellPhone>
{
    public static void Setup(IDomainSecurityServiceBuilder<EmployeeCellPhone> builder) =>
        builder.SetDependency(v => v.Employee);
}
