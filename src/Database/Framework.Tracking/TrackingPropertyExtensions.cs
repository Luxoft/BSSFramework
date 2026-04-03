using CommonFramework;

namespace Framework.Tracking;

public static class TrackingPropertyExtensions
{
    public static MergeResult<T, T> ToMergeResult<T>(this TrackingProperty<IEnumerable<T>> trackingProperty)
    {
        var request = from currentValue in trackingProperty.CurrentValue

                      from previousValue in trackingProperty.PreviousValue

                      select new MergeResult<T, T>
                             {
                                     AddingItems = currentValue.Except(previousValue).ToList(),

                                     RemovingItems = previousValue.Except(currentValue).ToList(),

                                     CombineItems = (from prev in previousValue
                                                     join next in currentValue on prev equals next
                                                     select ValueTuple.Create(prev, next)).ToList()
                             };

        return request.GetValueOrDefault(MergeResult<T, T>.Empty);
    }
}
