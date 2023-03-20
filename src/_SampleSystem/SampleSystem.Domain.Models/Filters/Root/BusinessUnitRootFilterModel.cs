using System;
using System.Collections.Generic;

using Framework.Core;

namespace SampleSystem.Domain;

/// <summary>
///
/// </summary>
/// <seealso cref="SampleSystem.Domain.DomainObjectRootFilterModel{SampleSystem.Domain.BusinessUnit}" />
public class BusinessUnitRootFilterModel : DomainObjectRootFilterModel<BusinessUnit>
{
    /// <summary>
    /// Gets or sets the employee.
    /// </summary>
    /// <value>
    /// The employee.
    /// </value>
    public Employee Employee { get; set; }

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
