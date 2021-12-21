using System;
using System.Linq;
using Framework.Core;

namespace Framework.DomainDriven.BLL.Security.Lock
{
    public static class NamedLockExtension
    {
        public static TNamedLockOperation GetGlobalLock<TNamedLockOperation>(Type domainType)
        {

            var allFields = typeof(TNamedLockOperation).GetFields();

            var sourceWithAttributes = allFields.Select(z =>
                                                        new
                                                            {
                                                                Source = z,
                                                                GlobalLockAttribute = z.GetCustomAttributes(false).OfType<GlobalLockAttribute>().FirstOrDefault()
                                                            });

            var notNullValues = sourceWithAttributes
                .Where(z => null != z.GlobalLockAttribute)
                .Where(z => z.GlobalLockAttribute.DomainType == domainType);

            var result = notNullValues.Single(
                ()=> new System.ArgumentException(
                         $"{typeof(TNamedLockOperation).Name} must has value for {domainType.Name} lock with attribute {typeof(GlobalLockAttribute).Name}"),
                () => new System.ArgumentException(
                          $"{typeof(TNamedLockOperation).Name} have more then one value for {domainType.Name} lock with attribute {typeof(GlobalLockAttribute).Name}"));

            return (TNamedLockOperation)result.Source.GetValue(null);
        }
    }
}