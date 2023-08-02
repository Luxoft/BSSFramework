using Framework.Validation;

namespace SampleSystem.BLL;

public class SampleSystemValidatorCompileCache : ValidatorCompileCache
{
    public SampleSystemValidatorCompileCache(SampleSystemValidationMap sampleSystemValidationMap) :
            base(sampleSystemValidationMap)
    {
    }
}
