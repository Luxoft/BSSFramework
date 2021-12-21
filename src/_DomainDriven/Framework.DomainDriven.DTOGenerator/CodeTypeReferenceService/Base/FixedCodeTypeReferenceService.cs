using System;
using System.Reflection;

namespace Framework.DomainDriven.DTOGenerator
{
    public class FixedCodeTypeReferenceService<TConfiguration> : LayerCodeTypeReferenceService<TConfiguration>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
    {
        private readonly RoleFileType _referenceFileType;

        private readonly RoleFileType _collectionFileType;

        private readonly bool _enabledSecurity;


        public FixedCodeTypeReferenceService(TConfiguration configuration, RoleFileType referenceFileType, RoleFileType collectionFileType, bool enabledSecurity = true)
            : base(configuration)
        {
            if (referenceFileType == null) throw new ArgumentNullException(nameof(referenceFileType));
            if (collectionFileType == null) throw new ArgumentNullException(nameof(collectionFileType));

            this._referenceFileType = referenceFileType;
            this._collectionFileType = collectionFileType;
            this._enabledSecurity = enabledSecurity;
        }


        public override bool IsOptional(PropertyInfo property)
        {
            return this._enabledSecurity && base.IsOptional(property);
        }

        public override RoleFileType GetReferenceFileType(PropertyInfo _)
        {
            return this._referenceFileType;
        }

        public override RoleFileType GetCollectionFileType(PropertyInfo _)
        {
            return this._collectionFileType;
        }
    }
}