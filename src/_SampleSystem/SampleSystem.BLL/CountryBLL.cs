using SecuritySystem;

using SampleSystem.Domain;

namespace SampleSystem.BLL;

public partial class CountryBLL
{
    // Manual BLL Constructor example. For configuration see BLLGeneratorConfiguration.cs
    public CountryBLL(
            ISampleSystemBLLContext context,
            ISecurityProvider<Country> securityProvider)
            : base(context, securityProvider)
    {
    }
}
