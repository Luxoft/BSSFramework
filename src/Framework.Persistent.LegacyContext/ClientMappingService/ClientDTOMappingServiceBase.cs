using CommonFramework.Maybe;

using Framework.Core;
using Framework.Exceptions;

namespace Framework.Persistent;

public abstract class ClientDTOMappingServiceBase
{
    protected ClientDTOMappingServiceBase()
    {
    }

    protected virtual List<UpdateItemData<TTarget, TIdentity>> ExtractUpdateDataFromSingle<TSource, TIdentity, TTarget>(IEnumerable<TSource> currentSource, Func<TSource, TTarget> getTargetFromSingle)
            where TSource : class, IIdentityObjectContainer<TIdentity>
    {
        return currentSource.ToList(item => UpdateItemData.CreateSave<TTarget, TIdentity>(getTargetFromSingle(item)));
    }

    protected virtual List<UpdateItemData<TTarget, TIdentity>> ExtractSecurityUpdateDataFromSingle<TSource, TIdentity, TTarget>(Maybe<List<TSource>> currentSource, Func<TSource, TTarget> getTargetFromSingle)
            where TSource : class, IIdentityObjectContainer<TIdentity>
    {
        var tryCurrentSourceValue = currentSource.GetValueOrDefault();

        if (tryCurrentSourceValue == null)
        {
            return new List<UpdateItemData<TTarget, TIdentity>>();
        }
        else
        {
            return tryCurrentSourceValue.ToList(item => UpdateItemData.CreateSave<TTarget, TIdentity>(getTargetFromSingle(item)));
        }
    }

    protected virtual List<UpdateItemData<TTarget, TIdentity>> ExtractUpdateDataL<TSource, TIdentity, TTarget>(IEnumerable<TSource> currentSource, IEnumerable<TSource> baseSource, Func<TSource, TSource, TTarget> getTargetFromPair, Func<TSource, TTarget> getTargetFromSingle)
            where TSource : class, IIdentityObjectContainer<TIdentity>
    {
        return currentSource.ExtractUpdateData(baseSource, (currentSourceItem, baseSourceItem) => baseSourceItem == null ? getTargetFromSingle(currentSourceItem) : getTargetFromPair(currentSourceItem, baseSourceItem), v => v.Identity).ToList();
    }

    protected virtual List<UpdateItemData<TTarget, TIdentity>> ExtractSecurityUpdateDataL<TSource, TIdentity, TTarget>(Maybe<List<TSource>> currentSource, Maybe<List<TSource>> baseSource,  Func<TSource, TSource, TTarget> getTargetFromPair, Func<TSource, TTarget> getTargetFromSingle)
            where TSource : class, IIdentityObjectContainer<TIdentity>
    {
        var tryCurrentSourceValue = currentSource.GetValueOrDefault();

        var tryBaseSourceValue = baseSource.GetValueOrDefault();

        if (tryCurrentSourceValue == null)
        {
            return new List<UpdateItemData<TTarget, TIdentity>>();
        }
        else
        {
            if (tryBaseSourceValue == null)
            {
                return tryCurrentSourceValue.ToList(item => UpdateItemData.CreateSave<TTarget, TIdentity>(getTargetFromSingle(item)));
            }
            else
            {
                return this.ExtractUpdateDataL<TSource, TIdentity, TTarget>(tryCurrentSourceValue, tryBaseSourceValue, getTargetFromPair, getTargetFromSingle);
            }
        }
    }

    protected virtual T GetEqualsValue<T>(T currentValue, T baseValue, string propertyName)
    {
        if (!Equals(currentValue, baseValue))
        {
            throw new BusinessLogicException($"Property \"{propertyName}\" must be equals");
        }

        return currentValue;
    }
}
