using System.Reflection;

using Framework.Core;

namespace Framework.Projection
{
    internal class PropertyMethodInfoImpl : BaseMethodInfoImpl
    {
        private readonly MethodInfo baseDefinition;

        public PropertyMethodInfoImpl(MethodInfo baseDefinition = null)
        {
            this.baseDefinition = baseDefinition;
        }

        public override MethodInfo GetBaseDefinition()
        {
            return this.baseDefinition;
        }

        public override bool Equals(object obj)
        {
            return object.ReferenceEquals(this, obj);
        }
    }
}
