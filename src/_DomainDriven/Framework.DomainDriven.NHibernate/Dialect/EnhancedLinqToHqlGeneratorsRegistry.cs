using NHibernate.Linq.Functions;

namespace Framework.DomainDriven.NHibernate;

/// <summary>
/// Represents DefaultLinqToHqlGeneratorsRegistry registry extensions that allows to
/// register custom generators
/// </summary>
public class EnhancedLinqToHqlGeneratorsRegistry : DefaultLinqToHqlGeneratorsRegistry
{
    /// <summary>
    /// Creates new registry instance and merges all out custom generators into nHibernate
    /// </summary>
    public EnhancedLinqToHqlGeneratorsRegistry()
    {
        this.Merge(new AddHoursGenerator());
        this.Merge(new AddDaysGenerator());
        this.Merge(new AddMonthsGenerator());
        this.Merge(new AddYearsGenerator());
        this.Merge(new DiffDaysHqlGenerator());
    }
}
