using Framework.ApplicationVariable;
using Framework.Core;

namespace SampleSystem.BLL;

public static class SampleSystemSystemConstant
{
    public static readonly ApplicationVariable<DateTime> SampleDateConstant = new(nameof(SampleDateConstant), "SampleDateConstant", DateTime.Now.ToStartMonthDate());

    public static readonly ApplicationVariable<int> SampleInt32Constant = new(nameof(SampleInt32Constant), "SampleInt32Constant", 123);

    public static readonly ApplicationVariable<string> SampleStringConstant = new(nameof(SampleStringConstant), "SampleStringConstant", "HelloWorld");
}
