using System;
using System.Collections.Generic;
using System.Linq;
using Framework.DomainDriven.SerializeMetadata;
using Framework.Persistent;

namespace Framework.CustomReports.Services
{
    public class TypeMetadataDictionary
    {
        private readonly Dictionary<string, PropertyMetadata> dict;

        public TypeMetadataDictionary(TypeMetadata source, bool allowNull)
        {
            this.AllowNull = allowNull;
            this.Source = source;
            this.dict = new Dictionary<string, PropertyMetadata>();
        }

        public TypeMetadata Source { get; }

        public bool IsHierarhical
        {
            get
            {
                var domainMetadata = this.Source as DomainTypeMetadata;
                if (null == domainMetadata)
                {
                    return false;
                }

                return domainMetadata.IsHierarchical;
            }
        }

        public bool AllowNull { get; set; }

        public PropertyMetadata this[string key]
        {
            get
            {
                var normKey = key.ToLower();
                PropertyMetadata result;
                if (!this.dict.TryGetValue(normKey, out result))
                {
                    result = (this.Source as DomainTypeMetadata)?.Properties.FirstOrDefault(z => z.Name.ToLower() == normKey);
                    if (null == result)
                    {
                        throw new ArgumentOutOfRangeException($"Property:'{key}' not in type:'{this.Source.Type.Name}'");
                    }

                    this.dict.Add(normKey, result);
                }

                return result;
            }
        }
    }
}
