using System.Collections.Generic;
using System.Linq;

using Framework.Core;

namespace Framework.DomainDriven.BLL.Tracking
{
    public static class TrackingPropertyExtensions
    {
        public static MergeResult<T, T> ToMergeResult<T>(this TrackingProperty<IEnumerable<T>> trackingProperty)
        {
            var request = from currentValue in trackingProperty.CurrentValue

                          from previusValue in trackingProperty.PreviusValue

                          select new MergeResult<T, T>
                          {
                              AddingItems = currentValue.Except(previusValue).ToList(),

                              RemovingItems = previusValue.Except(currentValue).ToList(),

                              CombineItems = (from prev in previusValue
                                              join next in currentValue on prev equals next
                                              select TupleStruct.Create(prev, next)).ToList()
                          };

            return request.GetValueOrDefault(MergeResult<T, T>.Empty);
        }
    }
}