using System;
using System.Reflection;

using Framework.Core;
using Framework.DomainDriven.Generation.Domain;
using Framework.Persistent;

namespace Framework.DomainDriven.DTOGenerator;

public class DynamicCodeTypeReferenceService<TConfiguration> : LayerCodeTypeReferenceService<TConfiguration>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    private readonly RoleFileType _referenceFileType;

    private readonly RoleFileType _detailFileType;


    public DynamicCodeTypeReferenceService(TConfiguration configuration, RoleFileType referenceFileType, RoleFileType detailFileType)
            : base(configuration)
    {
        if (referenceFileType == null) throw new ArgumentNullException(nameof(referenceFileType));
        if (detailFileType == null) throw new ArgumentNullException(nameof(detailFileType));

        this._referenceFileType = referenceFileType;
        this._detailFileType = detailFileType;
    }


    public override RoleFileType GetReferenceFileType(PropertyInfo property)
    {
        if (property == null) throw new ArgumentNullException(nameof(property));

        return property.IsDetail() ? this._detailFileType : this._referenceFileType;
    }


    public override RoleFileType GetCollectionFileType(PropertyInfo property)
    {
        if (property.IsDetail())
        {
            return this._detailFileType;
        }

        if (property.IsNotDetail())
        {
            return this._referenceFileType;
        }

        if (!this.DomainTypeIsPersistent(property) && this.Configuration.IsPersistentObject(property.PropertyType.GetCollectionElementType()))
        {
            if (!property.IsDetail())
            {
                return this._referenceFileType;
            }
        }

        return this._detailFileType;
    }
}
