using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Framework.Core;

using JetBrains.Annotations;

namespace Framework.Projection
{
    internal class CollectionOfProjectionType : BaseTypeImpl
    {
        private readonly Type originalType;

        private readonly Type checkCollectionBlankType;
        private readonly Type elementType;

        public CollectionOfProjectionType([NotNull] Type originalType)
        {
            this.originalType = originalType ?? throw new ArgumentNullException(nameof(originalType));

            this.elementType = this.originalType.GetGenericArguments().Single();
            this.checkCollectionBlankType = this.originalType.GetGenericTypeDefinition().MakeGenericType(typeof(object));
        }

        public override string Name => this.originalType.Name;

        public override string Namespace => this.originalType.Namespace;

        public override string FullName => this.originalType.FullName;

        public override Assembly Assembly => this.originalType.Assembly;

        public override Type BaseType => this.originalType.BaseType;

        public override string AssemblyQualifiedName => this.originalType.AssemblyQualifiedName;

        public override bool ContainsGenericParameters => this.originalType.ContainsGenericParameters;

        public override IEnumerable<CustomAttributeData> CustomAttributes => this.originalType.CustomAttributes;

        public override MethodBase DeclaringMethod => this.originalType.DeclaringMethod;

        public override Type DeclaringType => this.originalType.DeclaringType;

        public override bool IsGenericType => this.originalType.IsGenericType;

        public override Type[] GetGenericArguments() => this.originalType.GetGenericArguments();

        public override Type GetGenericTypeDefinition() => this.originalType.GetGenericTypeDefinition();

        public override bool IsEnum => this.originalType.IsEnum;

        public override Type UnderlyingSystemType => this.originalType.UnderlyingSystemType;

        public override Type[] GetInterfaces() => this.originalType.GetInterfaces();

        protected override bool IsArrayImpl() => this.originalType.IsArray;

        public override object[] GetCustomAttributes(bool inherit) => this.originalType.GetCustomAttributes(inherit);

        public override object[] GetCustomAttributes(Type attributeType, bool inherit) => this.originalType.GetCustomAttributes(attributeType, inherit);

        protected override TypeAttributes GetAttributeFlagsImpl() => this.originalType.Attributes;

        public override bool IsAssignableFrom(Type targetType)
        {
            if (targetType.IsGenericType)
            {
                var targetElementTypes = targetType.GetGenericArguments();

                if (targetElementTypes.Length == 1)
                {
                    var checkTargetCollectionBlackType = targetType.GetGenericTypeDefinition().MakeGenericType(typeof(object));

                    return this.checkCollectionBlankType.IsAssignableFrom(checkTargetCollectionBlackType)
                           && this.elementType.IsAssignableFrom(targetElementTypes.Single());
                }
            }

            return false;
        }
    }
}
