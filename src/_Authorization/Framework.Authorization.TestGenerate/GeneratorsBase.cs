using Framework.Core;
using Framework.FileGeneration.Checkout;

namespace Framework.Authorization.TestGenerate;

public abstract class GeneratorsBase
{
    protected ICheckOutService CheckOutService { get; } = Framework.FileGeneration.Checkout.CheckOutService.Trace;

    protected virtual string FrameworkPath { get; } = Environment.CurrentDirectory.Replace(@"\", @"/").TakeWhileNot(@"/src/", StringComparison.InvariantCultureIgnoreCase);

    protected abstract string GeneratePath { get; }
}
