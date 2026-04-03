using Framework.Core;
using Framework.FileGeneration.Checkout;

namespace Framework.Authorization.TestGenerate._Base;

public abstract class GeneratorsBase
{
    protected ICheckOutService CheckOutService { get; } = Framework.FileGeneration.Checkout.CheckOutService.Trace;

    protected virtual string FrameworkPath { get; } = System.Environment.CurrentDirectory.Replace(@"\", @"/").TakeWhileNot(@"/src/", StringComparison.InvariantCultureIgnoreCase);

    protected abstract string GeneratePath { get; }
}
