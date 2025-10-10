using Framework.ApplicationVariable;
using Framework.Core;

namespace SampleSystem.BLL;

public static class SampleSystemSystemConstant
{
    public static readonly ApplicationVariable<DateTime> SampleDateConstant =
        new(nameof(SampleDateConstant), DateTime.Now.ToStartMonthDate()) { Description = nameof(SampleDateConstant) };

    public static readonly ApplicationVariable<int> SampleInt32Constant = new(nameof(SampleInt32Constant), 123) { Description = nameof(SampleInt32Constant) };

    public static readonly ApplicationVariable<string> SampleStringConstant = new(nameof(SampleStringConstant), "HelloWorld") { Description = nameof(SampleStringConstant) };
}