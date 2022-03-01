using System;
using System.CodeDom;
using System.Collections.Generic;

using Framework.CodeDom;
using Framework.DomainDriven;
using Framework.DomainDriven.BLLCoreGenerator;
using Framework.DomainDriven.Generation.Domain;

namespace SampleSystem.CodeGenerate
{
    /// <summary>
    /// Кастомный фабричный класс для BLL-интерфейсов к доменным объектам (пример расширения с добавленеим обработки ComplexChangeModelType)
    /// </summary>
    public class SampleSystemBLLInterfaceFileFactory : BLLInterfaceFileFactory<BLLCoreGeneratorConfiguration>
    {
        public SampleSystemBLLInterfaceFileFactory(BLLCoreGeneratorConfiguration configuration, Type domainType)
            : base(configuration, domainType)
        {
        }

        protected override IEnumerable<CodeTypeMember> GetMembers()
        {
            foreach (var member in base.GetMembers())
            {
                yield return  member;
            }

            foreach (var complexChangeModelType in this.Configuration.Environment.GetModelTypes(this.DomainType, this.Configuration.ComplexChangeModelType))
            {
                var methodName = this.DomainType.GetModelMethodName(complexChangeModelType, SampleSystemModelRole.ComplexChange, false);

                complexChangeModelType.CheckDirectMode(DirectMode.In, true);

                yield return new CodeMemberMethod
                {
                    Name = methodName,
                    ReturnType = this.DomainType.ToTypeReference(),
                    Parameters =
                    {
                        complexChangeModelType.ToTypeReference().ToParameterDeclarationExpression("changeModel")
                    }
                };
            }
        }
    }
}
