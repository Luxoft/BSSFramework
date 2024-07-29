using Framework.Configuration;
using Framework.Core;

namespace SampleSystem.BLL;

public static class SampleSystemSystemConstant
{
    public static readonly SystemConstant<DateTime> SampleDateConstant = new(nameof(SampleDateConstant), DateTime.Now.ToStartMonthDate(), "SampleDateConstant");

    public static readonly SystemConstant<int> SampleInt32Constant = new(nameof(SampleInt32Constant), 123, "SampleInt32Constant");

    public static readonly SystemConstant<string> SampleStringConstant = new(nameof(SampleStringConstant), "HelloWorld", "SampleStringConstant");
}
