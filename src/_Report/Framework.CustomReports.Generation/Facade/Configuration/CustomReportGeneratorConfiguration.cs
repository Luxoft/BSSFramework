using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Framework.Core;
using Framework.CustomReports.Domain;
using Framework.CustomReports.Services;
using Framework.DomainDriven.BLLCoreGenerator;
using Framework.DomainDriven.Generation.Domain;
using Framework.DomainDriven.ServiceModelGenerator;

namespace Framework.CustomReports.Generation.Facade
{
    public class CustomReportServiceGeneratorConfiguration<TEnvironment> : DomainDriven.ServiceModelGenerator.GeneratorConfigurationBase<TEnvironment>, ICustomReportServiceGeneratorConfiguration<TEnvironment>
        where TEnvironment : class, ICustomReportServiceGenerationEnvironmentBase
    {
        public CustomReportServiceGeneratorConfiguration(
            TEnvironment environment)
            : base(environment)
        {
        }

        public override IGeneratePolicy<MethodIdentity> GeneratePolicy { get; } = GeneratePolicy<MethodIdentity>.AllowAll;

        protected override IEnumerable<Type> GetDomainTypes()
        {
            return this.Environment.ReportBLL.DomainTypes;
        }

        public override string ImplementClassName => this.GetClassName();

        protected override string NamespacePostfix => "ReportFacade";


        public virtual bool NeedCustomGetStreamMethodGenerate(Type type)
        {
            return true;
        }

        public override IEnumerable<IServiceMethodGenerator> GetMethodGenerators(Type parameterType)
        {
            if (parameterType == null) throw new ArgumentNullException("parameterType");

            if (this.NeedCustomGetStreamMethodGenerate(parameterType))
            {

                yield return new GetCustomReportStreamMethodGenerator<CustomReportServiceGeneratorConfiguration<TEnvironment>>(
                    this,
                    this.Environment.ReportBLL.Links.Single(z => z.ParameterType == parameterType,
                    () => new ArgumentException($"Expected custom report for type:'{nameof(parameterType)}'"),
                    () => new ArgumentException($"More then one custom report for parameter:'{nameof(parameterType)}'")).CustomReportType,
                    parameterType);
            }
        }

        protected virtual string GetClassName()
        {
            return $"{this.Environment.TargetSystemName}ReportFacade";
        }

        public virtual string GetServiceFacadeFileName(CodeTypeDeclaration typeDecl)
        {
            var suffix = "ReportFacade";
            if (typeDecl.IsInterface)
            {
                suffix += "Interface";
            }
            else if (typeDecl.Name.EndsWith("Facade"))
            {
                suffix += ".Impl";
            }

            return this.Environment.TargetSystemName + suffix + ".Generated";
        }
    }
}
