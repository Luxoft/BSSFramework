using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Framework.Core;
using Framework.Persistent;

using JetBrains.Annotations;

namespace Framework.Projection
{
    internal class ExplicitProperty : BasePropertyInfoImpl
    {
        internal readonly PropertyInfo InterfaceProp;

        private readonly string baseName;

        private readonly PropertyPath customPropertyPath;

        private readonly PropertyMethodInfoImpl getMethod = new PropertyMethodInfoImpl();


        public ExplicitProperty([NotNull] PropertyInfo interfaceProp, Type reflectedType, string baseName, Type propertyType, PropertyPath customPropertyPath = null)
        {
            this.ReflectedType = reflectedType ?? throw new ArgumentNullException(nameof(reflectedType));
            this.InterfaceProp = this.GetGenericInterfaceProp(interfaceProp ?? throw new ArgumentNullException(nameof(interfaceProp)));
            this.baseName = baseName ?? throw new ArgumentNullException(nameof(baseName));
            this.customPropertyPath = customPropertyPath;
            this.PropertyType = propertyType ?? throw new ArgumentNullException(nameof(propertyType));
        }


        public override Type PropertyType { get; }

        public override Type ReflectedType { get; }

        public override Type DeclaringType => this.ReflectedType;

        public override string Name => this.InterfaceProp.Name;// $"ExplicitProp_{this.baseProperty.Name}";


        private PropertyInfo GetGenericInterfaceProp(PropertyInfo baseProp)
        {
            if (baseProp == null) throw new ArgumentNullException(nameof(baseProp));

            if (baseProp.ReflectedType.IsGenericType)
            {
                var genericReflectedType = baseProp.ReflectedType.GetGenericTypeDefinition();

                return genericReflectedType.GetProperty(baseProp.Name, true);
            }

            return baseProp;
        }

        public override object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            return (object[])this.GetInternalCustomAttributes(attributeType).SelectMany().ToArray(attributeType);
        }

        private IEnumerable<IEnumerable<Attribute>> GetInternalCustomAttributes(Type attributeType)
        {
            if (attributeType.IsAssignableFrom(typeof(ExpandPathAttribute)))
            {
                yield return this.GetExpandPathAttributes().ToArray();
            }
        }

        public override object[] GetCustomAttributes(bool inherit)
        {
            return new Attribute[][]
            {
                this.GetExpandPathAttributes().ToArray()
            }.SelectMany().ToArray();
        }


        private IEnumerable<ExpandPathAttribute> GetExpandPathAttributes()
        {
            yield return this.customPropertyPath.Maybe(path => new ExpandPathAttribute(path)) ?? new ExpandPathAttribute(this.baseName);
        }

        public override ParameterInfo[] GetIndexParameters()
        {
            return new ParameterInfo[0];// this.sourceProperty.GetIndexParameters();
        }

        public override MethodInfo GetGetMethod(bool nonPublic)
        {
            return this.getMethod;
        }

        public override MethodInfo GetSetMethod(bool nonPublic)
        {
            return null;//new PropertyMethodInfoImpl();
        }
    }
}
