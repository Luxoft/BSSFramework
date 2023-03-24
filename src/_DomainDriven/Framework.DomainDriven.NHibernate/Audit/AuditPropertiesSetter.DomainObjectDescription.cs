using System;
using System.Collections.Concurrent;

using JetBrains.Annotations;

namespace Framework.DomainDriven.NHibernate.Audit;

internal partial class AuditPropertiesSetter
{
    public class DomainObjectDescription
    {
        private static readonly ConcurrentDictionary<Type, DomainObjectDescription> Cache = new ConcurrentDictionary<Type, DomainObjectDescription>();

        private DomainObjectDescription([NotNull] Type type, [NotNull] string[] propertyNames)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (propertyNames == null || propertyNames.Length == 0)
            {
                throw new ArgumentNullException(nameof(propertyNames));
            }

            this.Type = type;
            this.PropertyNames = propertyNames;
        }

        public Type Type { get; }

        public string[] PropertyNames { get; }

        public static DomainObjectDescription Get(Type type, string[] propertyNames)
        {
            var result = Cache.GetOrAdd(type, key => new DomainObjectDescription(key, propertyNames));
            return result;
        }

        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(this, obj))
            {
                return true;
            }

            var other = obj as DomainObjectDescription;
            return this.Type == other?.Type;
        }

        public override int GetHashCode()
        {
            return this.Type.GetHashCode();
        }

        public override string ToString()
        {
            return this.Type.ToString();
        }
    }
}
