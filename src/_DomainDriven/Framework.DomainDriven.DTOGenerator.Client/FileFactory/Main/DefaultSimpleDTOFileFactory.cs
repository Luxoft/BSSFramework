using System;
using System.Collections.Generic;
using System.Linq;
using System.CodeDom;
using System.Reflection;

using Framework.CodeDom;
using Framework.Core;
using Framework.Persistent;
using Framework.Security;

namespace Framework.DomainDriven.DTOGenerator.Client
{
    public class DefaultSimpleDTOFileFactory<TConfiguration> : MainDTOFileFactory<TConfiguration>
        where TConfiguration : class, IClientGeneratorConfigurationBase<IClientGenerationEnvironmentBase>
    {
        private readonly Lazy<PropertyInfo> _visualIdentityProperty;


        public DefaultSimpleDTOFileFactory(TConfiguration configuration, Type domainType)
            : base(configuration, domainType)
        {
            this._visualIdentityProperty = LazyHelper.Create(() => this.GetProperties(false).SingleOrDefault(property =>
                property.IsVisualIdentity() && property.PropertyType == typeof(string)));
        }


        public override MainDTOFileType FileType { get; } = DTOGenerator.FileType.SimpleDTO;


        public override CodeTypeReference BaseReference =>

            this.IsPersistent() ? this.Configuration.GetBaseAuditPersistentReference() : this.Configuration.GetBaseAbstractReference();


        private PropertyInfo VisualIdentityProperty => this._visualIdentityProperty.Value;


        protected override CodeExpression GetFieldInitExpression(PropertyInfo property)
        {
            if (!this.CodeTypeReferenceService.IsOptional(property))
            {
                if (property.PropertyType == typeof(Period))
                {
                    return typeof(Period).ToTypeReferenceExpression().ToPropertyReference("Eternity");
                }
            }

            return base.GetFieldInitExpression(property);
        }

        protected override System.Collections.Generic.IEnumerable<CodeTypeReference> GetBaseTypes()
        {
            foreach (var baseType in base.GetBaseTypes())
            {
                yield return baseType;
            }

            if (this.IsPersistent() && this.Configuration.GeneratePolicy.Used(this.DomainType, DTOGenerator.FileType.IdentityDTO))
            {
                yield return this.GetIdentityObjectContainerTypeReference();
            }

            if (this.DomainType.IsPropertyImplement(typeof(IPeriodObject)))
            {
                yield return typeof(IMutablePeriodObject).ToTypeReference();
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

        protected override System.Collections.Generic.IEnumerable<CodeTypeMember> GetMembers()
        {
            foreach (var baseMember in base.GetMembers())
            {
                yield return baseMember;
            }

            if (this.IsPersistent() && this.Configuration.GeneratePolicy.Used(this.DomainType, DTOGenerator.FileType.IdentityDTO))
            {
                yield return this.GetIdentityObjectContainerImplementation();
            }

            if (this.VisualIdentityProperty != null)
            {
                yield return CodeDomHelper.GenerateToStringMethod(this.VisualIdentityProperty);

                yield return CodeDomHelper.GenerateToCompareMethod(this.CurrentReference, this.VisualIdentityProperty);
            }
        }

        protected override IEnumerable<CodeAttributeDeclaration> GetCustomAttributes()
        {
            foreach (var customAttribute in base.GetCustomAttributes())
            {
                yield return customAttribute;
            }

            foreach (var attr in this.DomainType.GetDomainObjectAccessAttributes())
            {
                yield return attr.ToCodeAttributeDeclaration();
            }
        }
    }
}
