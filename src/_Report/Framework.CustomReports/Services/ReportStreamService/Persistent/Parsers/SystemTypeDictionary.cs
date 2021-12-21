using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Framework.Core;
using Framework.DomainDriven.SerializeMetadata;
using Framework.QueryLanguage;

namespace Framework.CustomReports.Services
{
    public class SystemTypeDictionary
    {
        private readonly ReadOnlyCollection<TypeMetadata> _sourceTypes;

        private readonly Dictionary<string, TypeMetadataDictionary> dict;

        public SystemTypeDictionary(ReadOnlyCollection<TypeMetadata> types)
        {
            this._sourceTypes = types;
            this.dict = new Dictionary<string, TypeMetadataDictionary>();
        }

        public TypeMetadataDictionary this[string key, bool allowNull]
        {
            get
            {
                var normKey = key.ToLower();
                TypeMetadataDictionary result;
                if (!this.dict.TryGetValue(normKey, out result))
                {
                    var type = this._sourceTypes.FirstOrDefault(z => z.Type.Name.ToLower() == normKey);
                    if (null == type)
                    {
                        throw new ArgumentOutOfRangeException(string.Format("Type:'{0}' not in system", key));
                    }

                    result = new TypeMetadataDictionary(type, allowNull);

                    this.dict.Add(normKey, result);
                }

                return result;
            }
        }

        public TypeMetadataDictionary GetPropertyType(string domainTypeName, PropertyExpression propertyExpression)
        {
            var propertyPath = propertyExpression.GetAllElements(z => (z.Source as PropertyExpression)).Select(z => z.PropertyName).Reverse();

            var domainType = this[domainTypeName, true];

            var result = propertyPath.Aggregate(
                domainType,
                (prev, current) =>
                {
                    var property = prev[current];
                    return this[property.Type.Name, property.AllowNull];
                });

            return result;
        }
    }
}