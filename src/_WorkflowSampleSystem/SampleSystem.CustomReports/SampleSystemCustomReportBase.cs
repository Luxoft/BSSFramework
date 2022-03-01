using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Framework.CustomReports.Domain;

namespace SampleSystem.CustomReports
{
    public abstract class SampleSystemCustomReportBase<TParameter> : CustomReportBase<SampleSystemSecurityOperationCode, TParameter>
          where TParameter : new()
    {
    }
}
