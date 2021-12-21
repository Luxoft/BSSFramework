using System;
using System.Collections.Generic;

using Framework.CustomReports.Domain;
using Framework.DomainDriven.Generation.Domain;

namespace Framework.CustomReports.Generation.BLL
{
    public interface ICustomReportBLLGeneratorConfiguration<out TEnvironment> : ICustomReportBLLGeneratorConfiguration, IGeneratorConfiguration<TEnvironment, FileType>
        where TEnvironment : IGenerationEnvironment
    {
    }

    public interface ICustomReportBLLGeneratorConfiguration : IGeneratorConfiguration, ICodeTypeReferenceService<FileType>
    {
        IEnumerable<CustomReportParameterLink> Links { get; }
    }
}