using System;
using System.CodeDom;
using System.Collections.Generic;

using Framework.CustomReports.Domain;
using Framework.DomainDriven.ServiceModelGenerator;

namespace Framework.CustomReports.Generation.Facade
{
    public interface ICustomReportServiceGeneratorConfiguration<out TEnvironment> : ICustomReportServiceGeneratorConfiguration, IGeneratorConfigurationBase<TEnvironment>
        where TEnvironment : ICustomReportServiceGenerationEnvironmentBase
    {
    }

    public interface ICustomReportServiceGeneratorConfiguration : IGeneratorConfigurationBase
    {
    }
}