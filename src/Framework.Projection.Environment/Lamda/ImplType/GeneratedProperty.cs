using System;
using System.Linq;
using System.Reflection;

using Framework.Core;

using JetBrains.Annotations;

namespace Framework.Projection.Lambda
{
    internal class GeneratedProperty : BasePropertyInfoImpl
    {
        private readonly ProjectionLambdaEnvironment environment;

        private readonly IProjectionProperty projectionProperty;

        //private readonly PropertyInfo sourceProperty;

        private readonly Lazy<Type> lazyPropertyType;

        private readonly PropertyMethodInfoImpl getMethod = new PropertyMethodInfoImpl();


        public GeneratedProperty([NotNull] ProjectionLambdaEnvironment environment, [NotNull] IProjectionProperty projectionProperty, [NotNull] GeneratedType reflectedType)
        {
            this.environment = environment ?? throw new ArgumentNullException(nameof(environment));
            this.projectionProperty = projectionProperty ?? throw new ArgumentNullException(nameof(projectionProperty));

            this.ReflectedType = reflectedType ?? throw new ArgumentNullException(nameof(reflectedType));

            this.lazyPropertyType = LazyHelper.Create(() => this.environment.BuildPropertyType(projectionProperty.Type, reflectedType, this.Name));

            if (this.IsIdentity)
            {
                this.getMethod = new PropertyMethodInfoImpl(this.environment.IdentityProperty.GetGetMethod());
            }
        }

        public bool IsIdentity => this.projectionProperty.Name == this.environment.IdentityProperty.Name;




        public override Type PropertyType => this.lazyPropertyType.Value;

        public override Type ReflectedType { get; }

        public override Type DeclaringType => this.ReflectedType;

        public override string Name => this.projectionProperty.Name;


        public override object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            return (object[])this.projectionProperty.Attributes.Where(attributeType.IsInstanceOfType).ToArray(attributeType);
        }

        public override object[] GetCustomAttributes(bool inherit)
        {
            return this.projectionProperty.Attributes.ToArray();
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

        public override string ToString()
        {
            return $"GeneratedProperty: {this.Name}";
        }
    }
}
