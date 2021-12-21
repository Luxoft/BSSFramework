using System;
using System.CodeDom;
using System.Reflection;

using Framework.CodeDom;
using Framework.Security.Cryptography;

namespace Framework.DomainDriven.DTOGenerator.Server
{
    public class CryptDomainObjectToDTOPropertyAssigner<TConfiguration> : DomainObjectToDTOPropertyAssigner<TConfiguration>
        where TConfiguration : class, IServerGeneratorConfigurationBase<IServerGenerationEnvironmentBase>
    {
        public CryptDomainObjectToDTOPropertyAssigner(IDTOSource<TConfiguration> source)
            : base(source)
        {
        }


        public override CodeStatement GetAssignStatement(PropertyInfo property, CodeExpression sourcePropertyRef, CodeExpression targetPropertyRef)
        {
            var cryptSystem = property.GetCryptSystem();

            if (cryptSystem != null)
            {
                var binaryConverterExpr = this.MappingServiceRefExpr.ToPropertyReference("BinaryConverter");

                var convertToBytesExpr = binaryConverterExpr.ToMethodInvokeExpression("GetBytes", sourcePropertyRef);

                var encryptExpr = this.Configuration.GetCryptServiceProvider(this.ContextRef, cryptSystem)
                                                    .ToMethodInvokeExpression((ICryptProvider provider) => provider.Encrypt(null), convertToBytesExpr);

                return encryptExpr.ToAssignStatement(targetPropertyRef);
            }
            else
            {
                return base.GetAssignStatement(property, sourcePropertyRef, targetPropertyRef);
            }
        }
    }
}