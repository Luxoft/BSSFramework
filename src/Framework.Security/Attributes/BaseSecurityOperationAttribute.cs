using System;

using JetBrains.Annotations;

namespace Framework.Security
{
    [AttributeUsage(AttributeTargets.Enum, AllowMultiple = true)]
    public class BaseSecurityOperationTypeAttribute : Attribute
    {
        public BaseSecurityOperationTypeAttribute()
            : this(typeof(SecurityOperationCode))
        {

        }

        public BaseSecurityOperationTypeAttribute([NotNull] Type baseSecurityOperationType)
        {
            if (baseSecurityOperationType == null) throw new ArgumentNullException(nameof(baseSecurityOperationType));

            this.BaseSecurityOperationType = baseSecurityOperationType;
        }


        public Type BaseSecurityOperationType { get; private set; }
    }
}
