using NHibernate.Dialect;

namespace Framework.DomainDriven.NHibernate
{
    /// <summary>
    /// Represents extended dialect derived from MsSql2012Dialect. Defines nHibernate extensions that works with dates
    /// </summary>
    public class EnhancedMsSql2012Dialect : MsSql2012Dialect
    {
        /// <summary>
        /// Registers all our custom functions and defines corresponding MS SQL functions
        /// </summary>
        protected override void RegisterFunctions()
        {
            base.RegisterFunctions();

            foreach (var descriptor in SQLFunctionDescriptorStore.Descriptors)
            {
                this.RegisterFunction(descriptor.Key, descriptor.Value);
            }
        }
    }
}