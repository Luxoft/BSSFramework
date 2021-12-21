using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Configuration.Domain.Models.Custom.Reports;
using Framework.Core;
using Framework.CustomReports.Domain;
using Framework.Validation;

namespace Framework.CustomReports.Services
{
    internal class FilterEvaluator
    {
        public static FilterEvaluator Instance = new FilterEvaluator();


        private FilterEvaluator()
        {
            
        }

        public IEnumerable<EvaluatedFilter> Evaluate<TDomainSource>(ReportGenerationModel model)
        {
            var parameterNameDict = model.Items.ToDictionary(z => z.Parameter.Name.ToLower(), z => z);
            
            foreach (var filter in model.Report.Filters)
            {
                var propertyChain = typeof(TDomainSource)
                    .ToPropertyInfoChain(filter.Property.GetPropertyNameChain())
                    .Select(z => z.Name)
                    .ToArray();

                if (filter.IsValueFromParameters)
                {
                    ReportGenerationValue generationParameter;
                    if (parameterNameDict.TryGetValue(filter.Value.ToLower(), out generationParameter))
                    {
                        yield return new EvaluatedFilter(propertyChain, filter.FilterOperator, generationParameter.Value);
                    }
                    ////параметр не был указан, ничего не делаем.
                }
                else
                {
                    yield return new EvaluatedFilter(propertyChain, filter.FilterOperator, filter.Value);
                }
            }
        }
    }
}