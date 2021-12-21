using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Reflection;

using Framework.CodeDom;
using Framework.Core;
using Framework.Persistent;

namespace Framework.DomainDriven.DTOGenerator.Client
{
    public class DefaultVisualDTOFileFactory<TConfiguration> : MainDTOFileFactoryBase<TConfiguration>
        where TConfiguration : class, IClientGeneratorConfigurationBase<IClientGenerationEnvironmentBase>
    {
        private readonly Lazy<PropertyInfo> _visualIdentityProperty;

        public DefaultVisualDTOFileFactory(TConfiguration configuration, Type domainType)
            : base(configuration, domainType)
        {
            this._visualIdentityProperty = LazyHelper.Create(() => this.GetProperties(false).SingleOrDefault(property =>
               property.IsVisualIdentity() && property.PropertyType == typeof(string),
               properties => new Exception(
                   $"To many VisualIdentity Properties ({properties.Join(", ", prop => prop.Name)}) for type {this.DomainType.Name}")));
        }


        public override MainDTOFileType FileType { get; } = DTOGenerator.FileType.VisualDTO;

        public override CodeTypeReference CurrentInterfaceReference { get; } = null;

        private PropertyInfo VisualIdentityProperty => this._visualIdentityProperty.Value;


        protected override System.Collections.Generic.IEnumerable<CodeTypeMember> GetMembers()
        {
            foreach (var baseMember in base.GetMembers())
            {
                yield return baseMember;
            }

            if (this.IsPersistent())
            {
                yield return this.GetIdentityObjectContainerImplementation();
            }

            if (this.VisualIdentityProperty != null)
            {
                yield return CodeDomHelper.GenerateToStringMethod(this.VisualIdentityProperty);

                yield return CodeDomHelper.GenerateToCompareMethod(this.CurrentReference, this.VisualIdentityProperty);
            }
        }

        protected override IEnumerable<CodeConstructor> GetConstructors()
        {
            yield return this.GenerateDefaultConstructor();
            yield return this.GeneratePersistentCloneConstructor();
            yield return this.GeneratePersistentCloneConstructorWithCopyIdParameter();
        }

        protected override System.Collections.Generic.IEnumerable<CodeTypeReference> GetBaseTypes()
        {
            foreach (var baseType in base.GetBaseTypes())
            {
                yield return baseType;
            }

            if (this.IsPersistent())
            {
                yield return this.GetIdentityObjectContainerTypeReference();
            }

            if (this.DomainType.IsVisualIdentityObject())
            {
                yield return typeof(IVisualIdentityObject).ToTypeReference();
            }

            if (this.VisualIdentityProperty != null)
            {
                yield return this.CurrentReference.ToComparableReference();
            }
        }
    }
}
