using System;
using System.CodeDom;
using System.Reflection;

using Framework.CodeDom;
using Framework.Security.Cryptography;


namespace Framework.DomainDriven.DTOGenerator.Server
{
    public class CryptDTOToDomainObjectPropertyAssigner<TConfiguration> : DTOToDomainObjectPropertyAssigner<TConfiguration>
        where TConfiguration : class, IServerGeneratorConfigurationBase<IServerGenerationEnvironmentBase>
    {
        public CryptDTOToDomainObjectPropertyAssigner(IDTOSource<TConfiguration> source)
            : base(source)
        {
        }


        public override CodeStatement GetAssignStatement(PropertyInfo property, CodeExpression sourcePropertyRef, CodeExpression targetPropertyRef)
        {
            var cryptSystem = property.GetCryptSystem();

            if (cryptSystem != null)
            {
                var binaryConverterExpr = this.MappingServiceRefExpr.ToPropertyReference("BinaryConverter");

                var decryptExpr = this.Configuration.GetCryptServiceProvider(this.ContextRef, cryptSystem)
                                      .ToMethodInvokeExpression((ICryptProvider provider) => provider.Decrypt(null), sourcePropertyRef);

                var convertFromBytesExpr = binaryConverterExpr.ToMethodInvokeExpression("Get" + property.PropertyType.Name, decryptExpr);

                return convertFromBytesExpr.ToAssignStatement(targetPropertyRef);
            }
            else
            {
                return base.GetAssignStatement(property, sourcePropertyRef, targetPropertyRef);
            }
        }
    }
}