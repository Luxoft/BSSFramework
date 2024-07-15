﻿namespace Framework.SecuritySystem;

public class SecurityAccessorDataOptimizer : SecurityAccessorDataVisitor, ISecurityAccessorDataOptimizer
{
    public SecurityAccessorData Optimize(SecurityAccessorData data)
    {
        return this.Visit(data);
    }

    public override SecurityAccessorData Visit(SecurityAccessorData.AndSecurityAccessorData data)
    {
        if (data.Left is SecurityAccessorData.InfinitySecurityAccessorData)
        {
            return data.Right;
        }
        else if (data.Right is SecurityAccessorData.InfinitySecurityAccessorData)
        {
            return data.Left;
        }
        else if (data.Left is SecurityAccessorData.FixedSecurityAccessorData left
                 && data.Right is SecurityAccessorData.FixedSecurityAccessorData right)
        {
            return SecurityAccessorData.Return(left.Items.Intersect(right.Items, StringComparer.CurrentCultureIgnoreCase));
        }
        else
        {
            return data;
        }
    }

    public override SecurityAccessorData Visit(SecurityAccessorData.OrSecurityAccessorData data)
    {
        if (data.Left is SecurityAccessorData.InfinitySecurityAccessorData)
        {
            return data.Left;
        }
        else if (data.Right is SecurityAccessorData.InfinitySecurityAccessorData)
        {
            return data.Right;
        }
        else if (data.Left is SecurityAccessorData.FixedSecurityAccessorData left
                 && data.Right is SecurityAccessorData.FixedSecurityAccessorData right)
        {
            return SecurityAccessorData.Return(left.Items.Union(right.Items, StringComparer.CurrentCultureIgnoreCase));
        }
        else
        {
            return data;
        }
    }

    public override SecurityAccessorData Visit(SecurityAccessorData.NegateSecurityAccessorData data)
    {
        switch (data.InnerData)
        {
            case SecurityAccessorData.InfinitySecurityAccessorData:
                return SecurityAccessorData.Empty;

            case SecurityAccessorData.FixedSecurityAccessorData([]):
                return SecurityAccessorData.Infinity;

            case SecurityAccessorData.NegateSecurityAccessorData innerData:
                return innerData.InnerData;

            default:
                return data;
        }
    }
}
