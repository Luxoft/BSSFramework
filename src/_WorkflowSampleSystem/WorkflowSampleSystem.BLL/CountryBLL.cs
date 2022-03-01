using Framework.SecuritySystem;

using nuSpec.Abstraction;

using WorkflowSampleSystem.Domain;

namespace WorkflowSampleSystem.BLL
{
    public partial class CountryBLL
    {
        // Manual BLL Constructor example. For configuration see BLLGeneratorConfiguration.cs
        public CountryBLL(
                IWorkflowSampleSystemBLLContext context,
                ISecurityProvider<Country> securityProvider,
                ISpecificationEvaluator specificationEvaluator)
                : base(context, securityProvider, specificationEvaluator)
        {
        }
    }
}
