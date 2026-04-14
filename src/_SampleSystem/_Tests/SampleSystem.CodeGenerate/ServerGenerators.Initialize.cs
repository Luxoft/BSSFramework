using Framework.Core;
using Framework.FileGeneration.Checkout;

using SampleSystem.CodeGenerate.Configurations;

namespace SampleSystem.CodeGenerate;

public partial class ServerGenerators
{
    private readonly ServerGenerationEnvironment environment = new();

    private readonly string webApiNetCorePath = Path.Combine(TargetSystemPath, "SampleSystem.WebApiCore", @"Controllers/_Generated");

    private static string FrameworkPath { get; } = Environment.CurrentDirectory.Replace(@"\", @"/").TakeWhileNot(@"/src", StringComparison.InvariantCultureIgnoreCase) + @"/src";

    private static string TargetSystemPath => FrameworkPath + @"/_SampleSystem";

    private ICheckOutService CheckOutService { get; } = Framework.FileGeneration.Checkout.CheckOutService.Trace;
}
