using System.Reflection;

using Framework.CodeDom;
using Framework.Security.Cryptography;

namespace Framework.DomainDriven.DTOGenerator.Server
{
    public class CryptCodeTypeReferenceService<TConfiguration> : DynamicCodeTypeReferenceService<TConfiguration>
        where TConfiguration : class, IServerGeneratorConfigurationBase<IServerGenerationEnvironmentBase>
    {
        public CryptCodeTypeReferenceService(TConfiguration configuration, RoleFileType referenceFileType, RoleFileType collectionFileType)
            : base(configuration, referenceFileType, collectionFileType)
        {

        }


        public override bool IsOptional(PropertyInfo property)
        {
            return false;
        }

        protected override System.CodeDom.CodeTypeReference GetCodeTypeReferenceByProperty(PropertyInfo property)
        {
            if (property.GetCryptSystem() != null)
            {
                return typeof(byte[]).ToTypeReference();
            }
            else
            {
                return base.GetCodeTypeReferenceByProperty(property);
            }
        }
    }
}