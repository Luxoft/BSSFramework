using SampleSystem.Domain;

using SecuritySystem.Providers;

namespace SampleSystem.BLL;

public partial class CountryBLL(
    ISampleSystemBLLContext context,
    ISecurityProvider<Country> securityProvider)
    : SecurityDomainBLLBase<Country>(context, securityProvider) 
{
    // Manual BLL Constructor example. For configuration see BLLGeneratorConfiguration.cs
}
