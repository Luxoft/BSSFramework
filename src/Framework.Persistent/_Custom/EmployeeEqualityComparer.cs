using System;
using System.Collections.Generic;

namespace Framework.Persistent;

public abstract class EmployeeEqualityComparer : IEqualityComparer<IEmployee>
{
    public abstract bool Equals(IEmployee x, IEmployee y);

    public abstract int GetHashCode(IEmployee obj);


    public static readonly EmployeeEqualityComparer EMail = new EmployeeByEMailComparer();


    private class EmployeeByEMailComparer : EmployeeEqualityComparer
    {
        public override bool Equals(IEmployee x, IEmployee y)
        {
            return string.Equals(x.Email, y.Email, StringComparison.InvariantCultureIgnoreCase);
        }

        public override int GetHashCode(IEmployee obj)
        {
            return obj.Email.ToLower().GetHashCode();
        }
    }
}
