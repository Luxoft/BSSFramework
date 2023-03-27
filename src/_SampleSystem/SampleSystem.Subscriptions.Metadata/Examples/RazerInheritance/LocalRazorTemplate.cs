using System.Globalization;

using Framework.Configuration.SubscriptionModeling;

using SampleSystem.Domain;

namespace LuxIM.Subscriptions.Metadata;

public abstract class LocalRazorTemplate<T> : RazorTemplate<T>
{
    private static readonly CultureInfo CultureInfo = new CultureInfo("en-US");

    protected string GetDateString(DateTime? dateTime)
    {
        return dateTime.GetValueOrDefault().Date.ToString("dd MMM yyyy", CultureInfo);
    }

    protected string GetEmployeeName(Employee employee)
    {
        return employee.NameNative.FullName;
    }
}
