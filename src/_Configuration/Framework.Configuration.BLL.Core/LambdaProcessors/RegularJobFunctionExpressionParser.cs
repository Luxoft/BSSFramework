using System;

using Framework.ExpressionParsers;
using Framework.DomainDriven.BLL;
using Framework.Configuration.Domain;

namespace Framework.Configuration.BLL
{
    public class RegularJobFunctionExpressionParser<TBLLContext> : LambdaObjectExpressionParser<RegularJob, Action<TBLLContext>>
    {
        public RegularJobFunctionExpressionParser(INativeExpressionParser parser)
            : base(parser)
        {

        }
    }
}