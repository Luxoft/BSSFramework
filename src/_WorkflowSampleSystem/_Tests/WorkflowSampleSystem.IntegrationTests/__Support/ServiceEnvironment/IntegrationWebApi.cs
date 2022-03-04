﻿using System;

namespace WorkflowSampleSystem.IntegrationTests.__Support.ServiceEnvironment;

public class IntegrationWebApi : WebApiBase
{
    public IntegrationWebApi(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }


    public override ControllerEvaluator<TController> GetControllerEvaluator<TController>(string principalName = null) =>
            base.GetControllerEvaluator<TController>(principalName).WithIntegrationImpersonate();
}
