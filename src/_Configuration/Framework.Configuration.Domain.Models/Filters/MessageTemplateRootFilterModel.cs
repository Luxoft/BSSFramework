using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;

using Framework.Core;

namespace Framework.Configuration.Domain
{
    public class MessageTemplateRootFilterModel : DomainObjectMultiFilterModel<MessageTemplate>
    {
        public MessageTemplateRootFilterModel()
        {
            this.WithUntyped = true;
        }

        public TargetSystem TargetSystem { get; set; }

        public DomainType DomainType { get; set; }

        [DefaultValue(true)]
        public bool WithUntyped { get; set; }


        public override Expression<Func<MessageTemplate, bool>> ToFilterExpression()
        {
            return base.ToFilterExpression().Pipe(this.WithUntyped, filter => filter.BuildOr(lambda => lambda.TargetSystem == null));
        }

        protected override IEnumerable<System.Linq.Expressions.Expression<Func<MessageTemplate, bool>>> ToFilterExpressionItems()
        {
            var domainType = this.DomainType;

            if (domainType != null)
            {
                yield return lambda => lambda.DomainType == domainType;
            }


            var targetSystem = this.TargetSystem;

            if (targetSystem != null)
            {
                yield return lambda => lambda.TargetSystem == targetSystem;
            }
        }
    }
}