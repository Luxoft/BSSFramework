﻿using System;

using Framework.Core;
using Framework.DomainDriven.Generation;

namespace Framework.Workflow.TestGenerate
{
    public abstract class GeneratorsBase
    {
        protected ICheckOutService CheckOutService { get; } = Framework.DomainDriven.Generation.CheckOutService.Trace;

        protected virtual string FrameworkPath { get; } = System.Environment.CurrentDirectory.Replace(@"\",@"/").TakeWhileNot(@"/src/", StringComparison.InvariantCultureIgnoreCase);

        protected abstract string GeneratePath { get; }
    }
}
