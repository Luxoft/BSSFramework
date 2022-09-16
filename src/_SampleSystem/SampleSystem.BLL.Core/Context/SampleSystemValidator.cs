using Framework.Validation;

using Microsoft.Extensions.DependencyInjection;

namespace SampleSystem.BLL;

public partial class SampleSystemValidator : ISampleSystemValidator
{
    [ActivatorUtilitiesConstructor]
    public SampleSystemValidator(ISampleSystemBLLContext context, SampleSystemValidatorCompileCache cache) :
            this(context, (ValidatorCompileCache)cache)
    {
    }
}
