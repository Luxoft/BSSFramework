using System;
using System.Linq.Expressions;

using Framework.Core;

namespace Framework.Configuration.Domain;

public class ExceptionMessageRootFilterModel : DomainObjectFilterModel<ExceptionMessage>
{
    public Period Period { get; set; }

    public bool IsRoot { get; set; }


    public override Expression<Func<ExceptionMessage, bool>> ToFilterExpression()
    {
        var period = new Period(this.Period.StartDate, this.Period.EndDate.MaybeNullableToNullable(d => d.ToEndDayDate()));
        var isRoot = this.IsRoot;

        return exceptionMessage => exceptionMessage.IsRoot == isRoot && period.ContainsExt(exceptionMessage.CreateDate);
    }
}
