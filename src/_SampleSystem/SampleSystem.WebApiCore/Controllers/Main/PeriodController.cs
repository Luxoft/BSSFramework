using Framework.Core;

using Microsoft.AspNetCore.Mvc;

namespace SampleSystem.WebApiCore.Controllers.Main;

[Route("api/[controller]/[action]")]
[ApiController]
public class PeriodController(TimeProvider timeProvider) : ControllerBase
{
    [HttpGet]
    public Period GetCurrentMonth() => timeProvider.GetCurrentMonth();
}
