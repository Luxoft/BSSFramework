using Framework.Core;

using SampleSystem.Domain.BU;
using SampleSystem.Domain.Models.Filters._Base;

namespace SampleSystem.Domain.Models.Filters.Root;

/// <summary>
///
/// </summary>
/// <seealso cref="BusinessUnit" />
public class BusinessUnitRootFilterModel : DomainObjectRootFilterModel<BusinessUnit>
{
    /// <summary>
    /// Gets or sets the employee.
    /// </summary>
    /// <value>
    /// The employee.
    /// </value>
    public Employee.Employee Employee { get; set; }

    /// <summary>
    /// Gets or sets the list days.
    /// </summary>
    /// <value>
    /// The list days.
    /// </value>
    public List<DateTime> ListDays { get; set; }

    /// <summary>
    /// Gets or sets the array days.
    /// </summary>
    /// <value>
    /// The array days.
    /// </value>
    public DateTime[] ArrayDays { get; set; }

    /// <summary>
    /// Gets or sets the list periods.
    /// </summary>
    /// <value>
    /// The list periods.
    /// </value>
    public List<Period> ListPeriods { get; set; }

    /// <summary>
    /// Gets or sets the array periods.
    /// </summary>
    /// <value>
    /// The array periods.
    /// </value>
    public Period[] ArrayPeriods { get; set; }
}
