﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ASP
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Framework.Configuration.SubscriptionModeling;
    using SampleSystem.Subscriptions.Metadata.DomainChangedByRecipients.NotPersistentCustomModel;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    public partial class _DomainChangedByRecipients_NotPersistentCustomModel_MessageTemplate_cshtml : RazorTemplate<CustomNotificationModel>
    {
#line hidden

    public override string Subject => $"Country {Current.Country.Code} has been updated";

        public override void Execute()
        {



                                





WriteLiteral("\n<html>\n    <head></head>\n    <body>\n    <p>Country with code: ");


                     Write(Current.Country.Code);

WriteLiteral(" was updated and has ");


                                                               Write(Current.LocationsCount);

WriteLiteral(" locations</p>\n    <img src=\"");


         Write(AttachmentLambda.AttachmentName);

WriteLiteral("\"/>\n    </body>\n</html>\n");


        }
    }
}
#pragma warning restore 1591
