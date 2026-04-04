using Framework.Subscriptions.Metadata;

namespace SampleSystem.Subscriptions.Metadata.Examples.RazerInheritance;

public abstract class LocalRazorTemplate<T> : RazorTemplate<T>
{
    protected string GetDateString(DateTime? dateTime) => dateTime.GetValueOrDefault().Date.ToString("dd MMM yyyy", this.Culture);

    protected string GetEmployeeName(Domain.Employee employee) => employee.NameNative.FullName;
}
