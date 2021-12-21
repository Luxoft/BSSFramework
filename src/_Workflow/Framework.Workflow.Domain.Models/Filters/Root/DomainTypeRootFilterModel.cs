using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;

using Framework.Workflow.Domain.Definition;

namespace Framework.Workflow.Domain
{
    public class DomainTypeRootFilterModel : DomainObjectMultiFilterModel<DomainType>
    {
        public TargetSystem TargetSystem { get; set; }

        [DefaultValue(true)]
        public bool IncludeBaseTargetSystem { get; set; }


        public bool? IsSource { get; set; }



        public DomainTypeRootFilterModel()
        {
            this.IncludeBaseTargetSystem = true;
        }


        protected override IEnumerable<Expression<Func<DomainType, bool>>> ToFilterExpressionItems()
        {
            yield return domainType => this.TargetSystem == null
                                    || this.TargetSystem == domainType.TargetSystem
                                    || (this.IncludeBaseTargetSystem && domainType.TargetSystem.IsBase);


            if (this.IsSource.HasValue)
            {
                var isSource = this.IsSource.Value;

                if (isSource)
                {
                    yield return domainType => domainType.WorkflowSources.Any();
                }
                else
                {
                    yield return domainType => !domainType.WorkflowSources.Any();
                }
            }
        }
    }
}