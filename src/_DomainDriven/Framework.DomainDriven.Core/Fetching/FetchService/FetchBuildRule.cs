using System;

using Framework.Core;
using Framework.OData;
using Framework.Transfering;

namespace Framework.DomainDriven;

public abstract class FetchBuildRule : IEquatable<FetchBuildRule>
{
    public abstract bool Equals(FetchBuildRule other);

    public override bool Equals(object obj)
    {
        return this.Equals(obj as FetchBuildRule);
    }

    public override int GetHashCode()
    {
        return 0;
    }


    public static implicit operator FetchBuildRule(MainDTOType dtoType)
    {
        return new DTOFetchBuildRule((ViewDTOType)dtoType);
    }

    public static implicit operator FetchBuildRule(ViewDTOType dtoType)
    {
        return new DTOFetchBuildRule(dtoType);
    }

    public static implicit operator FetchBuildRule(SelectOperation selectOperation)
    {
        return new ODataFetchBuildRule(selectOperation);
    }


    public class ODataFetchBuildRule : FetchBuildRule, IEquatable<ODataFetchBuildRule>
    {
        public readonly SelectOperation SelectOperation;


        public ODataFetchBuildRule(SelectOperation selectOperation)
        {
            if (selectOperation == null) throw new ArgumentNullException(nameof(selectOperation));

            this.SelectOperation = selectOperation;
        }


        public override bool Equals(FetchBuildRule other)
        {
            return this.Equals(other as ODataFetchBuildRule);
        }

        public override int GetHashCode()
        {
            return this.SelectOperation.GetHashCode();
        }

        public bool Equals(ODataFetchBuildRule other)
        {
            return other != null && this.SelectOperation.Equals(other.SelectOperation);
        }
    }

    public class DTOFetchBuildRule : FetchBuildRule, IEquatable<DTOFetchBuildRule>
    {
        public readonly ViewDTOType DTOType;


        public DTOFetchBuildRule(ViewDTOType dtoType)
        {
            this.DTOType = dtoType;
        }


        public override bool Equals(FetchBuildRule other)
        {
            return this.Equals(other as DTOFetchBuildRule);
        }

        public override int GetHashCode()
        {
            return this.DTOType.GetHashCode();
        }

        public bool Equals(DTOFetchBuildRule other)
        {
            return other != null && this.DTOType == other.DTOType;
        }
    }
}

public static class FetchBuildRuleExtensions
{
    public static TResult Match<TResult>(this FetchBuildRule buildRule, Func<FetchBuildRule.DTOFetchBuildRule, TResult> getByMain, Func<SelectOperation, TResult> getByOData)
    {
        if (buildRule == null) throw new ArgumentNullException(nameof(buildRule));
        if (getByMain == null) throw new ArgumentNullException(nameof(getByMain));
        if (getByOData == null) throw new ArgumentNullException(nameof(getByOData));

        if (buildRule is FetchBuildRule.DTOFetchBuildRule)
        {
            return getByMain(((FetchBuildRule.DTOFetchBuildRule) buildRule));
        }
        else if (buildRule is FetchBuildRule.ODataFetchBuildRule)
        {
            return getByOData((buildRule as FetchBuildRule.ODataFetchBuildRule).SelectOperation);
        }
        else
        {
            throw new ArgumentOutOfRangeException(nameof(buildRule));
        }
    }
}
